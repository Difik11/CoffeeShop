using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        [Column("sale_date")]
        public DateTime SaleDate { get; set; }

        [Column("coffeehouse_id")]
        public int? CoffeeHouseId { get; set; }

        public Product Product { get; set; }
        public Coffeehouse Coffeehouse { get; set; }
    }
}
