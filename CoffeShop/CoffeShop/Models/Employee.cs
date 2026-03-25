using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        public string Phone { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("coffeehouse_id")]
        public int? CoffeeHouseId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
        public Coffeehouse Coffeehouse { get; set; }
    }
}
