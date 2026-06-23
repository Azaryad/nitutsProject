using System.Drawing;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Visual design language (Phase 10) — the single source of truth for the
    /// app's colors, fonts and control styling. <see cref="Apply"/> walks a
    /// panel's control tree and styles it consistently. It changes appearance
    /// only: it never touches event wiring, control positions/sizes, or the
    /// data a control shows — so there is no behavior change. The palette and
    /// rules are documented in CLAUDE.md › "Visual Design".
    ///
    /// Button roles are inferred from the colors the panels already used, so the
    /// established semantics are preserved without renaming anything:
    ///   DarkGreen  → Positive (Accent)   e.g. Send offer / Accept / Confirm / Save
    ///   DarkOrange → Caution  (Warning)  e.g. Requeue / Reject / Timeout
    ///   Firebrick  → Danger              e.g. Delete / Decline
    ///   otherwise  → Primary, or Secondary for Back/Clear/Refresh/Logout/Manage
    /// </summary>
    static class UiTheme
    {
        // ---- Palette ------------------------------------------------------
        public static readonly Color Primary      = ColorTranslator.FromHtml("#2563EB"); // brand blue
        public static readonly Color PrimaryDark  = ColorTranslator.FromHtml("#1D4ED8"); // hover/pressed
        public static readonly Color Accent       = ColorTranslator.FromHtml("#16A34A"); // positive (green)
        public static readonly Color AccentDark   = ColorTranslator.FromHtml("#15803D");
        public static readonly Color Danger        = ColorTranslator.FromHtml("#DC2626"); // destructive (red)
        public static readonly Color Warning       = ColorTranslator.FromHtml("#D97706"); // caution (amber)
        public static readonly Color PageBg        = ColorTranslator.FromHtml("#F4F6F8"); // page background
        public static readonly Color Surface       = Color.White;                          // cards / inputs / grids
        public static readonly Color TextDark      = ColorTranslator.FromHtml("#1F2937"); // primary text
        public static readonly Color TextMuted     = ColorTranslator.FromHtml("#6B7280"); // secondary text
        public static readonly Color Border        = ColorTranslator.FromHtml("#D1D5DB"); // neutral borders
        public static readonly Color GridHeaderBg  = ColorTranslator.FromHtml("#1F2937"); // dark grid header
        public static readonly Color GridAltRow    = ColorTranslator.FromHtml("#F3F6FB"); // zebra striping
        private static readonly Color HoverTint     = ColorTranslator.FromHtml("#F3F4F6");
        private static readonly Color PressTint      = ColorTranslator.FromHtml("#E5E7EB");

        // ---- Fonts --------------------------------------------------------
        public const string Family = "Segoe UI";
        public static Font Title()  => new Font(Family, 20F, FontStyle.Bold);
        public static Font Body()   => new Font(Family, 10F, FontStyle.Regular);
        public static Font Strong() => new Font(Family, 10F, FontStyle.Bold);

        // ---- Public entry -------------------------------------------------
        /// <summary>Style a panel/form and everything inside it. Call once, after InitializeComponent().</summary>
        public static void Apply(Control root)
        {
            if (root is UserControl || root is Form)
                root.BackColor = PageBg;
            StyleTree(root);
        }

        private static void StyleTree(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                switch (c)
                {
                    case Button b:           StyleButton(b); break;
                    case DataGridView g:     StyleGrid(g);   break;
                    case Label l:            StyleLabel(l);  break;
                    case TextBox t:          t.Font = Body(); t.BorderStyle = BorderStyle.FixedSingle; break;
                    case ComboBox cb:        cb.Font = Body(); break;
                    case DateTimePicker dt:  dt.Font = Body(); break;
                    case CheckBox ck:        ck.Font = Body(); ck.ForeColor = TextDark; break;
                    case NumericUpDown nud:  nud.Font = Body(); break;
                }
                if (c.HasChildren) StyleTree(c);   // recurse into GroupBox/Panel/TabPage containers
            }
        }

        private static bool Is(Color a, Color b) => a.ToArgb() == b.ToArgb();

        private static void StyleLabel(Label l)
        {
            // Titles: blue or large font → brand primary.
            if (Is(l.ForeColor, Color.DodgerBlue) || l.Font.Size >= 16f) { l.ForeColor = Primary; return; }
            // Already-muted summary text stays muted.
            if (Is(l.ForeColor, Color.DimGray) || Is(l.ForeColor, Color.Gray)) { l.ForeColor = TextMuted; return; }
            l.ForeColor = TextDark;
        }

        private static void StyleButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.Font = Strong();
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;

            Color fc = b.ForeColor;
            if (Is(fc, Color.DarkGreen))           Filled(b, Accent, AccentDark);   // positive
            else if (Is(fc, Color.Firebrick))      Outline(b, Danger, Danger);      // destructive
            else if (Is(fc, Color.DarkOrange))     Outline(b, Warning, Warning);    // caution
            else if (IsSecondary(b.Name))          Outline(b, Primary, Border);     // neutral nav
            else                                   Filled(b, Primary, PrimaryDark); // primary CTA
        }

        private static bool IsSecondary(string name)
        {
            string n = (name ?? "").ToLowerInvariant();
            return n.Contains("back") || n.Contains("logout") || n.Contains("clear")
                || n.Contains("refresh") || n.Contains("manage") || n.Contains("cancel")
                || n.Contains("close");
        }

        private static void Filled(Button b, Color bg, Color hover)
        {
            b.BackColor = bg;
            b.ForeColor = Color.White;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = hover;
            b.FlatAppearance.MouseDownBackColor = hover;
        }

        private static void Outline(Button b, Color text, Color border)
        {
            b.BackColor = Surface;
            b.ForeColor = text;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = border;
            b.FlatAppearance.MouseOverBackColor = HoverTint;
            b.FlatAppearance.MouseDownBackColor = PressTint;
        }

        /// <summary>Dark header, zebra rows, flat borders — applied to every report/list grid.</summary>
        public static void StyleGrid(DataGridView g)
        {
            g.EnableHeadersVisualStyles = false;
            g.BackgroundColor = Surface;
            g.BorderStyle = BorderStyle.None;
            g.GridColor = Border;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.RowHeadersVisible = false;
            g.AllowUserToResizeRows = false;
            g.RowTemplate.Height = 28;
            g.ColumnHeadersHeight = 34;

            DataGridViewCellStyle h = g.ColumnHeadersDefaultCellStyle;
            h.BackColor = GridHeaderBg;
            h.ForeColor = Color.White;
            h.Font = Strong();
            h.SelectionBackColor = GridHeaderBg;
            h.SelectionForeColor = Color.White;

            DataGridViewCellStyle d = g.DefaultCellStyle;   // modified in place — preserves NullValue etc.
            d.Font = Body();
            d.SelectionBackColor = Primary;
            d.SelectionForeColor = Color.White;
            d.Padding = new Padding(4, 2, 4, 2);

            g.AlternatingRowsDefaultCellStyle.BackColor = GridAltRow;
        }
    }
}
