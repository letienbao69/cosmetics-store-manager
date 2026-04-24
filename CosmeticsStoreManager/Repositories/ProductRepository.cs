using CosmeticsStoreManager.Data;
using CosmeticsStoreManager.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CosmeticsStoreManager.Repositories;

public class ProductRepository
{
    public List<Product> GetAll(string? keyword = null)
    {
        string query = @"
SELECT ProductId, ProductCode, ProductName, Category, Brand, ImportPrice, SalePrice, QuantityInStock, ExpiryDate, Description
FROM Products
WHERE (@Keyword IS NULL OR ProductName LIKE '%' + @Keyword + '%' OR ProductCode LIKE '%' + @Keyword + '%')
ORDER BY ProductId DESC";

        var dt = DbHelper.ExecuteQuery(query,
            new SqlParameter("@Keyword", string.IsNullOrWhiteSpace(keyword) ? DBNull.Value : keyword));

        return dt.AsEnumerable().Select(r => new Product
        {
            ProductId = Convert.ToInt32(r["ProductId"]),
            ProductCode = r["ProductCode"].ToString() ?? "",
            ProductName = r["ProductName"].ToString() ?? "",
            Category = r["Category"].ToString() ?? "",
            Brand = r["Brand"].ToString() ?? "",
            ImportPrice = Convert.ToDecimal(r["ImportPrice"]),
            SalePrice = Convert.ToDecimal(r["SalePrice"]),
            QuantityInStock = Convert.ToInt32(r["QuantityInStock"]),
            ExpiryDate = r["ExpiryDate"] == DBNull.Value ? null : Convert.ToDateTime(r["ExpiryDate"]),
            Description = r["Description"].ToString() ?? ""
        }).ToList();
    }

    public void Add(Product p)
    {
        const string query = @"
INSERT INTO Products(ProductCode, ProductName, Category, Brand, ImportPrice, SalePrice, QuantityInStock, ExpiryDate, Description)
VALUES(@ProductCode, @ProductName, @Category, @Brand, @ImportPrice, @SalePrice, @QuantityInStock, @ExpiryDate, @Description)";

        DbHelper.ExecuteNonQuery(query,
            new SqlParameter("@ProductCode", p.ProductCode),
            new SqlParameter("@ProductName", p.ProductName),
            new SqlParameter("@Category", p.Category),
            new SqlParameter("@Brand", p.Brand),
            new SqlParameter("@ImportPrice", p.ImportPrice),
            new SqlParameter("@SalePrice", p.SalePrice),
            new SqlParameter("@QuantityInStock", p.QuantityInStock),
            new SqlParameter("@ExpiryDate", (object?)p.ExpiryDate ?? DBNull.Value),
            new SqlParameter("@Description", p.Description));
    }

    public void Update(Product p)
    {
        const string query = @"
UPDATE Products
SET ProductCode=@ProductCode, ProductName=@ProductName, Category=@Category, Brand=@Brand,
    ImportPrice=@ImportPrice, SalePrice=@SalePrice, QuantityInStock=@QuantityInStock,
    ExpiryDate=@ExpiryDate, Description=@Description
WHERE ProductId=@ProductId";

        DbHelper.ExecuteNonQuery(query,
            new SqlParameter("@ProductId", p.ProductId),
            new SqlParameter("@ProductCode", p.ProductCode),
            new SqlParameter("@ProductName", p.ProductName),
            new SqlParameter("@Category", p.Category),
            new SqlParameter("@Brand", p.Brand),
            new SqlParameter("@ImportPrice", p.ImportPrice),
            new SqlParameter("@SalePrice", p.SalePrice),
            new SqlParameter("@QuantityInStock", p.QuantityInStock),
            new SqlParameter("@ExpiryDate", (object?)p.ExpiryDate ?? DBNull.Value),
            new SqlParameter("@Description", p.Description));
    }

    public void Delete(int productId)
    {
        const string query = "DELETE FROM Products WHERE ProductId = @ProductId";
        DbHelper.ExecuteNonQuery(query, new SqlParameter("@ProductId", productId));
    }

    public Product? GetById(int productId)
    {
        const string query = @"
SELECT ProductId, ProductCode, ProductName, Category, Brand, ImportPrice, SalePrice, QuantityInStock, ExpiryDate, Description
FROM Products
WHERE ProductId=@ProductId";

        var dt = DbHelper.ExecuteQuery(query, new SqlParameter("@ProductId", productId));
        if (dt.Rows.Count == 0) return null;

        var r = dt.Rows[0];
        return new Product
        {
            ProductId = Convert.ToInt32(r["ProductId"]),
            ProductCode = r["ProductCode"].ToString() ?? "",
            ProductName = r["ProductName"].ToString() ?? "",
            Category = r["Category"].ToString() ?? "",
            Brand = r["Brand"].ToString() ?? "",
            ImportPrice = Convert.ToDecimal(r["ImportPrice"]),
            SalePrice = Convert.ToDecimal(r["SalePrice"]),
            QuantityInStock = Convert.ToInt32(r["QuantityInStock"]),
            ExpiryDate = r["ExpiryDate"] == DBNull.Value ? null : Convert.ToDateTime(r["ExpiryDate"]),
            Description = r["Description"].ToString() ?? ""
        };
    }
}
