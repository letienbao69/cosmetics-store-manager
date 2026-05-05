using CosmeticsStoreManager.Models;
using CosmeticsStoreManager.Repositories;
using CosmeticsStoreManager.UI;

namespace CosmeticsStoreManager.Forms;

public class ProductForm : Form
{
    private readonly ProductRepository _repo = new();
    private readonly DataGridView dgv = new() { Dock = DockStyle.Fill };
    private readonly TextBox txtSearch = Theme.CreateTextBox("Tìm theo mã hoặc tên sản phẩm");
    private readonly TextBox txtCode = Theme.CreateTextBox();
    private readonly TextBox txtName = Theme.CreateTextBox();
    private readonly TextBox txtCategory = Theme.CreateTextBox();
    private readonly TextBox txtBrand = Theme.CreateTextBox();
    private readonly NumericUpDown nudImportPrice = Theme.CreateNumeric();
    private readonly NumericUpDown nudSalePrice = Theme.CreateNumeric();
    private readonly NumericUpDown nudStock = Theme.CreateNumeric();
    private readonly DateTimePicker dtpExpiry = Theme.CreateDatePicker();
    private readonly TextBox txtDescription = Theme.CreateTextBox();

    private int _selectedId;

    public ProductForm()
    {
        Theme.ApplyForm(this, "Quản lý sản phẩm");
        Size = new Size(1380, 820);

        nudImportPrice.Maximum = 1_000_000_000;
        nudSalePrice.Maximum = 1_000_000_000;
        nudStock.Maximum = 100_000;
        txtDescription.Multiline = true;
        txtDescription.Height = 80;

        Theme.StyleGrid(dgv);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(20)
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 410));
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
            RowCount = 16,
            AutoScroll = true
        };
        for (int i = 0; i < 16; i++)
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        layout.Controls.Add(Theme.CreatePageTitle("💄  Thông tin sản phẩm"), 0, 0);
        layout.Controls.Add(Theme.CreateSubtitle("Nhập đầy đủ thông tin mỹ phẩm để thêm hoặc cập nhật."), 0, 1);

        layout.Controls.Add(Theme.CreateFieldLabel("Mã sản phẩm"), 0, 2);
        txtCode.Width = 330; layout.Controls.Add(txtCode, 0, 3);

        layout.Controls.Add(Theme.CreateFieldLabel("Tên sản phẩm"), 0, 4);
        txtName.Width = 330; layout.Controls.Add(txtName, 0, 5);

        layout.Controls.Add(Theme.CreateFieldLabel("Loại sản phẩm"), 0, 6);
        txtCategory.Width = 330; layout.Controls.Add(txtCategory, 0, 7);

        layout.Controls.Add(Theme.CreateFieldLabel("Thương hiệu"), 0, 8);
        txtBrand.Width = 330; layout.Controls.Add(txtBrand, 0, 9);

        // Giá nhập / Giá bán
        var priceRow = new FlowLayoutPanel
        {
            AutoSize = true,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 4)
        };
        priceRow.Controls.Add(CreateMiniField("Giá nhập", nudImportPrice));
        priceRow.Controls.Add(CreateMiniField("Giá bán", nudSalePrice));
        layout.Controls.Add(priceRow, 0, 10);

        // Tồn kho / Hạn dùng
        var stockRow = new FlowLayoutPanel
        {
            AutoSize = true,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 4)
        };
        stockRow.Controls.Add(CreateMiniField("Tồn kho", nudStock));
        stockRow.Controls.Add(CreateMiniField("Hạn dùng", dtpExpiry));
        layout.Controls.Add(stockRow, 0, 11);

        layout.Controls.Add(Theme.CreateFieldLabel("Mô tả"), 0, 12);
        txtDescription.Width = 330; layout.Controls.Add(txtDescription, 0, 13);

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Margin = new Padding(0, 14, 0, 0)
        };
        var btnAdd = Theme.CreatePrimaryButton("Thêm");
        var btnUpdate = Theme.CreateSuccessButton("Cập nhật");
        var btnDelete = Theme.CreateDangerButton("Xóa");
        var btnClear = Theme.CreateSecondaryButton("Làm mới");

        btnAdd.Click += (_, _) => AddProduct();
        btnUpdate.Click += (_, _) => UpdateProduct();
        btnDelete.Click += (_, _) => DeleteProduct();
        btnClear.Click += (_, _) => ClearInput();

        actions.Controls.AddRange([btnAdd, btnUpdate, btnDelete, btnClear]);
        layout.Controls.Add(actions, 0, 14);

        card.Controls.Add(layout);
        return card;
    }

    private Control BuildRightPanel()
    {
        var wrap = new Panel { Dock = DockStyle.Fill };

        var headerCard = Theme.CreateCard(18);
        headerCard.Dock = DockStyle.Top;
        headerCard.Height = 140;

        var headerLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };
        for (int i = 0; i < 3; i++)
            headerLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        headerLayout.Controls.Add(Theme.CreatePageTitle("📋  Danh sách sản phẩm"), 0, 0);
        headerLayout.Controls.Add(Theme.CreateSubtitle(
            "Tìm kiếm nhanh, chọn dòng để chỉnh sửa hoặc xóa."), 0, 1);

        var searchRow = new FlowLayoutPanel
        {
            AutoSize = true,
            Margin = new Padding(0, 8, 0, 0)
        };
        txtSearch.Width = 340;
        var btnSearch = Theme.CreatePrimaryButton("🔍 Tìm", 110);
        var btnReload = Theme.CreateSecondaryButton("↺ Tải lại", 110);

        btnSearch.Click += (_, _) => LoadData(txtSearch.Text.Trim());
        btnReload.Click += (_, _) => { txtSearch.Clear(); LoadData(); };

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

    private Panel CreateMiniField(string label, Control control)
    {
        control.Width = 155;
        var panel = new Panel
        {
            Width = 168,
            Height = 90,
            Margin = new Padding(0, 0, 14, 8)
        };
        var lbl = Theme.CreateFieldLabel(label);
        lbl.Location = new Point(0, 0);
        control.Location = new Point(0, 34);
        panel.Controls.Add(lbl);
        panel.Controls.Add(control);
        return panel;
    }

    private void LoadData(string? keyword = null)
        => dgv.DataSource = _repo.GetAll(keyword);

    private Product BuildProduct() => new()
    {
        ProductId = _selectedId,
        ProductCode = txtCode.Text.Trim(),
        ProductName = txtName.Text.Trim(),
        Category = txtCategory.Text.Trim(),
        Brand = txtBrand.Text.Trim(),
        ImportPrice = nudImportPrice.Value,
        SalePrice = nudSalePrice.Value,
        QuantityInStock = (int)nudStock.Value,
        ExpiryDate = dtpExpiry.Checked ? dtpExpiry.Value.Date : null,
        Description = txtDescription.Text.Trim()
    };

    private void AddProduct()
    {
        try
        {
            _repo.Add(BuildProduct());
            MessageBox.Show("Thêm sản phẩm thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData(); ClearInput();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi thêm sản phẩm: " + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateProduct()
    {
        if (_selectedId == 0) { MessageBox.Show("Hãy chọn sản phẩm cần sửa."); return; }
        try
        {
            _repo.Update(BuildProduct());
            MessageBox.Show("Cập nhật sản phẩm thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData(); ClearInput();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi cập nhật sản phẩm: " + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DeleteProduct()
    {
        if (_selectedId == 0) { MessageBox.Show("Hãy chọn sản phẩm cần xóa."); return; }
        if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
        try
        {
            _repo.Delete(_selectedId);
            MessageBox.Show("Xóa sản phẩm thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData(); ClearInput();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi xóa sản phẩm: " + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Dgv_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgv.CurrentRow?.DataBoundItem is not Product p) return;
        _selectedId = p.ProductId;
        txtCode.Text = p.ProductCode;
        txtName.Text = p.ProductName;
        txtCategory.Text = p.Category;
        txtBrand.Text = p.Brand;
        nudImportPrice.Value = p.ImportPrice;
        nudSalePrice.Value = p.SalePrice;
        nudStock.Value = p.QuantityInStock;
        dtpExpiry.Checked = p.ExpiryDate.HasValue;
        if (p.ExpiryDate.HasValue) dtpExpiry.Value = p.ExpiryDate.Value;
        txtDescription.Text = p.Description;
    }

    private void ClearInput()
    {
        _selectedId = 0;
        txtCode.Clear(); txtName.Clear(); txtCategory.Clear(); txtBrand.Clear();
        nudImportPrice.Value = 0; nudSalePrice.Value = 0; nudStock.Value = 0;
        dtpExpiry.Checked = false; txtDescription.Clear();
    }
}