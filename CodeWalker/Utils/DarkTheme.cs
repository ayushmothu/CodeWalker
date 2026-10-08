using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeWalker
{
    public static class DarkTheme
    {
        public static readonly Color Back = Color.FromArgb(40, 42, 46);
        public static readonly Color BackAlt = Color.FromArgb(32, 34, 38);
        public static readonly Color BackInput = Color.FromArgb(28, 30, 34);
        public static readonly Color Fore = Color.WhiteSmoke;
        public static readonly Color ForeMuted = Color.FromArgb(180, 190, 200);
        public static readonly Color Accent = Color.FromArgb(160, 210, 255);
        public static readonly Color Border = Color.FromArgb(70, 74, 80);

        static readonly HashSet<Form> appliedForms = new HashSet<Form>();
        static bool autoEnabled;

        public static void Apply(Control root)
        {
            if (root == null) return;

            try
            {
                ApplyControl(root);
            }
            catch { }

            var form = root as Form ?? root.FindForm();
            if (form != null)
            {
                appliedForms.Add(form);
                form.FormClosed -= Form_FormClosed;
                form.FormClosed += Form_FormClosed;
                form.Shown -= Form_Shown;
                form.Shown += Form_Shown;
            }
        }

        public static void EnableApplicationWide()
        {
            if (autoEnabled) return;
            autoEnabled = true;
            Application.Idle += Application_Idle;
        }

        static void Application_Idle(object sender, EventArgs e)
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form == null || form.IsDisposed) continue;
                    if (!appliedForms.Contains(form))
                    {
                        Apply(form);
                    }
                    TrySetDarkTitleBar(form);
                }
            }
            catch { }
        }

        static void Form_Shown(object sender, EventArgs e)
        {
            var form = sender as Form;
            if (form == null) return;
            ApplyControl(form);
        }

        static void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as Form;
            if (form == null) return;
            appliedForms.Remove(form);
            form.FormClosed -= Form_FormClosed;
            form.Shown -= Form_Shown;
        }

        static void ApplyControl(Control c)
        {
            if (c == null) return;
            if (c is PictureBox) return;

            if (c is StatusStrip status)
            {
                status.BackColor = BackAlt;
                status.ForeColor = Fore;
                status.RenderMode = ToolStripRenderMode.System;
                foreach (ToolStripItem item in status.Items)
                {
                    item.ForeColor = Fore;
                    item.BackColor = BackAlt;
                }
            }
            else if (c is MenuStrip || c is ToolStrip)
            {
                var ts = (ToolStrip)c;
                ts.BackColor = BackAlt;
                ts.ForeColor = Fore;
                ts.RenderMode = ToolStripRenderMode.System;
                foreach (ToolStripItem item in ts.Items)
                {
                    item.ForeColor = Fore;
                    item.BackColor = BackAlt;
                }
            }
            else if (c is TabControl tabs)
            {
                tabs.BackColor = Back;
                tabs.ForeColor = Fore;
            }
            else if (c is TabPage page)
            {
                page.BackColor = Back;
                page.ForeColor = Fore;
                page.UseVisualStyleBackColor = false;
            }
            else if (c is GroupBox)
            {
                c.BackColor = Back;
                c.ForeColor = Fore;
            }
            else if (c is TextBox || c is RichTextBox || c is MaskedTextBox)
            {
                c.BackColor = BackInput;
                c.ForeColor = Fore;
            }
            else if (c is ComboBox combo)
            {
                combo.BackColor = BackInput;
                combo.ForeColor = Fore;
                combo.FlatStyle = FlatStyle.Flat;
            }
            else if (c is NumericUpDown nud)
            {
                nud.BackColor = BackInput;
                nud.ForeColor = Fore;
            }
            else if (c is ListBox || c is ListView || c is TreeView)
            {
                c.BackColor = BackInput;
                c.ForeColor = Fore;
            }
            else if (c is DataGridView dgv)
            {
                dgv.BackgroundColor = Back;
                dgv.GridColor = Border;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = BackAlt;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Fore;
                dgv.DefaultCellStyle.BackColor = BackInput;
                dgv.DefaultCellStyle.ForeColor = Fore;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 90, 130);
                dgv.DefaultCellStyle.SelectionForeColor = Fore;
            }
            else if (c is PropertyGrid pg)
            {
                pg.BackColor = Back;
                pg.ForeColor = Fore;
                pg.ViewBackColor = BackInput;
                pg.ViewForeColor = Fore;
                pg.HelpBackColor = BackAlt;
                pg.HelpForeColor = ForeMuted;
                pg.LineColor = BackAlt;
                pg.CategoryForeColor = Accent;
            }
            else if (c is Button btn)
            {
                btn.UseVisualStyleBackColor = true;
                btn.ForeColor = SystemColors.ControlText;
            }
            else if (c is CheckBox || c is RadioButton || c is Label)
            {
                if (!(c.Parent is Button))
                {
                    c.ForeColor = Fore;
                    if (c.BackColor == SystemColors.Control || c.BackColor == SystemColors.ControlLight
                        || c.BackColor == SystemColors.ControlDark || c.BackColor == Color.Transparent
                        || c.BackColor.A == 0 || IsLight(c.BackColor))
                    {
                        c.BackColor = Color.Transparent;
                    }
                }
            }
            else if (c is Panel || c is SplitContainer || c is SplitterPanel || c is FlowLayoutPanel
                     || c is TableLayoutPanel || c is Form)
            {
                if (IsLight(c.BackColor) || c.BackColor == SystemColors.Control
                    || c.BackColor == SystemColors.ControlDark || c.BackColor == SystemColors.Window)
                {
                    c.BackColor = (c is Form) ? BackAlt : Back;
                }
                c.ForeColor = Fore;
            }
            else if (c is ProgressBar)
            {
            }
            else
            {
                if (IsLight(c.BackColor))
                {
                    c.BackColor = Back;
                }
                if (IsDark(c.ForeColor) || c.ForeColor == SystemColors.ControlText)
                {
                    c.ForeColor = Fore;
                }
            }

            foreach (Control child in c.Controls)
            {
                ApplyControl(child);
            }
        }

        static bool IsLight(Color c)
        {
            double y = (0.299 * c.R) + (0.587 * c.G) + (0.114 * c.B);
            return y > 140;
        }

        static bool IsDark(Color c)
        {
            double y = (0.299 * c.R) + (0.587 * c.G) + (0.114 * c.B);
            return y < 80;
        }

        static void TrySetDarkTitleBar(Form form)
        {
            if (form == null || !form.IsHandleCreated) return;
            try
            {
                int useImmersiveDarkMode = 1;
                if (DwmSetWindowAttribute(form.Handle, 20, ref useImmersiveDarkMode, sizeof(int)) != 0)
                {
                    DwmSetWindowAttribute(form.Handle, 19, ref useImmersiveDarkMode, sizeof(int));
                }
            }
            catch { }
        }

        [DllImport("dwmapi.dll")]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
    }
}
