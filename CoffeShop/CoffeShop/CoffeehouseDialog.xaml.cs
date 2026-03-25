using System.Windows;
using CoffeeShop.Models;

namespace CoffeeShop
{
    public partial class CoffeehouseDialog : Window
    {
        public Coffeehouse Coffeehouse { get; private set; }

        public CoffeehouseDialog(Coffeehouse coffeehouse = null)
        {
            InitializeComponent();

            if (coffeehouse != null)
            {
                Coffeehouse = coffeehouse;
                NameBox.Text = coffeehouse.Name;
                AddressBox.Text = coffeehouse.Address;
                PhoneBox.Text = coffeehouse.Phone;
            }
            else
            {
                Coffeehouse = new Coffeehouse();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameBox.Text))
            {
                MessageBox.Show("Введите название кофейни");
                return;
            }

            if (string.IsNullOrEmpty(AddressBox.Text))
            {
                MessageBox.Show("Введите адрес кофейни");
                return;
            }

            Coffeehouse.Name = NameBox.Text.Trim();
            Coffeehouse.Address = AddressBox.Text.Trim();
            Coffeehouse.Phone = PhoneBox.Text.Trim();

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
