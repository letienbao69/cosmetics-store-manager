using CosmeticsStoreManager.UI;

namespace CosmeticsStoreManager.Forms;

public class MainForm : Form
{
    public MainForm()
    {
        Theme.ApplyForm(this, "Trang chủ");
        WindowState = FormWindowState.Maximized;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var sidebar = BuildSidebar();
        var content = BuildContent();

        root.Controls.Add(sidebar, 0, 0);
        root.Controls.Add(content, 1, 0);
        Controls.Add(root);
    }

    private Panel BuildSidebar()
    {
        var sidebar = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Theme.Sidebar,
            Padding = new Padding(0, 24, 0, 0)
        };

        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 120,
            Padding = new Padding(20, 0, 20, 0)
        };
        header.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "MỸ PHẨM\nMANAGER",
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = false
        });

        var btnLogout = Theme.CreateSidebarButton("Đăng xuất");
        btnLogout.Click += (_, _) => Close();

        var btnReports = Theme.CreateSidebarButton("Báo cáo thống kê");
        btnReports.Click += (_, _) => new ReportForm().ShowDialog();

        var btnOrders = Theme.CreateSidebarButton("Quản lý đơn hàng");
        btnOrders.Click += (_, _) => new OrderForm().ShowDialog();

        var btnCustomers = Theme.CreateSidebarButton("Quản lý khách hàng");
        btnCustomers.Click += (_, _) => new CustomerForm().ShowDialog();

        var btnProducts = Theme.CreateSidebarButton("Quản lý sản phẩm");
        btnProducts.Click += (_, _) => new ProductForm().ShowDialog();

        if (Session.Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
            btnReports.Enabled = false;

        sidebar.Controls.Add(btnLogout);
        sidebar.Controls.Add(btnReports);
        sidebar.Controls.Add(btnOrders);
        sidebar.Controls.Add(btnCustomers);
        sidebar.Controls.Add(btnProducts);
        sidebar.Controls.Add(header);
        return sidebar;
    }

    private Control BuildContent()
    {
        var wrap = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(28),
            AutoScroll = true
        };

        var title = Theme.CreatePageTitle("Bảng điều khiển");
        var subtitle = Theme.CreateSubtitle($"Xin chào {Session.Username} - Vai trò: {Session.Role}. Chọn phân hệ cần thao tác ở menu bên trái.");
        subtitle.Margin = new Padding(0, 0, 0, 20);

        var statsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 170,
            WrapContents = true,
            Margin = new Padding(0, 0, 0, 18)
        };
        statsPanel.Controls.Add(Theme.CreateStatCard("Trạng thái hệ thống", "Ổn định", "Sẵn sàng quản lý bán hàng"));
        statsPanel.Controls.Add(Theme.CreateStatCard("Vai trò hiện tại", Session.Role, "Phân quyền theo tài khoản"));
        statsPanel.Controls.Add(Theme.CreateStatCard("Tài khoản đăng nhập", Session.Username, "Người đang thao tác"));

        var quickCard = Theme.CreateCard(24);
        quickCard.Dock = DockStyle.Top;
        quickCard.Height = 320;

        var quickLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1
        };
        quickLayout.Controls.Add(Theme.CreatePageTitle("Truy cập nhanh"), 0, 0);
        quickLayout.Controls.Add(Theme.CreateSubtitle("Mở nhanh các chức năng chính bằng nút bên dưới."), 0, 1);

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 12, 0, 0)
        };

        var btnProducts = Theme.CreatePrimaryButton("Sản phẩm", 160);
        btnProducts.Click += (_, _) => new ProductForm().ShowDialog();

        var btnCustomers = Theme.CreateSecondaryButton("Khách hàng", 160);
        btnCustomers.Click += (_, _) => new CustomerForm().ShowDialog();

        var btnOrders = Theme.CreateSuccessButton("Đơn hàng", 160);
        btnOrders.Click += (_, _) => new OrderForm().ShowDialog();

        var btnReports = Theme.CreatePrimaryButton("Thống kê", 160);
        btnReports.Click += (_, _) => new ReportForm().ShowDialog();
        if (Session.Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
            btnReports.Enabled = false;

        buttons.Controls.Add(btnProducts);
        buttons.Controls.Add(btnCustomers);
        buttons.Controls.Add(btnOrders);
        buttons.Controls.Add(btnReports);

        quickLayout.Controls.Add(buttons, 0, 2);
        quickCard.Controls.Add(quickLayout);

        var noteCard = Theme.CreateCard(24);
        noteCard.Dock = DockStyle.Top;
        noteCard.Height = 200;
        noteCard.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "Gợi ý trình bày bài tập lớn:\n\n" +
                   "• Form đăng nhập và phân quyền Admin / Staff\n" +
                   "• Form quản lý sản phẩm, khách hàng, đơn hàng\n" +
                   "• Tồn kho cập nhật sau khi bán\n" +
                   "• Báo cáo doanh thu và sản phẩm sắp hết\n\n" +
                   "Giao diện này đã được làm lại theo hướng sáng, gọn, chuyên nghiệp hơn để dễ demo.",
            Font = new Font("Segoe UI", 10.5F),
            ForeColor = Theme.TextPrimary
        });

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };
        flow.Controls.Add(title);
        flow.Controls.Add(subtitle);
        flow.Controls.Add(statsPanel);
        flow.Controls.Add(quickCard);
        flow.Controls.Add(noteCard);

        wrap.Controls.Add(flow);
        return wrap;
    }
}
