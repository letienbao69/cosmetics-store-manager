using CosmeticsStoreManager.Data;
using CosmeticsStoreManager.Models;
using Microsoft.Data.SqlClient;

namespace CosmeticsStoreManager.Repositories;

public class UserRepository
{
    public UserAccount? Login(string username, string password)
    {
        const string query = @"
SELECT UserId, Username, PasswordHash, Role
FROM Users
WHERE Username = @Username AND PasswordHash = @PasswordHash";

        var dt = DbHelper.ExecuteQuery(query,
            new SqlParameter("@Username", username),
            new SqlParameter("@PasswordHash", password)); // Bài nhóm: giữ mật khẩu 123456 dạng plain text cho dễ demo.

        if (dt.Rows.Count == 0) return null;

        var row = dt.Rows[0];
        return new UserAccount
        {
            UserId = Convert.ToInt32(row["UserId"]),
            Username = row["Username"].ToString() ?? "",
            PasswordHash = row["PasswordHash"].ToString() ?? "",
            Role = row["Role"].ToString() ?? ""
        };
    }
}
