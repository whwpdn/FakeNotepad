using System.Windows.Forms;
using System.Collections.Generic;
namespace FakeNotepad
{
    class CodeBox : System.Windows.Forms.RichTextBox
    {
        private string mstrSpace = "";
        private string mstrFileName = "";
        private char[] chrCheckingKeyChars = new char[] { '{', '(' };
        private int miCurrentLineNumber = 0;
        private UpdateLineNumberDelegate updateLineNumber;
        public UpdateLineNumberDelegate UpdateLineNumber
        {
            get { return updateLineNumber; }
            set { updateLineNumber = value; }
        }

        private UpdateCurrentLocation updateCurLoc;
        public UpdateCurrentLocation UpdateCurLoc
        {
            get { return this.updateCurLoc; }
            set { this.updateCurLoc = value; }
        }
        public string FileName
        {
            get { return mstrFileName; }
            set { mstrFileName = value; }
        }
        public bool SavedOrOpened
        {
            get { return mstrFileName.Length > 0; }
        }
        
        public CodeBox()
        {
            
            InitializeComponent();
            this.BorderStyle = BorderStyle.None;
        }

        // event functions
        private void VScrollEvent(object sender, System.EventArgs e)
        {
            updateLineNumber(this);
        }

        private void TextChangedEvent(object sender, System.EventArgs e)
        {

            AutoIndention();
            
            //int iCurCol = this.SelectionStart - GetLineNumber(this.SelectionStart);
            //if (iCurCol == 1)
            //    updateLineNumber(this);
            //if (iCurCol == 1)
            //    updateLineNumber(this);
        }

        private void SelectionChangedEvent(object sender, System.EventArgs e)
        {
           //System.Drawing.Point pt = this.GetPositionFromCharIndex(this.SelectionStart);
            int iCurLine = GetLineNumber();
            int iCurCol = this.SelectionStart - this.GetFirstCharIndexFromLine(iCurLine);
            updateCurLoc(++iCurLine, ++iCurCol);

            
            updateLineNumber(this);


        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode.Equals(Keys.Enter))
            {
                mstrSpace = GetWhiteSpaceFromCurrentLine();
            }

        }
        private int getWidth()
        {
            int iWidth = 25;
            // get total lines of richTextBox
            int iLine = this.Lines.Length;

            if (iLine <= 99)
            {
                iWidth = 20 + (int)this.Font.Size;
            }
            else if (iLine <= 999)
            {
                iWidth = 30 + (int)this.Font.Size;
            }
            else
            {
                iWidth = 50 + (int)this.Font.Size;
            }

            return iWidth;
        }
        private void AutoIndention()
        {
            if (mstrSpace.Length > 0)
            {
                int lastSelection = this.SelectionStart;
                //this.Text = Text.Insert(lastSelection, mstrSpace);
                this.Text += mstrSpace;
                string test = this.SelectedText;
                this.SelectionStart = lastSelection + mstrSpace.Length;
                mstrSpace = string.Empty;
            }
            
        }
        private int GetLineNumber()
        {
            return this.GetLineNumber(this.GetFirstCharIndexOfCurrentLine());
        }

        private int GetLineNumber(int intCharIndex)
        {
            return this.GetLineFromCharIndex(intCharIndex);
        }


      
        private string GetWhiteSpaceFromCurrentLine()
        {
            System.Text.StringBuilder tempstrSpace= new System.Text.StringBuilder();
            //int lineIndex = this.GetLineFromCharIndex(this.SelectionStart) ;
            int lineIndex = GetLineNumber(this.SelectionStart);

            if (this.Lines.Length > 0)
            {
                for (int i = 0; i < this.Lines[lineIndex].Length; i++)
                {
                    // If iterated char is equal to 
                    if (this.Lines[lineIndex][i].Equals('\t'))
                    {
                        tempstrSpace.Append('\t');
                    }
                    else if (this.Lines[lineIndex][i].Equals(' '))
                    {
                        tempstrSpace.Append(' ');
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return tempstrSpace.ToString();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CodeBox
            // 

            // style
            this.Dock = DockStyle.Fill;
            //this.BorderStyle = BorderStyle.None;
            this.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            this.SelectionIndent = 8;
            //this.SelectionHangingIndent = 3;
            //this.SelectionRightIndent = 12;

            ///
            this.AcceptsTab = true;
            base.AllowDrop = true;

            // set event haandler
            this.VScroll += new System.EventHandler(this.VScrollEvent);
            this.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.SelectionChanged += new System.EventHandler(this.SelectionChangedEvent);
            

            this.ResumeLayout(false);

        }
        ////

    }
}
