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
        private CodeTabPage tempTabPage;
        public CodeTabPage TempTabPage
        {
            get { return this.tempTabPage; }
        }

        private ClosingTabPageEvent closingCodeTabPage;
        public ClosingTabPageEvent ClosingCodeTabPage
        {
            get { return this.closingCodeTabPage; }
            set { this.closingCodeTabPage = value; }
        }

       

        public CodeTabControl()
        {
            InitializeComponent();
            
            // 탭 x 버튼 이미지 로드
            Size mysize = new System.Drawing.Size(15, 15);
            Bitmap bt = new Bitmap(Properties.Resources.close);
            Bitmap btm = new Bitmap(bt, mysize);
            closeImage = btm;

            tempTabPage = new CodeTabPage("temp", true);
            //((CodeBox)TempTabPage.Controls[0]).UpdateCurLoc = new UpdateCurrentLocation(this.UpdateStatusStrip);
            //TempTabPage.Controls[0].DragDrop += new DragEventHandler(this.FileDragDropEvent);
        }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        public void RemoveTab()
        {
            // Remove all document dependent panels before closing last tab
            //if (codeTabControl.TabCount == 1)
            //{

            //}

            //currentTabCode.Dispose();
            //codeTabControl.TabPages.Remove(codeTabControl.SelectedTab);

            //if (this.SelectedTab == tempTabPage)
            //{
                
                //return;
            //}
            //else
            //{
                //this.Controls[0].Dispose();
                this.TabPages.Remove(this.SelectedTab);
            //}
                       
            //this.SelectedIndex = this.TabCount - 1;
        }
        public void RemoveTab(TabPage tabpage)
        {
            this.TabPages.Remove(tabpage);
        }
        public CodeTabPage AddTempTabPage(string filepath)
        {
            string fileName = System.IO.Path.GetFileName(filepath);
            tempTabPage.Controls[0].Focus();
            tempTabPage.Text = fileName;
            //tempTabPage.TabPageFilePath = filepath;

            //this.SelectedTab = tempTabPage;
            //this.SelectTab((TabPage)tempTabPage);
            
            // 임시 탭 페이지가 추가 안되있는경우
            if (!this.Contains(tempTabPage))
            {
                this.TabPages.Add(tempTabPage);
                this.SelectedIndex = this.TabCount - 1;
               
            }
            else
            {
                this.SelectTab((TabPage)tempTabPage);
            }

            return tempTabPage;
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

        private void MouseClickEvent(object sender, MouseEventArgs e)
        {
            this.Select();
            // tab close event process
            Rectangle rect = this.GetTabRect(this.SelectedIndex);
            Rectangle imageRec = new Rectangle(
                rect.Right - closeImage.Width,
                rect.Top + (rect.Height - closeImage.Height) / 2,
                closeImage.Width,
                closeImage.Height);

            if (imageRec.Contains(e.Location))
            {
                //codeTabControl.TabPages.Remove(codeTabControl.SelectedTab);
                if (closingCodeTabPage())
                {
                    RemoveTab();
                }
            }

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CodeTabControl
            // 
            this.AllowDrop = true;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Multiline = true;
            this.Name = "codeTabControl";
            this.Padding = new System.Drawing.Point(0, 0);
            this.SelectedIndex = 0;
            this.Size = new System.Drawing.Size(564, 418);
            this.TabIndex = 1;
            this.TabStop = false;
            this.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.codeTabControl_DrawItem);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseClickEvent);
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
