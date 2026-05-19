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
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 240));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        root.Controls.Add(BuildSidebar(), 0, 0);
        root.Controls.Add(BuildContent(), 1, 0);
        Controls.Add(root);
    }

    private Panel BuildSidebar()
    {
        var sidebar = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Theme.Sidebar
        };

        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = Theme.Sidebar,
            Padding = new Padding(22, 20, 20, 10)
        };
        var lblLogo = new Label
        {
            Dock = DockStyle.Fill,
            Text = "🌸 MỸ PHẨM\nMANAGER",
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = false
        };
        header.Controls.Add(lblLogo);

        var divider = new Panel
        {
            Dock = DockStyle.Top,
            Height = 1,
            BackColor = Color.FromArgb(80, 255, 255, 255)
        };

        var btnProducts = Theme.CreateSidebarButton("💄  Quản lý sản phẩm");
        var btnCustomers = Theme.CreateSidebarButton("👤  Quản lý khách hàng");
        var btnOrders = Theme.CreateSidebarButton("🛒  Quản lý đơn hàng");
        var btnReports = Theme.CreateSidebarButton("📊  Báo cáo thống kê");
        var btnLogout = Theme.CreateSidebarButton("⬅  Đăng xuất");

        btnProducts.Click += (_, _) => new ProductForm().ShowDialog();
        btnCustomers.Click += (_, _) => new CustomerForm().ShowDialog();
        btnOrders.Click += (_, _) => new OrderForm().ShowDialog();
        btnReports.Click += (_, _) => new ReportForm().ShowDialog();
        btnLogout.Click += (_, _) => Close();

        if (Session.Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
            btnReports.Enabled = false;

        btnLogout.Dock = DockStyle.Bottom;
        var footerDivider = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 1,
            BackColor = Color.FromArgb(60, 255, 255, 255)
        };

        sidebar.Controls.Add(btnLogout);
        sidebar.Controls.Add(footerDivider);
        sidebar.Controls.Add(btnReports);
        sidebar.Controls.Add(btnOrders);
        sidebar.Controls.Add(btnCustomers);
        sidebar.Controls.Add(btnProducts);
        sidebar.Controls.Add(divider);
        sidebar.Controls.Add(header);
        return sidebar;
    }

    private Control BuildContent()
    {
        var wrap = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Theme.AppBackground,
            Padding = new Padding(32),
            AutoScroll = true
        };

        var title = Theme.CreatePageTitle("Bảng điều khiển");
        var subtitle = Theme.CreateSubtitle(
            $"Xin chào, {Session.Username} · Vai trò: {Session.Role}  —  Chọn phân hệ ở menu bên trái.");
        subtitle.Margin = new Padding(0, 2, 0, 24);

        var statsPanel = new FlowLayoutPanel
        {
            AutoSize = true,
            WrapContents = true,
            Margin = new Padding(0, 0, 0, 20)
        };
        statsPanel.Controls.Add(Theme.CreateStatCard("Trạng thái hệ thống", "Ổn định", "Sẵn sàng quản lý bán hàng"));
        statsPanel.Controls.Add(Theme.CreateStatCard("Vai trò hiện tại", Session.Role, "Phân quyền theo tài khoản"));
        statsPanel.Controls.Add(Theme.CreateStatCard("Tài khoản đăng nhập", Session.Username, "Người đang thao tác"));

        var quickCard = Theme.CreateCard(24);
        quickCard.Width = 900;
        quickCard.Height = 200;
        quickCard.Margin = new Padding(0, 0, 0, 20);

        var quickLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1
        };
        quickLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        quickLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        quickLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        quickLayout.Controls.Add(Theme.CreatePageTitle("Truy cập nhanh"), 0, 0);
        quickLayout.Controls.Add(Theme.CreateSubtitle("Mở nhanh các chức năng bằng nút bên dưới."), 0, 1);

        var buttons = new FlowLayoutPanel
        {
            AutoSize = true,
            Padding = new Padding(0, 8, 0, 0),
            BackColor = Color.Transparent
        };

        var btnProducts = Theme.CreatePrimaryButton("💄 Sản phẩm", 160);
        var btnCustomers = Theme.CreateSecondaryButton("👤 Khách hàng", 160);
        var btnOrders = Theme.CreateSuccessButton("🛒 Đơn hàng", 160);
        var btnReports = Theme.CreatePrimaryButton("📊 Thống kê", 160);

        btnProducts.Click += (_, _) => new ProductForm().ShowDialog();
        btnCustomers.Click += (_, _) => new CustomerForm().ShowDialog();
        btnOrders.Click += (_, _) => new OrderForm().ShowDialog();
        btnReports.Click += (_, _) => new ReportForm().ShowDialog();

        if (Session.Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
            btnReports.Enabled = false;

        buttons.Controls.AddRange([btnProducts, btnCustomers, btnOrders, btnReports]);
        quickLayout.Controls.Add(buttons, 0, 2);
        quickCard.Controls.Add(quickLayout);

        var noteCard = Theme.CreateCard(24);
        noteCard.Width = 900;
        noteCard.Height = 195;

        var noteTitle = Theme.CreateFieldLabel("✨ Giới thiệu cửa hàng mỹ phẩm");
        noteTitle.Dock = DockStyle.Top;
        noteTitle.Height = 32;

        var noteContent = new Label
        {
            Dock = DockStyle.Fill,
            Text = "• Chuyên cung cấp mỹ phẩm chính hãng, chăm sóc da và trang điểm\n" +
                   "• Đa dạng sản phẩm: son môi, kem dưỡng, serum, nước hoa\n" +
                   "• Cam kết chất lượng, an toàn và phù hợp nhiều loại da\n" +
                   "• Cập nhật xu hướng làm đẹp mới nhất, sản phẩm bán chạy\n" +
                   "• Nhân viên thao tác hỗ trợ nhanh và chính xác hơn.",
            Font = new Font("Segoe UI", 10.5F),
            ForeColor = Theme.TextPrimary
        };
        noteCard.Controls.Add(noteContent);
        noteCard.Controls.Add(noteTitle);

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Color.Transparent
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
