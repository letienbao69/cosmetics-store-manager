using CosmeticsStoreManager.Repositories;
using CosmeticsStoreManager.UI;

namespace CosmeticsStoreManager.Forms;

public class LoginForm : Form
{
    private readonly TextBox txtUsername = Theme.CreateTextBox("Nhập tên đăng nhập");
    private readonly TextBox txtPassword = Theme.CreateTextBox("Nhập mật khẩu");
    private readonly Button btnLogin = Theme.CreatePrimaryButton("Đăng nhập", 240);

    public LoginForm()
    {
        Theme.ApplyForm(this, "Đăng nhập hệ thống");
        Size = new Size(980, 580);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        txtPassword.UseSystemPasswordChar = true;
        AcceptButton = btnLogin;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            BackColor = Theme.AppBackground
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));

        var branding = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Theme.Primary
        };

        var brandWrap = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(48, 80, 30, 30),
            BackColor = Color.Transparent
        };

        brandWrap.Controls.Add(new Label
        {
            Text = "COSMETICS\nSTORE MANAGER",
            AutoSize = true,
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = Color.White,
            Margin = new Padding(0, 0, 0, 16)
        });
        brandWrap.Controls.Add(new Label
        {
            Text = "Giao diện mới, trực quan hơn cho bài tập lớn nhóm.\nQuản lý sản phẩm, khách hàng, đơn hàng,\nkho và báo cáo trong một ứng dụng Windows.",
            AutoSize = true,
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.White,
            Margin = new Padding(0, 0, 0, 28)
        });

        var infoCard = new Panel
        {
            Width = 320,
            Height = 140,
            BackColor = Color.FromArgb(255, 255, 255, 40),
            Padding = new Padding(18),
            Margin = new Padding(0)
        };
        infoCard.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = "Tài khoản mẫu\n• Admin: admin / 123456\n• Staff: staff / 123456",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            AutoSize = false
        });
        brandWrap.Controls.Add(infoCard);
        branding.Controls.Add(brandWrap);

        var loginHost = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(50)
        };

        var loginCard = Theme.CreateCard(28);
        loginCard.Dock = DockStyle.Fill;

        var loginLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 9,
            ColumnCount = 1
        };
        for (int i = 0; i < 9; i++)
            loginLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        loginLayout.Controls.Add(new Label
        {
            Text = "Chào mừng quay lại",
            AutoSize = true,
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = Theme.TextPrimary
        }, 0, 0);

        loginLayout.Controls.Add(new Label
        {
            Text = "Đăng nhập để sử dụng hệ thống quản lý cửa hàng mỹ phẩm.",
            AutoSize = true,
            Font = new Font("Segoe UI", 10.5F),
            ForeColor = Theme.TextMuted,
            Margin = new Padding(0, 8, 0, 22)
        }, 0, 1);

        loginLayout.Controls.Add(Theme.CreateFieldLabel("Tên đăng nhập"), 0, 2);
        txtUsername.Width = 360;
        txtUsername.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        loginLayout.Controls.Add(txtUsername, 0, 3);

        loginLayout.Controls.Add(Theme.CreateFieldLabel("Mật khẩu"), 0, 4);
        txtPassword.Width = 360;
        txtPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        loginLayout.Controls.Add(txtPassword, 0, 5);

        btnLogin.Margin = new Padding(0, 18, 0, 10);
        btnLogin.Anchor = AnchorStyles.Left;
        loginLayout.Controls.Add(btnLogin, 0, 6);

        loginLayout.Controls.Add(new Label
        {
            Text = "Mẹo: dùng tài khoản Staff để kiểm tra phân quyền cơ bản.",
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Theme.TextMuted,
            Margin = new Padding(0, 5, 0, 0)
        }, 0, 7);

        loginCard.Controls.Add(loginLayout);
        loginHost.Controls.Add(loginCard);

        root.Controls.Add(branding, 0, 0);
        root.Controls.Add(loginHost, 1, 0);
        Controls.Add(root);

        btnLogin.Click += BtnLogin_Click;
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        try
        {
            var repo = new UserRepository();
            var user = repo.Login(txtUsername.Text.Trim(), txtPassword.Text.Trim());

            if (user is null)
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            MessageBox.Show("Lỗi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
