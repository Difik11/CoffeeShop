using System.Windows;
using CoffeeShop.Models;

namespace CoffeeShop
{
    public partial class WriteOffDialog : Window
    {
        public decimal Amount { get; private set; }

        public WriteOffDialog(Product product)
        {
            InitializeComponent();
            ProductNameText.Text = $"Товар: {product.Name}";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(AmountBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }

            Amount = amount;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
