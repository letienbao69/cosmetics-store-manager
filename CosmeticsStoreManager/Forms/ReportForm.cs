using CosmeticsStoreManager.Repositories;
using CosmeticsStoreManager.UI;

namespace CosmeticsStoreManager.Forms;

public class ReportForm : Form
{
    private readonly OrderRepository _repo = new();
    private readonly DataGridView dgvRevenue = new() { Dock = DockStyle.Fill };
    private readonly DataGridView dgvLowStock = new() { Dock = DockStyle.Fill };
    private readonly DataGridView dgvBestSelling = new() { Dock = DockStyle.Fill };

    public ReportForm()
    {
        Theme.ApplyForm(this, "Báo cáo thống kê");
        Size = new Size(1180, 820);
        AutoScroll = true;
        Theme.StyleGrid(dgvRevenue);
        Theme.StyleGrid(dgvLowStock);
        Theme.StyleGrid(dgvBestSelling);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            ColumnCount = 1,
            Padding = new Padding(20)
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 33));

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
            "Theo dõi doanh thu theo ngày, sản phẩm bán chạy và danh sách sản phẩm sắp hết hàng."), 0, 1);
        header.Controls.Add(headerLayout);

        root.Controls.Add(header, 0, 0);
        root.Controls.Add(CreateGridCard("📈  Doanh thu theo ngày", dgvRevenue), 0, 1);
        root.Controls.Add(CreateGridCard("🏆  Sản phẩm bán chạy", dgvBestSelling), 0, 2);
        root.Controls.Add(CreateGridCard("⚠️  Sản phẩm sắp hết hàng", dgvLowStock), 0, 3);
        Controls.Add(root);

        LoadData();
    }

    private Control CreateGridCard(string title, DataGridView grid)
    {
        var card = Theme.CreateCard(14);
        card.Dock = DockStyle.Fill;

        var label = new Label
        {
            Text = title,
            Dock = DockStyle.Top,
            Height = 38,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Theme.Primary,
            Padding = new Padding(4, 6, 0, 0)
        };
        card.Controls.Add(grid);
        card.Controls.Add(label);
        label.BringToFront();
        return card;
    }

    private void LoadData()
    {
        dgvRevenue.DataSource = _repo.GetRevenueByDate();
        dgvBestSelling.DataSource = _repo.GetBestSellingProducts();
        dgvLowStock.DataSource = _repo.GetLowStockProducts();
    }
}
