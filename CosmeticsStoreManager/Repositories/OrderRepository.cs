using CosmeticsStoreManager.Data;
using CosmeticsStoreManager.Models;
using Microsoft.Data.SqlClient;

namespace CosmeticsStoreManager.Repositories;

public class OrderRepository
{
    public int CreateOrder(int customerId, int userId, List<OrderItem> items)
    {
        using var conn = DbHelper.GetConnection();
        conn.Open();
        using var tran = conn.BeginTransaction();

        try
        {
            decimal total = items.Sum(x => x.LineTotal);

            var insertOrder = new SqlCommand(@"
INSERT INTO Orders(OrderDate, CustomerId, UserId, TotalAmount)
OUTPUT INSERTED.OrderId
VALUES(GETDATE(), @CustomerId, @UserId, @TotalAmount)", conn, tran);

            insertOrder.Parameters.AddWithValue("@CustomerId", customerId);
            insertOrder.Parameters.AddWithValue("@UserId", userId);
            insertOrder.Parameters.AddWithValue("@TotalAmount", total);

            int orderId = Convert.ToInt32(insertOrder.ExecuteScalar());

            foreach (var item in items)
            {
                var stockCheckCmd = new SqlCommand("SELECT QuantityInStock FROM Products WHERE ProductId=@ProductId", conn, tran);
                stockCheckCmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                int stock = Convert.ToInt32(stockCheckCmd.ExecuteScalar());

                if (stock < item.Quantity)
                    throw new Exception($"Sản phẩm ID {item.ProductId} không đủ tồn kho.");

                var insertDetail = new SqlCommand(@"
INSERT INTO OrderDetails(OrderId, ProductId, Quantity, UnitPrice, LineTotal)
VALUES(@OrderId, @ProductId, @Quantity, @UnitPrice, @LineTotal)", conn, tran);

                insertDetail.Parameters.AddWithValue("@OrderId", orderId);
                insertDetail.Parameters.AddWithValue("@ProductId", item.ProductId);
                insertDetail.Parameters.AddWithValue("@Quantity", item.Quantity);
                insertDetail.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                insertDetail.Parameters.AddWithValue("@LineTotal", item.LineTotal);
                insertDetail.ExecuteNonQuery();

                var updateStock = new SqlCommand(@"
UPDATE Products
SET QuantityInStock = QuantityInStock - @Quantity
WHERE ProductId=@ProductId", conn, tran);

                updateStock.Parameters.AddWithValue("@Quantity", item.Quantity);
                updateStock.Parameters.AddWithValue("@ProductId", item.ProductId);
                updateStock.ExecuteNonQuery();
            }

            tran.Commit();
            return orderId;
        }
        catch
        {
            tran.Rollback();
            throw;
        }
    }

    public System.Data.DataTable GetRevenueByDate()
    {
        const string query = @"
SELECT CONVERT(date, OrderDate) AS [Date], SUM(TotalAmount) AS Revenue
FROM Orders
GROUP BY CONVERT(date, OrderDate)
ORDER BY [Date] DESC";

        return DbHelper.ExecuteQuery(query);
    }

    public System.Data.DataTable GetLowStockProducts()
    {
        const string query = @"
SELECT ProductCode, ProductName, QuantityInStock
FROM Products
WHERE QuantityInStock <= 10
ORDER BY QuantityInStock ASC";

        return DbHelper.ExecuteQuery(query);
    }
}
