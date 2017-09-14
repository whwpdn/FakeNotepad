using System.Windows.Forms;
using System.Collections.Generic;
namespace FakeNotepad
{
    class CodeBox : System.Windows.Forms.RichTextBox
    {
        private string mstrSpace = "";
        private string mstrFileName = "";
        private char[] chrStartCheckingKey = new char[] {'{', '('};
        //private char[] chrEndCheckingKey= new char[] {'}' , ')'};
        private const int iTabSpaceSize = 4;
        private bool bSkipEvent = false;
        //shift_tab_unindent": false
        //"tab_size": 4,
        //"translate_tabs_to_spaces": false,
        private bool bTranslateTabsToSpaces = true;
        //private int miCheckingIndentionLevel=0;
        //private int miCurrentLineNumber = 0;
        private LoadDropFiles loadDropFiles;
        public LoadDropFiles LoadDropFiles
        {
            get { return loadDropFiles; }
            set { loadDropFiles = value; }
        }
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
        }

        private void SelectionChangedEvent(object sender, System.EventArgs e)
        {
            int iCurLine = GetLineNumber();
            int iCurCol = GetColumnIndex();
            updateCurLoc(++iCurLine, ++iCurCol);
                        
            updateLineNumber(this);
        }
        private void FileDragDropEvent(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            loadDropFiles(files);
            //foreach (string file in files) Console.WriteLine(file);
        }

        //private void Form1_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        //}


        //}
        //////////////////////////////
        ///////////////// override functions
        //////////////////////////////

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (bTranslateTabsToSpaces)
            {
                if (keyData == Keys.Tab)
                {
                    this.SelectionLength = 0;
                    int numSpaces = iTabSpaceSize - ((this.SelectionStart - this.GetFirstCharIndexOfCurrentLine()) % iTabSpaceSize);
                    this.SelectedText = new string(' ', numSpaces);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //base.OnKeyDown(e);

            if (e.KeyCode.Equals(Keys.Enter))
            {
                int iCheckingIndentionLevel = GetWhiteSpaceFromCurrentLine();
                iCheckingIndentionLevel += IsExistCheckingKeyChars();

                for (int i = 0; i < iCheckingIndentionLevel; i++)
                {
                    if (bTranslateTabsToSpaces)
                    {
                        mstrSpace+=' ';
                    }
                    else
                    {
                        mstrSpace += "\t";
                    }
                }

            }
            else if (e.KeyCode.Equals(Keys.OemCloseBrackets) )
            {
                // ignore selection change
                bSkipEvent = true;

                int lastSelection = this.SelectionStart;
                this.Text = this.Text.Remove(this.Text.Length - iTabSpaceSize);
                this.SelectionStart = lastSelection + mstrSpace.Length;

                bSkipEvent = false;
            }

        }

        protected override void OnSelectionChanged(System.EventArgs e)
        {
            // ignore selection change
            if(!bSkipEvent)
                base.OnSelectionChanged(e);
        }


        ///////////////////////////////////
        /////////////// private functions
        //////////////////////////////

        private int IsExistCheckingKeyChars()
        {
            
            char ch = GetChar();
            for(int i =0;i<chrStartCheckingKey.Length; i++)
            {
                if (chrStartCheckingKey[i] == ch)
                {
                    int iLevel = 0;
                    iLevel++;
                    if (bTranslateTabsToSpaces) return iLevel * iTabSpaceSize;
                    else                        return iLevel ;
                }
                    
            }

            //for(int i=0;i<chrEndCheckingKey.Length; i++)
            //{
            //    if (chrEndCheckingKey[i] == ch)
            //    {
            //        int iLevel = 0;
            //        iLevel--;
            //        if (bTranslateTabsToSpaces) return iLevel * iTabSpaceSize;
            //        else return iLevel;
            //    }
                    
            //}
            return 0;
        }

        private void AutoIndention()
        {
            if (mstrSpace.Length > 0)
            {
                // ignore selection change
                bSkipEvent = true;

                int lastSelection = this.SelectionStart;
                this.Text = Text.Insert(lastSelection, mstrSpace);
                //this.Text += mstrSpace;
                //string test = this.SelectedText;
                this.SelectionStart = lastSelection + mstrSpace.Length;
                mstrSpace = string.Empty;

                bSkipEvent = false;
            }
            
        }


        private int GetColumnIndex()
        {
            // get Current Column Index
            int iCurCol = this.SelectionStart -
                this.GetFirstCharIndexFromLine(GetLineNumber());
            return iCurCol;
        }
        private char GetChar()
        {
            // get Current position character
            return GetChar(this.SelectionStart);
        }
        private char GetChar(int intCharIndex)
        {
            System.Drawing.Point ptCurrentCharPos; 
            if (intCharIndex != this.TextLength)
            {
                ptCurrentCharPos = this.GetPositionFromCharIndex(intCharIndex-1 );
            }
            else
            {
                ptCurrentCharPos = this.GetPositionFromCharIndex(intCharIndex);
            }
            return this.GetCharFromPosition(ptCurrentCharPos);
        }
        private int GetLineNumber()
        {
            return this.GetLineNumber(this.GetFirstCharIndexOfCurrentLine());
        }

        private int GetLineNumber(int intCharIndex)
        {
            return this.GetLineFromCharIndex(intCharIndex);
        }
        private int GetWhiteSpaceFromCurrentLine()
        {
            int iCheckingIndentionLevel = 0;
            int lineIndex = GetLineNumber(this.SelectionStart);
            char ckeckchar = ' ';
            if (!bTranslateTabsToSpaces)
                ckeckchar = '\t';

            if (this.Lines.Length > 0)
            {
                for (int i = 0; i < this.Lines[lineIndex].Length; i++)
                {
                    // If iterated char is equal to 
                    if (this.Lines[lineIndex][i].Equals(ckeckchar))
                    {
                        iCheckingIndentionLevel++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return iCheckingIndentionLevel;
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
            //this.SelectionIndent = 8;
            //this.SelectionHangingIndent = 3;
            //this.SelectionRightIndent = 12;

            ///
            this.AcceptsTab = true;
            base.AllowDrop = true;
            // set event haandler
            this.VScroll += new System.EventHandler(this.VScrollEvent);
            this.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.SelectionChanged += new System.EventHandler(this.SelectionChangedEvent);
            this.DragDrop += new DragEventHandler(this.FileDragDropEvent);
            

            this.ResumeLayout(false);

        }
        ////

    }
}
