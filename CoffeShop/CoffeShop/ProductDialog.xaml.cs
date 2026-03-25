using System.Windows;
using CoffeeShop.Models;

namespace CoffeeShop
{
    public partial class ProductDialog : Window
    {
        public Product Product { get; private set; }

        public ProductDialog(Product product = null)
        {
            InitializeComponent();

            if (product != null)
            {
                Product = product;
                NameBox.Text = product.Name;
                CategoryBox.Text = product.Category;
                QuantityBox.Text = product.Quantity.ToString();
            }
            else
            {
                Product = new Product();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameBox.Text))
            {
                MessageBox.Show("Введите название товара");
                return;
            }

            if (!decimal.TryParse(QuantityBox.Text, out decimal quantity))
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }

            Product.Name = NameBox.Text.Trim();
            Product.Category = CategoryBox.Text.Trim();
            Product.Quantity = quantity;

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
