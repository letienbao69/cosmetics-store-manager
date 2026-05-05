using CosmeticsStoreManager.Repositories;
using CosmeticsStoreManager.UI;

namespace CosmeticsStoreManager.Forms;

public class LoginForm : Form
{
    private readonly TextBox txtUsername = Theme.CreateTextBox("Nhập tên đăng nhập");
    private readonly TextBox txtPassword = Theme.CreateTextBox("Nhập mật khẩu");
    private readonly Button btnLogin = Theme.CreatePrimaryButton("Đăng nhập", 280);

    public LoginForm()
    {
        Theme.ApplyForm(this, "Đăng nhập hệ thống");
        Size = new Size(1100, 640);
        MinimumSize = new Size(1000, 600);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        txtPassword.UseSystemPasswordChar = true;
        AcceptButton = btnLogin;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Theme.AppBackground
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        root.Controls.Add(BuildBrandingPanel(), 0, 0);
        root.Controls.Add(BuildLoginPanel(), 1, 0);
        Controls.Add(root);

        btnLogin.Click += BtnLogin_Click;
    }

    private Panel BuildBrandingPanel()
    {
        var branding = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Theme.Primary
        };

        var brandWrap = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(48, 72, 40, 40),
            BackColor = Color.Transparent
        };
        brandWrap.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        brandWrap.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        brandWrap.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var lblTitle = new Label
        {
            Text = "COSMETICS\nSTORE MANAGER",
            AutoSize = true,
            Font = new Font("Segoe UI", 26F, FontStyle.Bold),
            ForeColor = Color.White,
            Margin = new Padding(0, 0, 0, 20)
        };

        var lblDesc = new Label
        {
            Text = "Giao diện mới, trực quan hơn cho bài tập lớn nhóm.\nQuản lý sản phẩm, khách hàng, đơn hàng,\nkho và báo cáo trong một ứng dụng Windows.",
            AutoSize = true,
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(255, 220, 234),
            Margin = new Padding(0, 0, 0, 32)
        };

        // Info card — dùng PrimaryDark thay vì alpha để tránh lỗi màu vàng
        var infoCard = new Panel
        {
            Width = 320,
            Height = 130,
            BackColor = Theme.PrimaryDark,
            Padding = new Padding(20),
            Margin = new Padding(0)
        };
        var lblInfo = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Tài khoản mẫu\n• Admin: admin / 123456\n• Staff: staff / 123456",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            AutoSize = false
        };
        infoCard.Controls.Add(lblInfo);

        brandWrap.Controls.Add(lblTitle, 0, 0);
        brandWrap.Controls.Add(lblDesc, 0, 1);
        brandWrap.Controls.Add(infoCard, 0, 2);
        branding.Controls.Add(brandWrap);
        return branding;
    }

    private Panel BuildLoginPanel()
    {
        var loginHost = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Theme.Surface,
            Padding = new Padding(60, 0, 60, 0)
        };

        // Dùng TableLayoutPanel để căn giữa dọc
        var centerWrap = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Color.Transparent
        };
        centerWrap.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        centerWrap.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        centerWrap.RowStyles.Add(new RowStyle(SizeType.Percent, 80));

        var loginForm = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Color.Transparent
        };

        var lblWelcome = new Label
        {
            Text = "Chào mừng quay lại",
            AutoSize = true,
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = Theme.TextPrimary,
            Margin = new Padding(0, 0, 0, 6)
        };

        var lblSub = new Label
        {
            Text = "Đăng nhập để sử dụng hệ thống quản lý cửa hàng mỹ phẩm.",
            AutoSize = true,
            Font = new Font("Segoe UI", 10.5F),
            ForeColor = Theme.TextMuted,
            Margin = new Padding(0, 0, 0, 28)
        };

        var lblUser = Theme.CreateFieldLabel("Tên đăng nhập");
        lblUser.Margin = new Padding(0, 0, 0, 4);
        txtUsername.Width = 360;
        txtUsername.Height = 36;
        txtUsername.Margin = new Padding(0, 0, 0, 16);

        var lblPass = Theme.CreateFieldLabel("Mật khẩu");
        lblPass.Margin = new Padding(0, 0, 0, 4);
        txtPassword.Width = 360;
        txtPassword.Height = 36;
        txtPassword.Margin = new Padding(0, 0, 0, 24);

        btnLogin.Width = 360;
        btnLogin.Height = 44;
        btnLogin.Margin = new Padding(0, 0, 0, 14);
        btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

        var lblTip = new Label
        {
            Text = "Mẹo: dùng tài khoản Staff để kiểm tra phân quyền cơ bản.",
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Theme.TextMuted
        };

        loginForm.Controls.Add(lblWelcome);
        loginForm.Controls.Add(lblSub);
        loginForm.Controls.Add(lblUser);
        loginForm.Controls.Add(txtUsername);
        loginForm.Controls.Add(lblPass);
        loginForm.Controls.Add(txtPassword);
        loginForm.Controls.Add(btnLogin);
        loginForm.Controls.Add(lblTip);

        centerWrap.Controls.Add(new Panel { BackColor = Color.Transparent }, 0, 0);
        centerWrap.Controls.Add(loginForm, 0, 1);
        centerWrap.Controls.Add(new Panel { BackColor = Color.Transparent }, 0, 2);

        loginHost.Controls.Add(centerWrap);
        return loginHost;
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        try
        {
            var repo = new UserRepository();
            var user = repo.Login(txtUsername.Text.Trim(), txtPassword.Text.Trim());

            if (user is null)
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Session.UserId = user.UserId;
            Session.Username = user.Username;
            Session.Role = user.Role;

            Hide();
            new MainForm().ShowDialog();
            Show();
            txtPassword.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi đăng nhập: " + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}