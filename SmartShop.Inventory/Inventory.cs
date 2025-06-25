using System.ComponentModel.DataAnnotations;

namespace SmartShop.Inventory
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int StoreId { get; set; }
        
        public int Quantity { get; set; } = 0;
        
        public DateTime? LastRestocked { get; set; }
        
        // Navigation properties
        public Product Product { get; set; }
        public Store Store { get; set; }
    }
}
