using CosmeticsStoreManager.Data;
using CosmeticsStoreManager.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CosmeticsStoreManager.Repositories;

public class CustomerRepository
{
    public List<Customer> GetAll(string? keyword = null)
    {
        const string query = @"
SELECT CustomerId, FullName, Phone, Address, Email
FROM Customers
WHERE (@Keyword IS NULL OR FullName LIKE '%' + @Keyword + '%' OR Phone LIKE '%' + @Keyword + '%' OR Email LIKE '%' + @Keyword + '%')
ORDER BY CustomerId DESC";

        var dt = DbHelper.ExecuteQuery(query,
            new SqlParameter("@Keyword", string.IsNullOrWhiteSpace(keyword) ? DBNull.Value : keyword));

        return dt.AsEnumerable().Select(r => new Customer
        {
            CustomerId = Convert.ToInt32(r["CustomerId"]),
            FullName = r["FullName"].ToString() ?? "",
            Phone = r["Phone"].ToString() ?? "",
            Address = r["Address"].ToString() ?? "",
            Email = r["Email"].ToString() ?? ""
        }).ToList();
    }

    public void Add(Customer c)
    {
        const string query = @"
INSERT INTO Customers(FullName, Phone, Address, Email)
VALUES(@FullName, @Phone, @Address, @Email)";

        DbHelper.ExecuteNonQuery(query,
            new SqlParameter("@FullName", c.FullName),
            new SqlParameter("@Phone", c.Phone),
            new SqlParameter("@Address", string.IsNullOrWhiteSpace(c.Address) ? DBNull.Value : c.Address),
            new SqlParameter("@Email", string.IsNullOrWhiteSpace(c.Email) ? DBNull.Value : c.Email));
    }

    public void Update(Customer c)
    {
        const string query = @"
UPDATE Customers
SET FullName=@FullName, Phone=@Phone, Address=@Address, Email=@Email
WHERE CustomerId=@CustomerId";

        DbHelper.ExecuteNonQuery(query,
            new SqlParameter("@CustomerId", c.CustomerId),
            new SqlParameter("@FullName", c.FullName),
            new SqlParameter("@Phone", c.Phone),
            new SqlParameter("@Address", string.IsNullOrWhiteSpace(c.Address) ? DBNull.Value : c.Address),
            new SqlParameter("@Email", string.IsNullOrWhiteSpace(c.Email) ? DBNull.Value : c.Email));
    }

    public void Delete(int customerId)
    {
        const string query = "DELETE FROM Customers WHERE CustomerId=@CustomerId";
        DbHelper.ExecuteNonQuery(query, new SqlParameter("@CustomerId", customerId));
    }
}
