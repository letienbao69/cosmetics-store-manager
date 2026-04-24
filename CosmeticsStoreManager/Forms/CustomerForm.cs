using CosmeticsStoreManager.Models;
using CosmeticsStoreManager.Repositories;
using CosmeticsStoreManager.UI;

namespace CosmeticsStoreManager.Forms;

public class CustomerForm : Form
{
    private readonly CustomerRepository _repo = new();
    private readonly DataGridView dgv = new() { Dock = DockStyle.Fill };
    private readonly TextBox txtSearch = Theme.CreateTextBox("Tìm theo tên hoặc số điện thoại");
    private readonly TextBox txtFullName = Theme.CreateTextBox();
    private readonly TextBox txtPhone = Theme.CreateTextBox();
    private readonly TextBox txtAddress = Theme.CreateTextBox();
    private readonly TextBox txtEmail = Theme.CreateTextBox();

    private int _selectedId;

    public CustomerForm()
    {
        Theme.ApplyForm(this, "Quản lý khách hàng");
        Size = new Size(1280, 760);
        Theme.StyleGrid(dgv);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(20)
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 380));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        root.Controls.Add(BuildLeftPanel(), 0, 0);
        root.Controls.Add(BuildRightPanel(), 1, 0);

        Controls.Add(root);
        LoadData();
        dgv.SelectionChanged += Dgv_SelectionChanged;
    }

    private Control BuildLeftPanel()
    {
        var card = Theme.CreateCard(22);
        card.Dock = DockStyle.Fill;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 12,
            AutoScroll = true
        };

        layout.Controls.Add(Theme.CreatePageTitle("Thông tin khách hàng"), 0, 0);
        layout.Controls.Add(Theme.CreateSubtitle("Quản lý hồ sơ khách hàng để tiện tra cứu và lập đơn hàng."), 0, 1);

        layout.Controls.Add(Theme.CreateFieldLabel("Họ và tên"), 0, 2); layout.Controls.Add(txtFullName, 0, 3);
        layout.Controls.Add(Theme.CreateFieldLabel("Số điện thoại"), 0, 4); layout.Controls.Add(txtPhone, 0, 5);
        layout.Controls.Add(Theme.CreateFieldLabel("Địa chỉ"), 0, 6); layout.Controls.Add(txtAddress, 0, 7);
        layout.Controls.Add(Theme.CreateFieldLabel("Email"), 0, 8); layout.Controls.Add(txtEmail, 0, 9);

        var actions = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0, 12, 0, 0) };
        var btnAdd = Theme.CreatePrimaryButton("Thêm");
        var btnUpdate = Theme.CreateSuccessButton("Cập nhật");
        var btnDelete = Theme.CreateDangerButton("Xóa");
        var btnClear = Theme.CreateSecondaryButton("Làm mới");

        btnAdd.Click += (_, _) => AddCustomer();
        btnUpdate.Click += (_, _) => UpdateCustomer();
        btnDelete.Click += (_, _) => DeleteCustomer();
        btnClear.Click += (_, _) => ClearInput();

        actions.Controls.AddRange([btnAdd, btnUpdate, btnDelete, btnClear]);
        layout.Controls.Add(actions, 0, 10);

        card.Controls.Add(layout);
        return card;
    }

    private Control BuildRightPanel()
    {
        var wrap = new Panel { Dock = DockStyle.Fill };

        var headerCard = Theme.CreateCard(18);
        headerCard.Dock = DockStyle.Top;
        headerCard.Height = 135;

        var headerLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };

        headerLayout.Controls.Add(Theme.CreatePageTitle("Danh sách khách hàng"), 0, 0);
        headerLayout.Controls.Add(Theme.CreateSubtitle("Tìm kiếm khách hàng nhanh để hỗ trợ bán hàng thuận tiện hơn."), 0, 1);

        var searchRow = new FlowLayoutPanel { Dock = DockStyle.Fill };
        txtSearch.Width = 330;
        var btnSearch = Theme.CreatePrimaryButton("Tìm", 110);
        var btnReload = Theme.CreateSecondaryButton("Tải lại", 110);

        btnSearch.Click += (_, _) => LoadData(txtSearch.Text.Trim());
        btnReload.Click += (_, _) => LoadData();

        searchRow.Controls.Add(txtSearch);
        searchRow.Controls.Add(btnSearch);
        searchRow.Controls.Add(btnReload);

        headerLayout.Controls.Add(searchRow, 0, 2);
        headerCard.Controls.Add(headerLayout);

        var gridCard = Theme.CreateCard(10);
        gridCard.Dock = DockStyle.Fill;
        gridCard.Controls.Add(dgv);

        wrap.Controls.Add(gridCard);
        wrap.Controls.Add(headerCard);
        return wrap;
    }

    private void LoadData(string? keyword = null)
    {
        dgv.DataSource = _repo.GetAll(keyword);
    }

    private Customer BuildCustomer()
    {
        return new Customer
        {
            CustomerId = _selectedId,
            FullName = txtFullName.Text.Trim(),
            Phone = txtPhone.Text.Trim(),
            Address = txtAddress.Text.Trim(),
            Email = txtEmail.Text.Trim()
        };
    }

    private void AddCustomer()
    {
        try
        {
            _repo.Add(BuildCustomer());
            MessageBox.Show("Thêm khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            ClearInput();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateCustomer()
    {
        if (_selectedId == 0)
        {
            MessageBox.Show("Hãy chọn khách hàng cần sửa.");
            return;
        }

        try
        {
            _repo.Update(BuildCustomer());
            MessageBox.Show("Cập nhật khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            ClearInput();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi cập nhật khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DeleteCustomer()
    {
        if (_selectedId == 0)
        {
            MessageBox.Show("Hãy chọn khách hàng cần xóa.");
            return;
        }

        if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            return;

        try
        {
            _repo.Delete(_selectedId);
            MessageBox.Show("Xóa khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            ClearInput();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi xóa khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Dgv_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgv.CurrentRow?.DataBoundItem is not Customer c) return;

        _selectedId = c.CustomerId;
        txtFullName.Text = c.FullName;
        txtPhone.Text = c.Phone;
        txtAddress.Text = c.Address;
        txtEmail.Text = c.Email;
    }

    private void ClearInput()
    {
        _selectedId = 0;
        txtFullName.Clear();
        txtPhone.Clear();
        txtAddress.Clear();
        txtEmail.Clear();
    }
}
