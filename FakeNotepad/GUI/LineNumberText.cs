using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace FakeNotepad
{
    class LineNumberText : RichTextBox
    {

        private readonly CodeBox currentCodeBox;
        //private float _fontHeight;

        public LineNumberText(CodeBox curCodeBox)
        {
            InitializeComponent();
            this.BorderStyle = BorderStyle.None;
            this.currentCodeBox = curCodeBox;
            //SetFontHeight();
        }

        // public 
        public void SetLineNumbers(CodeBox code)
        {
            AddLineNumber(code);
            Invalidate();
        }
        // public 
        public void SetLineNumbers(int iLineNum, int iWidth)
        {
           // AddLineNumber();
            Invalidate();
        }


       // private functions 
        private void SetFontHeight()
        {
            Font f = this.Font;
            // Shrink the font for minor compensation
            this.Font = new Font(f.FontFamily, f.Size - 0.09f, f.Style);
            
        }
        private void AddLineNumber(CodeBox code)
        {
            Point pt = new Point(0, 0);
            int iFirstIdx = code.GetCharIndexFromPosition(pt);
            int iFirstLine = code.GetLineFromCharIndex(iFirstIdx) ;

            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;

            int iLastIdx = code.GetCharIndexFromPosition(pt)+1;
            //int iLastIdx = code.SelectionStart;
            int iLastLine = code.GetLineFromCharIndex(iLastIdx);

            SelectionAlignment = HorizontalAlignment.Center;
            this.Text = "";
            this.Width = getWidth(code);

            for (int i = iFirstLine; i <= iLastLine; i++)
            {
                Text += i + 1 +"\n";
            }

        }
        private void AddLineNumber(int iLineNum, int iWidth)
        {
            //int iLastIdx = code.GetCharIndexFromPosition(pt);

            SelectionAlignment = HorizontalAlignment.Center;
            this.Text = "";
            this.Width = iWidth;


        }

        private int getWidth(CodeBox code)
        {
            int iWidth = 25;
            // get total lines of richTextBox1    
            int iLine = code.Lines.Length;

            if (iLine <= 99)
            {
                iWidth = 20 + (int)code.Font.Size;
            }
            else if (iLine <= 999)
            {
                iWidth = 30 + (int)code.Font.Size;
            }
            else
            {
                iWidth = 50 + (int)code.Font.Size;
            }

            return iWidth;
        }
        private void MouseDownEvent(object sender, MouseEventArgs e)
        {
            currentCodeBox.Select();
            this.DeselectAll();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LineNumberText
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Dock = System.Windows.Forms.DockStyle.Left;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.ReadOnly = true;
            this.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TabStop = false;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownEvent);
            this.ResumeLayout(false);

        }
    }
}
