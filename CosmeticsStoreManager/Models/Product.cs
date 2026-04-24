namespace CosmeticsStoreManager.Models;

public class Product
{
    public int ProductId { get; set; }
    public string ProductCode { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string Category { get; set; } = "";
    public string Brand { get; set; } = "";
    public decimal ImportPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int QuantityInStock { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Description { get; set; } = "";
}
