using System.ComponentModel.DataAnnotations;

namespace SmartShop.Inventory
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int StoreId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        public decimal SalePrice { get; set; }
        
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Product Product { get; set; }
        public Store Store { get; set; }
    }
}
