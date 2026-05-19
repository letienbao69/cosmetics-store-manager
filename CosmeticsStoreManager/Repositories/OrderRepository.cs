using CosmeticsStoreManager.Data;
using CosmeticsStoreManager.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CosmeticsStoreManager.Repositories;

public class OrderRepository
{
    public int CreateOrder(int customerId, int userId, List<OrderItem> items)
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Đơn hàng chưa có sản phẩm.");

        using var conn = DbHelper.GetConnection();
        conn.Open();
        using var tran = conn.BeginTransaction();

        try
        {
            decimal total = items.Sum(x => x.LineTotal);

            using var insertOrder = new SqlCommand(@"
INSERT INTO Orders(OrderDate, CustomerId, UserId, TotalAmount)
OUTPUT INSERTED.OrderId
VALUES(GETDATE(), @CustomerId, @UserId, @TotalAmount)", conn, tran);

            insertOrder.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            insertOrder.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            var totalParam = insertOrder.Parameters.Add("@TotalAmount", SqlDbType.Decimal);
            totalParam.Precision = 18;
            totalParam.Scale = 2;
            totalParam.Value = total;

            int orderId = Convert.ToInt32(insertOrder.ExecuteScalar());

            foreach (var item in items)
            {
                // Trừ kho an toàn: chỉ trừ khi tồn kho còn đủ.
                using var updateStock = new SqlCommand(@"
UPDATE Products
SET QuantityInStock = QuantityInStock - @Quantity
WHERE ProductId = @ProductId AND QuantityInStock >= @Quantity", conn, tran);

                updateStock.Parameters.Add("@Quantity", SqlDbType.Int).Value = item.Quantity;
                updateStock.Parameters.Add("@ProductId", SqlDbType.Int).Value = item.ProductId;

                int affected = updateStock.ExecuteNonQuery();
                if (affected == 0)
                    throw new Exception($"Sản phẩm {item.ProductName} không đủ tồn kho hoặc không tồn tại.");

                using var insertDetail = new SqlCommand(@"
INSERT INTO OrderDetails(OrderId, ProductId, Quantity, UnitPrice, LineTotal)
VALUES(@OrderId, @ProductId, @Quantity, @UnitPrice, @LineTotal)", conn, tran);

                insertDetail.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;
                insertDetail.Parameters.Add("@ProductId", SqlDbType.Int).Value = item.ProductId;
                insertDetail.Parameters.Add("@Quantity", SqlDbType.Int).Value = item.Quantity;

                var unitPriceParam = insertDetail.Parameters.Add("@UnitPrice", SqlDbType.Decimal);
                unitPriceParam.Precision = 18;
                unitPriceParam.Scale = 2;
                unitPriceParam.Value = item.UnitPrice;

                var lineTotalParam = insertDetail.Parameters.Add("@LineTotal", SqlDbType.Decimal);
                lineTotalParam.Precision = 18;
                lineTotalParam.Scale = 2;
                lineTotalParam.Value = item.LineTotal;

                insertDetail.ExecuteNonQuery();
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

    public DataTable GetRevenueByDate()
    {
        const string query = @"
SELECT CONVERT(date, OrderDate) AS [Date], SUM(TotalAmount) AS Revenue
FROM Orders
GROUP BY CONVERT(date, OrderDate)
ORDER BY [Date] DESC";

        return DbHelper.ExecuteQuery(query);
    }

    public DataTable GetLowStockProducts()
    {
        const string query = @"
SELECT ProductCode, ProductName, QuantityInStock
FROM Products
WHERE QuantityInStock <= 10
ORDER BY QuantityInStock ASC";

        return DbHelper.ExecuteQuery(query);
    }

    public DataTable GetBestSellingProducts()
    {
        const string query = @"
SELECT TOP 10
    p.ProductCode,
    p.ProductName,
    SUM(od.Quantity) AS TotalSold,
    SUM(od.LineTotal) AS Revenue
FROM OrderDetails od
JOIN Products p ON p.ProductId = od.ProductId
GROUP BY p.ProductCode, p.ProductName
ORDER BY TotalSold DESC";

        return DbHelper.ExecuteQuery(query);
    }
}
