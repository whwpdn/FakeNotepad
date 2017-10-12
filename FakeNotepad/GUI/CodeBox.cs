using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
namespace FakeNotepad
{
    class CodeBox : System.Windows.Forms.RichTextBox
    {
        private TextUndoRedo mUndoRedo;
        //private int miUndoCount =0;
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
        //private bool bCulyBraceAutoIndent = false;
        private int miCheckingIndentionLevel=0;
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
            mUndoRedo = new TextUndoRedo();

           // SetFontHeight();
           
            
        }
        public void CodeUndo(){
            bSkipEvent = true;
            this.Text = this.mUndoRedo.Undo();
            this.SelectionStart = this.Text.Length;
            bSkipEvent = false;
        }
        public void CodeRedo(){
            bSkipEvent = true;
            string strRedo = this.mUndoRedo.Redo();
            if (strRedo != null)
            {
                this.Text = strRedo;
                this.SelectionStart = this.Text.Length;
            }
            bSkipEvent = false;

        }
        public bool canUndo()
        {
            return mUndoRedo.CanUndo();
        }
        public bool canRedo()
        {
            return mUndoRedo.CanRedo();
        }
        // event functions
        private void VScrollEvent(object sender, System.EventArgs e)
        {
            updateLineNumber(this);
        }

        private void TextChangedEvent(object sender, System.EventArgs e)
        {
            //bCulyBraceAutoIndent = false;

            AutoIndention();
            mUndoRedo.Save(this.Text);
        }

        private void SelectionChangedEvent(object sender, System.EventArgs e)
        {
            int iCurLine = GetLineNumber();
            int iCurCol = GetColumnIndex();
            updateCurLoc(++iCurLine, ++iCurCol);
                        
            updateLineNumber(this);
        }

       

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            string[] files = (string[])drgevent.Data.GetData(DataFormats.FileDrop);
            foreach (string aFile in files)
            {
                FileAttributes attr = File.GetAttributes(aFile);
                if (attr.HasFlag(FileAttributes.Directory))// if true , dir\
                {

                }
                else
                {
                    loadDropFiles(aFile);
                }
            }

            base.OnDragDrop(drgevent);
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
                miCheckingIndentionLevel = iCheckingIndentionLevel / iTabSpaceSize;

            }
            else if (e.KeyCode.Equals(Keys.OemCloseBrackets) )
            {
                // ignore selection change
                bSkipEvent = true;
                if (miCheckingIndentionLevel > 0 && IsAutoIndentStatus())
                {
                   int lastSelection = this.SelectionStart;
                    //this.Text = this.Text.Remove(this.Text.Length - iTabSpaceSize ,);
                    this.Text = this.Text.Remove(lastSelection - iTabSpaceSize, iTabSpaceSize);
                    this.SelectionStart = lastSelection - iTabSpaceSize;// +mstrSpace.Length;
                    //miCheckingIndentionLevel--;
                }
                bSkipEvent = false;
            }

        }
        private bool IsAutoIndentStatus()
        {
            
            int lastSelection = this.SelectionStart;
            int iCurrentIdx = this.SelectionStart - this.GetFirstCharIndexFromLine(GetLineNumber());
            string[] tmpStringArray = this.Lines;
            string currentLineString = tmpStringArray[GetLineNumber()].Trim('\n');


            //if (iCurrentIdx < currentLineString.Length) return false;
            if (iCurrentIdx < (miCheckingIndentionLevel * iTabSpaceSize)) return false;
            
            for (int i = 0; i < currentLineString.Length; i++)
            {
                //iIndentLevel++;
                if (currentLineString[i] != ' ')
                {
                    return false;
                }

            }
             
            //if (miCheckingIndentionLevel != (iIndentLevel / 4)) return false;
            
            return true;
        }

        protected override void OnSelectionChanged(System.EventArgs e)
        {
            // ignore selection change
            if(!bSkipEvent)
                base.OnSelectionChanged(e);
        }
        protected override void OnTextChanged(System.EventArgs e)
        {
            // ignore selection change
            if (!bSkipEvent)
                base.OnTextChanged(e);
        }

        ///////////////////////////////////
        /////////////// private functions
        //////////////////////////////
        private void SetFontHeight()
        {
            System.Drawing.Font f = this.Font;
            // Shrink the font for minor compensation
            this.Font = new System.Drawing.Font(f.FontFamily, f.Size - 0.09f, f.Style);

        }
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
                //bCulyBraceAutoIndent = true;
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
            this.AcceptsTab = true;
            this.AllowDrop = true;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.SelectionChanged += new System.EventHandler(this.SelectionChangedEvent);
            this.VScroll += new System.EventHandler(this.VScrollEvent);
            this.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.ResumeLayout(false);

        }
        ////

    }
}
