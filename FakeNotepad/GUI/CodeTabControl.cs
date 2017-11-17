using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FakeNotepad
{
    class CodeTabControl : TabControl
    {
        private Image closeImage;

        public CodeTabControl()
        {
            InitializeComponent();
            
            Size mysize = new System.Drawing.Size(15, 15);
            Bitmap bt = new Bitmap(Properties.Resources.close);
            Bitmap btm = new Bitmap(bt, mysize);
            closeImage = btm;           
        }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        private void codeTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            // tabpage close image 
            // get the real bounds for the tab rectangle
            Rectangle rect = this.GetTabRect(e.Index);
            Rectangle imageRec = new Rectangle(
                rect.Right - closeImage.Width,
                rect.Top + (rect.Height - closeImage.Height) / 2,
                closeImage.Width,
                closeImage.Height);
            // size rect
            rect.Size = new Size(rect.Width + 24, 38);
            Brush br = Brushes.Black;
            StringFormat strF = new StringFormat(StringFormat.GenericDefault);
            e.Graphics.DrawImage(closeImage, imageRec);
            SizeF sz = e.Graphics.MeasureString(TabPages[e.Index].Text, e.Font);

            // tab text
            Font TabFont = new Font(e.Font.FontFamily, (float)10, FontStyle.Italic, GraphicsUnit.Pixel);
            //e.Graphics.DrawString(
            //    TabPages[e.Index].Text,
            //    e.Font, Brushes.Black, e.Bounds.Left + (e.Bounds.Width - sz.Width) / 5,
            //    //TabFont, Brushes.Black, e.Bounds.Left + (e.Bounds.Width - sz.Width) / 5,
            //    e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1);

            if(((CodeTabPage)TabPages[e.Index]).IsTempPage)
            {
                e.Graphics.DrawString(
                TabPages[e.Index].Text,
                //e.Font, Brushes.Black, e.Bounds.Left + (e.Bounds.Width - sz.Width) / 5,
                TabFont, Brushes.Black, e.Bounds.Left + (e.Bounds.Width - sz.Width) / 5,
                e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1);

            }
            else
            {
                e.Graphics.DrawString(
                TabPages[e.Index].Text,
                e.Font, Brushes.Black, e.Bounds.Left + (e.Bounds.Width - sz.Width) / 5,
                e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1);
            }
            // tabpage italic
            //Graphics g = e.Graphics;
            //Brush TextBrush;
            // get the item from the collection
            //Rectangle tabRect = this.GetTabRect(e.Index);
            //TabPage tabpage = this.TabPages[e.Index];
            //if (e.State == DrawItemState.Selected)
            //{
            //    // Draw a different background color, and don't paint a focus rectangle.
            //    TextBrush = new SolidBrush(Color.Blue);
            //    g.FillRectangle(Brushes.Gray, e.Bounds);

            //}
            //else
            //{
            //    TextBrush = new System.Drawing.SolidBrush(e.ForeColor);
            //}
          


        }

        private void InitializeComponent()
        {
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SuspendLayout();
            this.TabStop = false;
            this.Dock = DockStyle.Fill;
            this.AllowDrop = true;
            this.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Multiline = true;
            this.Name = "codeTabControl";
            this.Padding = new System.Drawing.Point(0, 0);
            this.SelectedIndex = 0;
            this.Size = new System.Drawing.Size(564, 418);
            this.TabIndex = 1;
            this.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.codeTabControl_DrawItem);
            this.ResumeLayout(false);


        }
    //protected override void WndProc(ref Message m)
    //{
    //    if (m.Msg == 0x1328 && !this.DesignMode)
    //        m.Result = new IntPtr(1);
    //    else
    //        base.WndProc(ref m);
    //}
    }
}
