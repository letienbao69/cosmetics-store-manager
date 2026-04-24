using System.Drawing;
using System.Windows.Forms;

namespace CosmeticsStoreManager.UI;

public static class Theme
{
    public static readonly Color AppBackground = Color.FromArgb(245, 247, 251);
    public static readonly Color Surface = Color.White;
    public static readonly Color SurfaceAlt = Color.FromArgb(248, 250, 252);
    public static readonly Color Primary = Color.FromArgb(37, 99, 235);
    public static readonly Color PrimaryDark = Color.FromArgb(30, 64, 175);
    public static readonly Color Sidebar = Color.FromArgb(15, 23, 42);
    public static readonly Color Success = Color.FromArgb(22, 163, 74);
    public static readonly Color Danger = Color.FromArgb(220, 38, 38);
    public static readonly Color TextPrimary = Color.FromArgb(15, 23, 42);
    public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
    public static readonly Color Border = Color.FromArgb(226, 232, 240);

    public static void ApplyForm(Form form, string title)
    {
        form.Text = title;
        form.BackColor = AppBackground;
        form.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        form.StartPosition = FormStartPosition.CenterScreen;
    }

    public static Panel CreateCard(int padding = 18)
    {
        return new Panel
        {
            BackColor = Surface,
            Padding = new Padding(padding),
            Margin = new Padding(10),
            BorderStyle = BorderStyle.FixedSingle
        };
    }

    public static Label CreatePageTitle(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = TextPrimary,
            Margin = new Padding(0, 0, 0, 6)
        };
    }

    public static Label CreateSubtitle(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
            ForeColor = TextMuted,
            Margin = new Padding(0, 0, 0, 10)
        };
    }

    public static Label CreateFieldLabel(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = TextPrimary,
            Margin = new Padding(0, 6, 0, 6)
        };
    }

    public static TextBox CreateTextBox(string placeholder = "")
    {
        return new TextBox
        {
            PlaceholderText = placeholder,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10.5F),
            Margin = new Padding(0, 0, 0, 12),
            Width = 240
        };
    }

    public static ComboBox CreateComboBox()
    {
        return new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10.5F),
            Margin = new Padding(0, 0, 0, 12),
            Width = 240
        };
    }

    public static NumericUpDown CreateNumeric()
    {
        return new NumericUpDown
        {
            Font = new Font("Segoe UI", 10.5F),
            Margin = new Padding(0, 0, 0, 12),
            Width = 240,
            ThousandsSeparator = true
        };
    }

    public static DateTimePicker CreateDatePicker()
    {
        return new DateTimePicker
        {
            Font = new Font("Segoe UI", 10.5F),
            Margin = new Padding(0, 0, 0, 12),
            Width = 240,
            Format = DateTimePickerFormat.Short,
            ShowCheckBox = true
        };
    }

    public static Button CreatePrimaryButton(string text, int width = 130)
    {
        return BuildButton(text, Primary, Color.White, width);
    }

    public static Button CreateSecondaryButton(string text, int width = 130)
    {
        return BuildButton(text, Color.White, TextPrimary, width, Border);
    }

    public static Button CreateSuccessButton(string text, int width = 130)
    {
        return BuildButton(text, Success, Color.White, width);
    }

    public static Button CreateDangerButton(string text, int width = 130)
    {
        return BuildButton(text, Danger, Color.White, width);
    }

    public static Button CreateSidebarButton(string text)
    {
        var btn = new Button
        {
            Text = text,
            Height = 46,
            Dock = DockStyle.Top,
            FlatStyle = FlatStyle.Flat,
            BackColor = Sidebar,
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(18, 0, 0, 0),
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(37, 99, 235);
        return btn;
    }

    private static Button BuildButton(string text, Color back, Color fore, int width, Color? border = null)
    {
        var btn = new Button
        {
            Text = text,
            Width = width,
            Height = 40,
            FlatStyle = FlatStyle.Flat,
            BackColor = back,
            ForeColor = fore,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 10, 10)
        };
        btn.FlatAppearance.BorderSize = border.HasValue ? 1 : 0;
        btn.FlatAppearance.BorderColor = border ?? back;
        return btn;
    }

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Surface;
        grid.BorderStyle = BorderStyle.None;
        grid.RowHeadersVisible = false;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.ReadOnly = true;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersHeight = 42;
        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(241, 245, 249),
            ForeColor = TextPrimary,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Alignment = DataGridViewContentAlignment.MiddleLeft
        };
        grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Surface,
            ForeColor = TextPrimary,
            Font = new Font("Segoe UI", 10F),
            SelectionBackColor = Color.FromArgb(219, 234, 254),
            SelectionForeColor = TextPrimary
        };
        grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = SurfaceAlt
        };
    }

    public static Panel CreateStatCard(string title, string value, string note)
    {
        var card = CreateCard(18);
        card.Width = 250;
        card.Height = 140;

        var lblTitle = new Label
        {
            Text = title,
            AutoSize = true,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = TextMuted
        };
        var lblValue = new Label
        {
            Text = value,
            AutoSize = true,
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = TextPrimary,
            Margin = new Padding(0, 10, 0, 8)
        };
        var lblNote = new Label
        {
            Text = note,
            AutoSize = true,
            Font = new Font("Segoe UI", 9F),
            ForeColor = TextMuted
        };

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };
        flow.Controls.Add(lblTitle);
        flow.Controls.Add(lblValue);
        flow.Controls.Add(lblNote);
        card.Controls.Add(flow);
        return card;
    }
}
