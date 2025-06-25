using System.ComponentModel.DataAnnotations;

namespace SmartShop.Inventory
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Location { get; set; }
        
        [Phone]
        public string ContactNumber { get; set; }
    }
}
