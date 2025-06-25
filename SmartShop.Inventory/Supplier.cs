using System.ComponentModel.DataAnnotations;

namespace SmartShop.Inventory
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(100)]
        public string ContactPerson { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        [Phone]
        public string Phone { get; set; }
    }
}
