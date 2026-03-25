using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop
{
    public partial class EmployeeWindow : Window
    {
        private Employee _currentEmployee;

        public EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            _currentEmployee = employee;
            LoadProducts();
        }

        private void LoadProducts(string search = "")
        {
            using (var db = new AppDbContext())
            {
                var query = db.Products
                    .Include(p => p.Coffeehouse)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(p => p.Name.Contains(search));

                ProductsGrid.ItemsSource = query.ToList();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProducts(SearchBox.Text);
        }

        private void WriteOff_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selected)
            {
                var dialog = new WriteOffDialog(selected);
                if (dialog.ShowDialog() == true)
                {
                    using (var db = new AppDbContext())
                    {
                        var product = db.Products.Find(selected.Id);
                        if (product.Quantity < dialog.Amount)
                        {
                            MessageBox.Show("Недостаточно товара на складе");
                            return;
                        }
                        product.Quantity -= dialog.Amount;
                        db.SaveChanges();
                    }
                    LoadProducts();
                    MessageBox.Show("Расход успешно списан");
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для списания");
            }
        }
    }
}
