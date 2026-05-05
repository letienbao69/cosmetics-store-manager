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
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        // Header
        var header = Theme.CreateCard(20);
        header.Dock = DockStyle.Fill;

        var headerLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent
        };
        headerLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        headerLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        headerLayout.Controls.Add(Theme.CreatePageTitle("Báo cáo thống kê"), 0, 0);
        headerLayout.Controls.Add(Theme.CreateSubtitle(
            "Theo dõi doanh thu theo ngày và danh sách sản phẩm sắp hết hàng."), 0, 1);
        header.Controls.Add(headerLayout);

        // Revenue card
        var revenueCard = Theme.CreateCard(14);
        revenueCard.Dock = DockStyle.Fill;

        var revenueTitle = new Label
        {
            Text = "📈  Doanh thu theo ngày",
            Dock = DockStyle.Top,
            Height = 38,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Theme.Primary,
            Padding = new Padding(4, 6, 0, 0)
        };
        revenueCard.Controls.Add(dgvRevenue);
        revenueCard.Controls.Add(revenueTitle);
        revenueTitle.BringToFront();

        // Low stock card
        var stockCard = Theme.CreateCard(14);
        stockCard.Dock = DockStyle.Fill;

        var stockTitle = new Label
        {
            Text = "⚠️  Sản phẩm sắp hết hàng",
            Dock = DockStyle.Top,
            Height = 38,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Theme.Primary,
            Padding = new Padding(4, 6, 0, 0)
        };
        stockCard.Controls.Add(dgvLowStock);
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