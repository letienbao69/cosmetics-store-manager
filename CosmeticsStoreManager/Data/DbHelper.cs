using Microsoft.Data.SqlClient;
using System.Data;

namespace CosmeticsStoreManager.Data;

public static class DbHelper
{
    // Mặc định dùng SQL Server Express local. Nếu máy bạn khác server thì sửa tại đây.
    // Ví dụ: Server=LAPTOP-M8KICGBD\SQLEXPRESS;Database=CosmeticsStoreDB;...
    private static readonly string _connectionString =
        @"Server=.\SQLEXPRESS;Database=CosmeticsStoreDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
    {
        using var conn = GetConnection();
        using var cmd = new SqlCommand(query, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);

        using var da = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(query, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteNonQuery();
    }

    public static object? ExecuteScalar(string query, params SqlParameter[] parameters)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(query, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteScalar();
    }
}
