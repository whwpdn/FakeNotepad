using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

//using System.Windows.Forms;


/*
 * 
 * line number 랑 본문이랑 위치 안맞는 문제 
 * file메뉴단축키,상태바에 확장자,
 * edit menu
 * 
 * 
 */
namespace FakeNotepad
{
    delegate void UpdateLineNumberDelegate(CodeBox code);
    delegate void UpdateCurrentLocation(int iLineNum, int iColNum);
    delegate void LoadDropFiles(params string[] fileNames);

    public partial class Form1 : Form
    {
        private Image closeImage;
        #region Properties
        private CodeBox currentTabCode
        {
            get { return (CodeBox)codeTabControl.SelectedTab.Controls[0]; }
        }
     

        #endregion
        public Form1()
        {
            
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            // create instance
       
            // add controls
            //codeTabControl.TabPages.Add("untitled");
            codeTabControl.BringToFront();
            AddNewTab("untitled");
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            // 종료시 처리할 것들..(저장여부 , 설정값저장등등)
        }
        //단축키
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.W))
            {
                CloseSelectedTab();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Menu Click Event
        private void newToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            AddNewTab("untitled");
        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //CodeBox currentCode = (CodeBox)codeTabControl.SelectedTab.Controls[0];

            SaveFile();
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (var dlgOpenFile = new OpenFileDialog())
            {
                dlgOpenFile.Multiselect = true;
                dlgOpenFile.Filter = "All files (*.*)|*.*";
                if (dlgOpenFile.ShowDialog() == DialogResult.OK)
                {
                    LoadCodeFiles(dlgOpenFile.FileNames);
                }
            }
        }

        private void codeTabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            if (codeTabControl.TabCount.Equals(1))
            {
                UpdateControlAbilitys();
            }

            //Setup CodeBox control
            CodeBox newCodeBox = new CodeBox();
            LineNumberText newLineNumberText = new LineNumberText(newCodeBox);

            // set delegate
            newCodeBox.UpdateLineNumber = new UpdateLineNumberDelegate(newLineNumberText.SetLineNumbers);
            newCodeBox.UpdateCurLoc = new UpdateCurrentLocation(this.UpdateStatusStrip);
            newCodeBox.LoadDropFiles = new LoadDropFiles(this.LoadCodeFiles);

            newCodeBox.TextChanged += CodeBox_TextChanged;
            
            e.Control.Controls.Add(newCodeBox);
            e.Control.Controls.Add(newLineNumberText);
            e.Control.Width = 0;
            e.Control.Height = 0;

            // first call 
            newLineNumberText.SetLineNumbers(newCodeBox);
        }

        private void codeTabControl_MouseClick(object sender, MouseEventArgs e)
        {
            currentTabCode.Select();
            // tab close event process
            Rectangle rect = codeTabControl.GetTabRect(codeTabControl.SelectedIndex);
            Rectangle imageRec = new Rectangle(
                rect.Right - closeImage.Width,
                rect.Top + (rect.Height - closeImage.Height) / 2,
                closeImage.Width,
                closeImage.Height);

            if (imageRec.Contains(e.Location))
            {
                //codeTabControl.TabPages.Remove(codeTabControl.SelectedTab);
                CloseSelectedTab();
            }

        }

        private void codeTabControl_TabIndexChanged(object sender, System.EventArgs e)
        {
            currentTabCode.Focus();
        }

        private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveAsFile();
        }
        private void CodeBox_TextChanged(object sender, System.EventArgs e)
        {
            //UpdateAllInfo();
            UpdateRedoUndoMenuItems();
        }




        private void UpdateStatusStrip(int iLineNum, int iColumnNum)
        {
            this.CurrentPosition.Text = string.Format("Line {0}, Column {1}", iLineNum, iColumnNum);
        }

        private void AddNewTab(string filename)
        {
            CodeTabPage newTabPage = new CodeTabPage(filename);
            codeTabControl.TabPages.Add(filename);
            //codeTabControl.TabPages.Add(newTabPage);
            codeTabControl.SelectedIndex = codeTabControl.TabCount - 1;
            currentTabCode.Focus();
          
        }
        private void SaveFile()
        {
            if (currentTabCode.SavedOrOpened)
            {
                //overwrite
                //SaveFile(currentTabCode);

                try
                {
                    File.WriteAllLines(currentTabCode.FileName, currentTabCode.Lines);
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {

                }

                currentTabCode.Modified = false;
                UpdateTabTextModifiedIndicator();
            }
            else
            {
                SaveAsFile();
            }

        }
        private void SaveAsFile()
        {
            using (var dlgSaveFile = new SaveFileDialog())
            {
                dlgSaveFile.Filter = "All files (*.*)|*.*";
                
                // make filename with document first line (maximum 30)
                //CodeBox newCode = (CodeBox)codeTabControl.SelectedTab.Controls[0];
                if (currentTabCode.TextLength > 0 && currentTabCode.Lines[0].Length < 30)
                {
                    dlgSaveFile.FileName = currentTabCode.Lines[0];
                }
                else
                {
                    dlgSaveFile.FileName = currentTabCode.Lines[0].Substring(0, 30); ;
                }
                string ext = Path.GetExtension(dlgSaveFile.FileName).ToLower();

                // if no extension specified of extension is .txt
                if (ext.Length.Equals(0) || ext.Equals(".txt"))
                {
                    dlgSaveFile.FilterIndex = 1; // txt file
                }
                else
                {
                    dlgSaveFile.FilterIndex = 2; // other file
                }

                if (dlgSaveFile.ShowDialog() == DialogResult.OK)
                {
                    currentTabCode.FileName = dlgSaveFile.FileName;
                    try
                    {
                        System.IO.File.WriteAllLines(currentTabCode.FileName, currentTabCode.Lines);
                       
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                    }

                    string strFileName = Path.GetFileName(dlgSaveFile.FileName);
                    codeTabControl.SelectedTab.Text = strFileName;
                    currentTabCode.Modified = false;
                    UpdateTabTextModifiedIndicator();
                }
            
            }
        }
        
        private void LoadCodeFiles(params string[] fileNames)
        {
            codeTabControl.SuspendLayout();
            LoadIntoSeparateTabs(fileNames);
            
            //UpdateAllInfo();
            codeTabControl.ResumeLayout();
        }
        private void LoadIntoSeparateTabs(IEnumerable<string> fileNames)
        {
            //bool isOneFileAlreadyOpen = false;

            foreach (string name in fileNames)
            {
                if (name.Length == 0)
                {
                    AddNewTab("untitled");
                    continue;
                  }

                // If user decides not to load big file
                //if (!LoadLargeFile(name)) continue;
                //isOneFileAlreadyOpen = IsAlreadyOpen(name);
                //if (hasOneFileAlreadyOpen) continue;

                try
                {
                    string strFileName = Path.GetFileName(name);
                    AddNewTab(strFileName);
                    //codeTabControl.TabPages.Add(strFileName);
                    int index = codeTabControl.TabPages.Count - 1;
                    CodeBox openedCode = (CodeBox)codeTabControl.SelectedTab.Controls[0];
                    LineNumberText lineNumText = (LineNumberText)codeTabControl.SelectedTab.Controls[1];

                    openedCode.LoadFile(name, RichTextBoxStreamType.PlainText);
                    openedCode.FileName = name.TrimEnd();
                    openedCode.Modified = false;
                    lineNumText.SetLineNumbers(openedCode);
                    this.Text = name;
                                           
                }
                catch (System.Exception ex)
                {
                    codeTabControl.SelectedIndex = codeTabControl.TabCount - 1;
                    CloseSelectedTab();
                    MessageBox.Show(ex.Message);
                }
            }

            //if (!hasOneFileAlreadyOpen)
            //{
            //    tabControl.SelectedIndex = tabControl.TabCount - 1;
            //}

            //UpdateAllInfo();
        }
        private void CloseSelectedTab()
        {
            // A save prompt will only popup when document has been modified
            // and the current text does not matche up  with initial text state
            if (currentTabCode.Modified)
            {
                DialogResult result = MessageBox.Show("has been modified, save ? ","save", MessageBoxButtons.YesNoCancel);
                if ( result == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            RemoveTab();
            
        }
        private void RemoveTab()
        {
            // Remove all document dependent panels before closing last tab
            if (codeTabControl.TabCount == 1)
            {
             
            }
            
            currentTabCode.Dispose();
            //codeTabControl.TabPages.Remove(codeTabControl.SelectedTab);
            codeTabControl.TabPages.Remove(codeTabControl.SelectedTab);
            codeTabControl.SelectedIndex = codeTabControl.TabCount - 1;
        }
        private void UpdateTabTextModifiedIndicator()
        {
            string title = codeTabControl.SelectedTab.Text.TrimEnd('*');
            //CodeBox currentCode = (CodeBox)codeTabControl.SelectedTab.Controls[0];
            if (currentTabCode.SavedOrOpened && currentTabCode.Modified)
            {
                title += "*";
            }

            if (codeTabControl.SelectedTab.Text[codeTabControl.SelectedTab.Text.Length - 1] !=
                title[title.Length - 1])
            {
                codeTabControl.SelectedTab.Text = title;
            }
        }
        private void UpdateControlAbilitys()
        {
            //    bool isEnabled = (tabControl.TabCount > 0);
            //    tsmiEdit.Enabled = tsmiWordWrap.Enabled = isEnabled;
            //    tsmiFind.Enabled = tsmiGoto.Enabled = isEnabled;
            //    tsmiReplace.Enabled = tsmiInsert.Enabled = isEnabled;
            //    tsmiFindNext.Enabled = isEnabled;
            //    tsmiSaveAs.Enabled = tsmiSave.Enabled = isEnabled;
            //    tsmiShortDateTime.Enabled = tsmiLongDateTime.Enabled = isEnabled;
            //    tsmiPageSetup.Enabled = tsmiPrint.Enabled = isEnabled;
            //    tsmiPrintPreview.Enabled = tsmiCloseTab.Enabled = isEnabled;
            //    tsmiCloseAllTabs.Enabled = tsmiCloseAllButCurrent.Enabled = isEnabled;
            //    tsmiCloseAndDelete.Enabled = tsmiSelectAll.Enabled = isEnabled;
            //    tsmiDocumentInfo.Enabled = tsmiReadingWindow.Enabled = isEnabled;

            //    if (!isEnabled)
            //    {
            //        tsmiUndo.Enabled = tsmiRedo.Enabled = false;
            //    }
        }

        private void codeTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            // tabpage close image 
            Rectangle rect = codeTabControl.GetTabRect(e.Index);
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
            SizeF sz = e.Graphics.MeasureString(codeTabControl.TabPages[e.Index].Text, e.Font);
            // tab text
            e.Graphics.DrawString(
                codeTabControl.TabPages[e.Index].Text,
                e.Font, Brushes.Black, e.Bounds.Left+(e.Bounds.Width - sz.Width) / 5,
                e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1);

        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            //this.AllowDrop = true;
            Size mysize = new System.Drawing.Size(15, 15);
            Bitmap bt = new Bitmap(Properties.Resources.close);
            Bitmap btm = new Bitmap(bt, mysize);
            closeImage = btm;

            codeTabControl.Padding = new Point(30);
            UpdateRedoUndoMenuItems();
        }

        private void undoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (currentTabCode.Focused)
            {
                currentTabCode.CodeUndo();

                UpdateRedoUndoMenuItems();
            }
            else
            {
                UserControl userControl = this.ActiveControl as UserControl;

                if (userControl != null)
                {
                    TextBox textBox = userControl.ActiveControl as TextBox;
                    if (textBox != null) textBox.Undo();
                }
                else
                {
                    TextBox textBox = this.ActiveControl as TextBox;
                    if (textBox != null) textBox.Undo();
                }
            }
        }

        private void redoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (currentTabCode.Focused)
            {
                currentTabCode.CodeRedo();
                UpdateRedoUndoMenuItems();
            }
        }
        private void UpdateRedoUndoMenuItems()
        {
            //undoToolStripMenuItem.Enabled = currentTabCode.CanUndo;

            undoToolStripMenuItem.Enabled = currentTabCode.canUndo();
            redoToolStripMenuItem.Enabled = currentTabCode.canRedo();
        }

        private void hIdeSideBarToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if(hIdeSideBarToolStripMenuItem.Checked)
            {
                //dirTree.Hide();
                //splitContainer1.Panel1.Hide();
                hIdeSideBarToolStripMenuItem.Text = "show side bar";
                //this.splitContainer1.SplitterDistance = 0;
                this.splitContainer1.Panel1Collapsed = true;
            }
            
            else
            {
                //dirTree.Show();
                //splitContainer1.Panel1.Show();
                hIdeSideBarToolStripMenuItem.Text = "hide side bar";
                //this.splitContainer1.SplitterDistance = dirTree.Width;
                this.splitContainer1.Panel1Collapsed = false;

            }
            this.Invalidate();
        }

        private void openFolderToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (var dlgOpenFolder = new FolderBrowserDialog())
            {
                if (dlgOpenFolder.ShowDialog() == DialogResult.OK)
                {
                    dirTree.SetData(dlgOpenFolder.SelectedPath);
                //    LoadCodeFiles(dlgOpenFile.FileNames);
                }
            }
         
        }
    }
}
