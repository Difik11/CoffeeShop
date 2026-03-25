using System.Linq;
using System.Windows;
using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop
{
    public partial class EmployeeDialog : Window
    {
        public Employee Employee { get; private set; }

        public EmployeeDialog(Employee employee = null)
        {
            InitializeComponent();
            LoadRoles();
            LoadCoffeehouses();

            if (employee != null)
            {
                Employee = employee;
                FullNameBox.Text = employee.FullName;
                PhoneBox.Text = employee.Phone;
                LoginBox.Text = employee.Login;

                using (var db = new AppDbContext())
                {
                    var role = db.Roles.Find(employee.RoleId);
                    RoleBox.SelectedItem = RoleBox.Items
                        .Cast<Role>()
                        .FirstOrDefault(r => r.Id == employee.RoleId);

                    if (employee.CoffeeHouseId != null)
                        CoffeehouseBox.SelectedItem = CoffeehouseBox.Items
                            .Cast<Coffeehouse>()
                            .FirstOrDefault(c => c.Id == employee.CoffeeHouseId);
                }
            }
            else
            {
                Employee = new Employee();
            }
        }

        private void LoadRoles()
        {
            using (var db = new AppDbContext())
            {
                RoleBox.ItemsSource = db.Roles.ToList();
                RoleBox.DisplayMemberPath = "Name";
            }
        }

        private void LoadCoffeehouses()
        {
            using (var db = new AppDbContext())
            {
                CoffeehouseBox.ItemsSource = db.Coffeehouses.ToList();
                CoffeehouseBox.DisplayMemberPath = "Name";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FullNameBox.Text))
            {
                MessageBox.Show("Введите ФИО сотрудника");
                return;
            }

            if (RoleBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль");
                return;
            }

            if (string.IsNullOrEmpty(LoginBox.Text))
            {
                MessageBox.Show("Введите логин");
                return;
            }

            Employee.FullName = FullNameBox.Text.Trim();
            Employee.Phone = PhoneBox.Text.Trim();
            Employee.Login = LoginBox.Text.Trim();
            Employee.RoleId = ((Role)RoleBox.SelectedItem).Id;

            if (CoffeehouseBox.SelectedItem != null)
                Employee.CoffeeHouseId = ((Coffeehouse)CoffeehouseBox.SelectedItem).Id;

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
