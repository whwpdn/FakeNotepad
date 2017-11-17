using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FakeNotepad
{
    class CodeTabPage : TabPage
    {
        private bool bTempPage = false; // side bar에서 click 으로 load될 경우
        private LineNumbers.LineNumbers_For_RichTextBox lineNumbers_For_RichTextBox1;
        private CodeBox cb;
        public bool IsTempPage
        {
            get { return bTempPage; }
            set { bTempPage = value; }
        }
        public CodeTabPage()
        {
        }
        public CodeTabPage(string name, bool bMode = false)
        {
            InitializeComponent();
            this.Name = name;
            this.bTempPage = bMode;
            this.Text = name;
            //Font TabFont = new Font(this.Font.FontFamily, (float)10, FontStyle.Bold, GraphicsUnit.Pixel);
            //this.Font = TabFont;
            
        }
        //protected override bool ShowFocusCues
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
        
       
        public void SetItalic(bool bItalic)
        {
           
        }

        private void InitializeComponent()
        {
            
            this.cb = new FakeNotepad.CodeBox();
            this.lineNumbers_For_RichTextBox1 = new LineNumbers.LineNumbers_For_RichTextBox();
            this.SuspendLayout();
            // 
            // cb
            // 
            this.cb.AcceptsTab = true;
            this.cb.AllowDrop = true;
            this.cb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb.FileName = "";
            this.cb.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb.LoadDropFiles = null;
            this.cb.Location = new System.Drawing.Point(24, 3);
            this.cb.Margin = new System.Windows.Forms.Padding(0);
            this.cb.Name = "cb";
            this.cb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.cb.Size = new System.Drawing.Size(550, 386);
            this.cb.TabIndex = 1;
            this.cb.Text = "";
            this.cb.UpdateCurLoc = null;
            // 
            // lineNumbers_For_RichTextBox1
            // 
            this.lineNumbers_For_RichTextBox1._SeeThroughMode_ = false;
            this.lineNumbers_For_RichTextBox1.AutoSizing = true;
            this.lineNumbers_For_RichTextBox1.BackgroundGradient_AlphaColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lineNumbers_For_RichTextBox1.BackgroundGradient_BetaColor = System.Drawing.Color.LightSteelBlue;
            this.lineNumbers_For_RichTextBox1.BackgroundGradient_Direction = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lineNumbers_For_RichTextBox1.BorderLines_Color = System.Drawing.Color.White;
            this.lineNumbers_For_RichTextBox1.BorderLines_Style = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lineNumbers_For_RichTextBox1.BorderLines_Thickness = 1F;
            this.lineNumbers_For_RichTextBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.lineNumbers_For_RichTextBox1.DockSide = LineNumbers.LineNumbers_For_RichTextBox.LineNumberDockSide.None;
            this.lineNumbers_For_RichTextBox1.GridLines_Color = System.Drawing.Color.Azure;
            this.lineNumbers_For_RichTextBox1.GridLines_Style = System.Drawing.Drawing2D.DashStyle.Custom;
            this.lineNumbers_For_RichTextBox1.GridLines_Thickness = 1F;
            this.lineNumbers_For_RichTextBox1.LineNrs_Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.lineNumbers_For_RichTextBox1.LineNrs_AntiAlias = true;
            this.lineNumbers_For_RichTextBox1.LineNrs_AsHexadecimal = false;
            this.lineNumbers_For_RichTextBox1.LineNrs_ClippedByItemRectangle = true;
            this.lineNumbers_For_RichTextBox1.LineNrs_LeadingZeroes = true;
            this.lineNumbers_For_RichTextBox1.LineNrs_Offset = new System.Drawing.Size(0, 0);
            this.lineNumbers_For_RichTextBox1.Location = new System.Drawing.Point(3, 3);
            this.lineNumbers_For_RichTextBox1.Margin = new System.Windows.Forms.Padding(0);
            this.lineNumbers_For_RichTextBox1.MarginLines_Color = System.Drawing.Color.SlateGray;
            this.lineNumbers_For_RichTextBox1.MarginLines_Side = LineNumbers.LineNumbers_For_RichTextBox.LineNumberDockSide.None;
            this.lineNumbers_For_RichTextBox1.MarginLines_Style = System.Drawing.Drawing2D.DashStyle.Custom;
            this.lineNumbers_For_RichTextBox1.MarginLines_Thickness = 0.0F;
            this.lineNumbers_For_RichTextBox1.Name = "lineNumbers_For_RichTextBox1";
            this.lineNumbers_For_RichTextBox1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.lineNumbers_For_RichTextBox1.ParentRichTextBox = this.cb;
            this.lineNumbers_For_RichTextBox1.Show_BackgroundGradient = false;
            this.lineNumbers_For_RichTextBox1.Show_BorderLines = false;
            this.lineNumbers_For_RichTextBox1.Show_GridLines = false;
            this.lineNumbers_For_RichTextBox1.Show_LineNrs = true;
            this.lineNumbers_For_RichTextBox1.Show_MarginLines = false;
            this.lineNumbers_For_RichTextBox1.Size = new System.Drawing.Size(10, 386);
            this.lineNumbers_For_RichTextBox1.TabIndex = 0;
            
            // 
            // CodeTabPage
            // 
            this.Controls.Add(this.cb);
            this.Controls.Add(this.lineNumbers_For_RichTextBox1);
            this.Location = new System.Drawing.Point(4, 22);
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(556, 392);
            this.UseVisualStyleBackColor = true;
            this.ResumeLayout(false);

        }
    }
}
