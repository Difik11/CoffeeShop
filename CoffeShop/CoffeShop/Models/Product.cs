using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = string.Empty;

        [Column("coffeehouse_id")]
        public int? CoffeeHouseId { get; set; }

        public Coffeehouse Coffeehouse { get; set; }
    }
}
