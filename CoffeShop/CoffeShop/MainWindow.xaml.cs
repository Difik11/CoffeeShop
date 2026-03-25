using System.Linq;
using System.Windows;
using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ErrorText.Text = "Введите логин и пароль";
                return;
            }

            using (var db = new AppDbContext())
            {
                var employee = db.Employees
                    .Include(emp => emp.Role)
                    .FirstOrDefault(emp => emp.Login == login && emp.Password == password);

                if (employee == null)
                {
                    ErrorText.Text = "Неверный логин или пароль";
                    return;
                }

                if (employee.Role.Name == "Администратор")
                {
                    AdminWindow adminWindow = new AdminWindow(employee);
                    adminWindow.Show();
                    this.Close();
                }
                else
                {
                    EmployeeWindow employeeWindow = new EmployeeWindow(employee);
                    employeeWindow.Show();
                    this.Close();
                }
            }
        }
    }
}
