using CosmeticsStoreManager.Models;
using CosmeticsStoreManager.Repositories;
using CosmeticsStoreManager.UI;

namespace CosmeticsStoreManager.Forms;

public class OrderForm : Form
{
    private readonly ProductRepository _productRepo = new();
    private readonly CustomerRepository _customerRepo = new();
    private readonly OrderRepository _orderRepo = new();

    private readonly ComboBox cboCustomer = Theme.CreateComboBox();
    private readonly ComboBox cboProduct = Theme.CreateComboBox();
    private readonly NumericUpDown nudQuantity = Theme.CreateNumeric();
    private readonly DataGridView dgvItems = new() { Dock = DockStyle.Fill };
    private readonly Label lblTotal = new()
    {
        Text = "Tổng tiền: 0 VNĐ",
        AutoSize = true,
        Font = new Font("Segoe UI", 18F, FontStyle.Bold),
        ForeColor = Theme.TextPrimary
    };

    private readonly List<OrderItem> _items = new();

    public OrderForm()
    {
        Theme.ApplyForm(this, "Quản lý đơn hàng");
        Size = new Size(1320, 780);

        nudQuantity.Minimum = 1;
        nudQuantity.Maximum = 1000;
        nudQuantity.Value = 1;

        Theme.StyleGrid(dgvItems);

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

        LoadLookups();
        BindItems();
    }

    private Control BuildLeftPanel()
    {
        var card = Theme.CreateCard(22);
        card.Dock = DockStyle.Fill;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 14,
            AutoScroll = true
        };

        layout.Controls.Add(Theme.CreatePageTitle("Tạo đơn hàng"), 0, 0);
        layout.Controls.Add(Theme.CreateSubtitle("Chọn khách hàng, thêm sản phẩm và lưu đơn hàng."), 0, 1);

        layout.Controls.Add(Theme.CreateFieldLabel("Khách hàng"), 0, 2);
        cboCustomer.Width = 320;
        layout.Controls.Add(cboCustomer, 0, 3);

        layout.Controls.Add(Theme.CreateFieldLabel("Sản phẩm"), 0, 4);
        cboProduct.Width = 320;
        layout.Controls.Add(cboProduct, 0, 5);

        layout.Controls.Add(Theme.CreateFieldLabel("Số lượng"), 0, 6);
        nudQuantity.Width = 320;
        layout.Controls.Add(nudQuantity, 0, 7);

        var actions = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0, 14, 0, 10) };
        var btnAddItem = Theme.CreatePrimaryButton("Thêm vào đơn", 150);
        var btnRemoveItem = Theme.CreateDangerButton("Xóa dòng", 120);
        var btnSaveOrder = Theme.CreateSuccessButton("Lưu đơn hàng", 150);

        btnAddItem.Click += (_, _) => AddItem();
        btnRemoveItem.Click += (_, _) => RemoveSelectedItem();
        btnSaveOrder.Click += (_, _) => SaveOrder();

        actions.Controls.AddRange([btnAddItem, btnRemoveItem, btnSaveOrder]);
        layout.Controls.Add(actions, 0, 8);

        var totalCard = Theme.CreateCard(20);
        totalCard.Height = 120;
        totalCard.Margin = new Padding(0, 10, 0, 0);
        totalCard.Controls.Add(lblTotal);
        lblTotal.Location = new Point(18, 36);

        layout.Controls.Add(totalCard, 0, 9);
        card.Controls.Add(layout);
        return card;
    }

    private Control BuildRightPanel()
    {
        var wrap = new Panel { Dock = DockStyle.Fill };

        var headerCard = Theme.CreateCard(18);
        headerCard.Dock = DockStyle.Top;
        headerCard.Height = 110;

        var headerLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        headerLayout.Controls.Add(Theme.CreatePageTitle("Chi tiết đơn hàng"), 0, 0);
        headerLayout.Controls.Add(Theme.CreateSubtitle("Các sản phẩm đã thêm sẽ hiển thị ở đây. Chọn dòng để xóa nếu cần."), 0, 1);
        headerCard.Controls.Add(headerLayout);

        var gridCard = Theme.CreateCard(10);
        gridCard.Dock = DockStyle.Fill;
        gridCard.Controls.Add(dgvItems);

        wrap.Controls.Add(gridCard);
        wrap.Controls.Add(headerCard);
        return wrap;
    }

    private void LoadLookups()
    {
        var customers = _customerRepo.GetAll();
        cboCustomer.DataSource = customers;
        cboCustomer.DisplayMember = "FullName";
        cboCustomer.ValueMember = "CustomerId";

        var products = _productRepo.GetAll();
        cboProduct.DataSource = products;
        cboProduct.DisplayMember = "ProductName";
        cboProduct.ValueMember = "ProductId";
    }

    private void BindItems()
    {
        dgvItems.DataSource = null;
        dgvItems.DataSource = _items.Select(x => new
        {
            x.ProductId,
            x.ProductName,
            x.UnitPrice,
            x.Quantity,
            x.LineTotal
        }).ToList();

        lblTotal.Text = $"Tổng tiền: {_items.Sum(x => x.LineTotal):N0} VNĐ";
    }

    private void AddItem()
    {
        if (cboProduct.SelectedItem is not Product product)
        {
            MessageBox.Show("Vui lòng chọn sản phẩm.");
            return;
        }

        int requested = (int)nudQuantity.Value;
        if (requested > product.QuantityInStock)
        {
            MessageBox.Show("Số lượng vượt quá tồn kho.");
            return;
        }

        var existing = _items.FirstOrDefault(x => x.ProductId == product.ProductId);
        if (existing != null)
        {
            if (existing.Quantity + requested > product.QuantityInStock)
            {
                MessageBox.Show("Tổng số lượng trong đơn vượt quá tồn kho.");
                return;
            }
            existing.Quantity += requested;
        }
        else
        {
            _items.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.SalePrice,
                Quantity = requested
            });
        }

        BindItems();
    }

    private void RemoveSelectedItem()
    {
        if (dgvItems.CurrentRow == null) return;
        int productId = Convert.ToInt32(dgvItems.CurrentRow.Cells["ProductId"].Value);

        var item = _items.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            BindItems();
        }
    }

    private void SaveOrder()
    {
        if (cboCustomer.SelectedValue == null)
        {
            MessageBox.Show("Vui lòng chọn khách hàng.");
            return;
        }

        if (_items.Count == 0)
        {
            MessageBox.Show("Đơn hàng chưa có sản phẩm.");
            return;
        }

        try
        {
            int customerId = Convert.ToInt32(cboCustomer.SelectedValue);
            int orderId = _orderRepo.CreateOrder(customerId, Session.UserId, _items);
            MessageBox.Show($"Tạo đơn hàng thành công. Mã đơn: {orderId}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _items.Clear();
            BindItems();
            LoadLookups();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi lưu đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
