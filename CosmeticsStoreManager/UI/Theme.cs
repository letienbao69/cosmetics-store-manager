using System.Drawing;
using System.Windows.Forms;

namespace CosmeticsStoreManager.UI;

public static class Theme
{
    // Core Palette
    public static readonly Color AppBackground = Color.FromArgb(255, 245, 248);
    public static readonly Color Surface = Color.FromArgb(255, 255, 255);
    public static readonly Color SurfaceAlt = Color.FromArgb(255, 240, 245);
    public static readonly Color Primary = Color.FromArgb(219, 63, 116);
    public static readonly Color PrimaryDark = Color.FromArgb(180, 35, 85);
    public static readonly Color PrimaryLight = Color.FromArgb(252, 220, 232);
    public static readonly Color Sidebar = Color.FromArgb(60, 18, 35);
    public static readonly Color SidebarHover = Color.FromArgb(90, 30, 55);
    public static readonly Color SidebarActive = Color.FromArgb(219, 63, 116);
    public static readonly Color Success = Color.FromArgb(14, 159, 110);
    public static readonly Color Danger = Color.FromArgb(229, 62, 62);
    public static readonly Color Warning = Color.FromArgb(214, 115, 38);
    public static readonly Color TextPrimary = Color.FromArgb(44, 13, 26);
    public static readonly Color TextMuted = Color.FromArgb(159, 99, 128);
    public static readonly Color Border = Color.FromArgb(240, 210, 225);

    // Form
    public static void ApplyForm(Form form, string title)
    {
        form.Text = title;
        form.BackColor = AppBackground;
        form.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        form.StartPosition = FormStartPosition.CenterScreen;
    }

    // Cards
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

    // Typography
    public static Label CreatePageTitle(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Primary,
            Margin = new Padding(0, 0, 0, 4)
        };
    }

    public static Label CreateSubtitle(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = false,
            Dock = DockStyle.Top,
            Height = 48,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
            ForeColor = TextMuted,
            Margin = new Padding(0, 0, 0, 6),
            AutoEllipsis = false
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
            Margin = new Padding(0, 6, 0, 4)
        };
    }

    // Form Controls
    public static TextBox CreateTextBox(string placeholder = "")
    {
        return new TextBox
        {
            PlaceholderText = placeholder,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10.5F),
            BackColor = Surface,
            ForeColor = TextPrimary,
            Margin = new Padding(0, 0, 0, 12),
            Width = 240
        };
    }

    public static ComboBox CreateComboBox()
    {
        return new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Popup,
            Font = new Font("Segoe UI", 10.5F),
            BackColor = Surface,
            ForeColor = TextPrimary,
            Margin = new Padding(0, 0, 0, 12),
            Width = 240
        };
    }

    public static NumericUpDown CreateNumeric()
    {
        return new NumericUpDown
        {
            Font = new Font("Segoe UI", 10.5F),
            BackColor = Surface,
            ForeColor = TextPrimary,
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
            CalendarMonthBackground = PrimaryLight,
            CalendarTitleBackColor = Primary,
            CalendarTitleForeColor = Color.White,
            CalendarForeColor = TextPrimary,
            Margin = new Padding(0, 0, 0, 12),
            Width = 240,
            Format = DateTimePickerFormat.Short,
            ShowCheckBox = true
        };
    }

    // Buttons
    public static Button CreatePrimaryButton(string text, int width = 130)
        => BuildButton(text, Primary, Color.White, width);

    public static Button CreateSecondaryButton(string text, int width = 130)
        => BuildButton(text, PrimaryLight, Primary, width, Border);

    public static Button CreateSuccessButton(string text, int width = 130)
        => BuildButton(text, Success, Color.White, width);

    public static Button CreateDangerButton(string text, int width = 130)
        => BuildButton(text, Danger, Color.White, width);

    public static Button CreateSidebarButton(string text)
    {
        var btn = new Button
        {
            Text = text,
            Height = 46,
            Dock = DockStyle.Top,
            FlatStyle = FlatStyle.Flat,
            BackColor = Sidebar,
            ForeColor = Color.FromArgb(255, 210, 225),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(18, 0, 0, 0),
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = SidebarHover;
        btn.FlatAppearance.MouseDownBackColor = SidebarActive;
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

    // DataGridView
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
            BackColor = PrimaryLight,
            ForeColor = Primary,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Alignment = DataGridViewContentAlignment.MiddleLeft
        };

        grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Surface,
            ForeColor = TextPrimary,
            Font = new Font("Segoe UI", 10F),
            SelectionBackColor = Color.FromArgb(252, 220, 232),
            SelectionForeColor = Primary
        };

        grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = SurfaceAlt
        };
    }

    // Stat Cards
    public static Panel CreateStatCard(string title, string value, string note)
    {
        var card = CreateCard(18);
        card.Width = 260;
        card.Height = 160;

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
            ForeColor = Primary,
            Margin = new Padding(0, 8, 0, 6)
        };

        var lblNote = new Label
        {
            Text = note,
            AutoSize = false,
            Width = 220,
            Height = 38,
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
