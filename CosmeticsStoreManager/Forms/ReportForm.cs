using CosmeticsStoreManager.Repositories;
using CosmeticsStoreManager.UI;

namespace CosmeticsStoreManager.Forms;

public class ReportForm : Form
{
    private readonly OrderRepository _repo = new();
    private readonly DataGridView dgvRevenue = new() { Dock = DockStyle.Fill };
    private readonly DataGridView dgvLowStock = new() { Dock = DockStyle.Fill };

    public ReportForm()
    {
        Theme.ApplyForm(this, "Báo cáo thống kê");
        Size = new Size(1180, 740);

        Theme.StyleGrid(dgvRevenue);
        Theme.StyleGrid(dgvLowStock);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1,
            Padding = new Padding(20)
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        var header = Theme.CreateCard(18);
        header.Dock = DockStyle.Fill;
        header.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "Báo cáo thống kê\nTheo dõi doanh thu theo ngày và danh sách sản phẩm sắp hết hàng.",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Theme.TextPrimary
        });

        var revenueCard = Theme.CreateCard(14);
        revenueCard.Dock = DockStyle.Fill;
        revenueCard.Controls.Add(dgvRevenue);

        var revenueTitle = new Label
        {
            Text = "Doanh thu theo ngày",
            Dock = DockStyle.Top,
            Height = 36,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Theme.TextPrimary
        };
        revenueCard.Controls.Add(revenueTitle);
        revenueTitle.BringToFront();

        var stockCard = Theme.CreateCard(14);
        stockCard.Dock = DockStyle.Fill;
        stockCard.Controls.Add(dgvLowStock);

        var stockTitle = new Label
        {
            Text = "Sản phẩm sắp hết",
            Dock = DockStyle.Top,
            Height = 36,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Theme.TextPrimary
        };
        stockCard.Controls.Add(stockTitle);
        stockTitle.BringToFront();

        root.Controls.Add(header, 0, 0);
        root.Controls.Add(revenueCard, 0, 1);
        root.Controls.Add(stockCard, 0, 2);

        Controls.Add(root);
        LoadData();
    }

    private void LoadData()
    {
        dgvRevenue.DataSource = _repo.GetRevenueByDate();
        dgvLowStock.DataSource = _repo.GetLowStockProducts();
    }
}
