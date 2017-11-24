using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

//using System.Windows.Forms;


/*
 * 
 * file메뉴단축키,상태바에 확장자,
 * edit menu
 * 
 */
namespace FakeNotepad
{
    //delegate void UpdateLineNumberDelegate(CodeBox code);
    delegate void UpdateCurrentLocation(int iLineNum, int iColNum);
    delegate void LoadDropFiles(params string[] fileNames);
    delegate bool ClosingTabPageEvent();

    public partial class Form1 : Form
    {
        //private Image closeImage;
        //private CodeTabPage tempTabPage = null;
        #region Properties
        private CodeBox currentTabCode
        {
            get { return (CodeBox)codeTabControl.SelectedTab.Controls[0]; }
        }
     

        #endregion
        //private Gui.OpenFileList openFilesView;
       
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
            //this.codeBox1.UpdateCurLoc = new UpdateCurrentLocation(this.UpdateStatusStrip);

            //tempTabPage = new CodeTabPage("temp",true);
            //((CodeBox)tempTabPage.Controls[0]).UpdateCurLoc = new UpdateCurrentLocation(this.UpdateStatusStrip);
            //tempTabPage.Controls[0].DragDrop += new DragEventHandler(this.FileDragDropEvent);
            
            // 임시 탭 페이지 richtextbox 이벤트 등록
            CodeBox tempCodeBox = ((CodeBox)codeTabControl.TempTabPage.Controls[0]);
            tempCodeBox.UpdateCurLoc = new UpdateCurrentLocation(this.UpdateStatusStrip);
            tempCodeBox.DragDrop += new DragEventHandler(this.FileDragDropEvent);
            tempCodeBox.LoadDropFiles = new LoadDropFiles(this.LoadCodeFiles);
            // 탭 종료 델리게이트
            codeTabControl.ClosingCodeTabPage = new ClosingTabPageEvent(this.CloseSelectedTab);

            //openFileView = new Gui.OpenFileList();

            // init temp tab page 
            //CodeBox newCodeBox = new CodeBox();
            //LineNumberText newLineNumberText = new LineNumberText(newCodeBox);
            //LineNumbers.LineNumbers_For_RichTextBox lineNumberRTB = new LineNumbers.LineNumbers_For_RichTextBox();
            //lineNumberRTB.ParentRichTextBox = newCodeBox;
            // set delegate
            //newCodeBox.UpdateLineNumber = new UpdateLineNumberDelegate(newLineNumberText.SetLineNumbers);
            //newCodeBox.UpdateCurLoc = new UpdateCurrentLocation(this.UpdateStatusStrip);
            
            //newCodeBox.LoadDropFiles = new LoadDropFiles(this.LoadCodeFiles);
            
            //newCodeBox.TextChanged += CodeBox_TextChanged;
            //newCodeBox.DragDrop += new DragEventHandler(this.FileDragDropEvent);

            //tempTabPage.Controls.Add(newCodeBox);
            //tempTabPage.Controls.Add(newLineNumberText);
            //tempTabPage.Controls.Add(lineNumberRTB);
            //codeTabControl.TabPages.Add(tempTabPage);
     
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

        private CodeTabPage AddNewTab(string filename)
        {
            CodeTabPage newTabPage = new CodeTabPage(filename);
            //codeTabControl.TabPages.Add(filename);
            //codeTabControl.TabPages.Add(newTabPage);
            codeTabControl.TabPages.Add(newTabPage);
            codeTabControl.SelectedIndex = codeTabControl.TabCount - 1;
            currentTabCode.UpdateCurLoc = new UpdateCurrentLocation(this.UpdateStatusStrip);
            currentTabCode.DragDrop += new DragEventHandler(this.FileDragDropEvent);
            currentTabCode.LoadDropFiles = new LoadDropFiles(this.LoadCodeFiles);
            currentTabCode.Focus();

            return newTabPage;
          
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
            foreach (string name in fileNames)
            {
                if (name.Length == 0)
                {
                    AddNewTab("untitled");
                    continue;
                }
                string strFileName = Path.GetFileName(name);

                //해당파일 열린 탭이 없는경우
                if(!this.openFileList.OpenFiles.Contains(name))
                {
                    CodeTabPage AddedPage = AddNewTab(strFileName);
                    //AddedPage.TabPageFilePath = name;
                    AddedPage.LoadIntoSeparateTabs(name);
                    this.Text = name;   // form title 

                    this.openFileList.AddListItem(name);
                }
                else    // 파일이 이미 열려있는 경우
                {
                    // tab select
                    CodeTabPage tabpage = FindTabPageByFilePath(name);
                    if(tabpage != null)
                        this.codeTabControl.SelectTab((TabPage)tabpage);

                }
            }
            //UpdateAllInfo();
            codeTabControl.ResumeLayout();
        }

        private CodeTabPage FindTabPageByFilePath(string filepath)
        {
            
            foreach (CodeTabPage e in codeTabControl.TabPages)
            {
                
                if (e.CodeBoxInTabPage.FileName == filepath)    // 파일경로와 같은 탭페이지 찾기
                {
                    if (e.IsTempPage) continue; // 임시탭페이지인경우 제외
                    return e;
                }
                    
            }

            return null;
            
        }


        private bool CloseSelectedTab()
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
                    return false;
                }
            }

            //RemoveTab();
            //this.openFileList.OpenFiles.Remove(currentTabCode.FileName);
            this.openFileList.RemoveListItem(currentTabCode.FileName);
            return true;
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
       
       
        private void Form1_Load(object sender, System.EventArgs e)
        {
            //this.AllowDrop = true;
            //Size mysize = new System.Drawing.Size(15, 15);
            //Bitmap bt = new Bitmap(Properties.Resources.close);
            //Bitmap btm = new Bitmap(bt, mysize);
            //closeImage = btm;

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

            ShowHideSideBar();
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

        private void ShowHideSideBar()
        {
            if (hIdeSideBarToolStripMenuItem.Checked)
            {
                hIdeSideBarToolStripMenuItem.Text = "show side bar";
                this.splitContainer1.Panel1Collapsed = true;
            }
            else
            {
                hIdeSideBarToolStripMenuItem.Text = "hide side bar";
                this.splitContainer1.Panel1Collapsed = false;
            }
            this.Invalidate();
        }

        private void FileDragDropEvent(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string aFile in files)
            {
                FileAttributes attr = File.GetAttributes(aFile);
                if (attr.HasFlag(FileAttributes.Directory))// if true , dir\
                {
                    if (hIdeSideBarToolStripMenuItem.Checked)
                    {
                        hIdeSideBarToolStripMenuItem.Checked = false;
                        ShowHideSideBar();
                    }
                    this.dirTree.SetData(aFile);
                }
                else
                {
                    //LoadCodeFiles(dlgOpenFile.FileNames);
                    // load file
                }
            }
            
        }

        private void dirTree_Click(object sender, TreeNodeMouseClickEventArgs e )
        {
            string filePath = (string)e.Node.Tag;// +"\\" + e.Node.Text;
            FileAttributes attr = File.GetAttributes(filePath);
            if (!attr.HasFlag(FileAttributes.Directory))// if true , dir\
            {
                CodeTabPage tabpage= FindTabPageByFilePath(filePath);
                if (tabpage != null)
                {
                    this.codeTabControl.SelectTab((TabPage)tabpage);
                }
                else
                {
                    string fileName = Path.GetFileName(filePath);
                    // 임시로 파일 열기
                    CodeTabPage tempTabPage = codeTabControl.AddTempTabPage(filePath);
                    tempTabPage.LoadIntoSeparateTabs(filePath);
                    this.Text = filePath;   // form title
                }
                
                //tempTabPage.Controls[0].Focus();
                //tempTabPage.SetItalic(true);
                //tempTabPage.Text =  Path.GetFileName(filePath);

                //LoadIntoSeparateTabs(filePath, (CodeBox)tempTabPage.Controls[0]); //AddNewTab(filePath);
               
                
                //((LineNumberText)tempTabPage.Controls[1]).SetLineNumbers((CodeBox)tempTabPage.Controls[0]);
                //if (!codeTabControl.Contains(tempTabPage))
                //{
                //    codeTabControl.TabPages.Add(tempTabPage);
                //    codeTabControl.SelectTab((TabPage)tempTabPage); // 탭페이지 선택되도록 수정
                //}
                //else
                //{
                //    codeTabControl.SelectTab((TabPage)tempTabPage); 
                //}
                //codeTabControl.SelectedIndex = codeTabControl.TabCount - 1;
                
                //
                //tempTabPage.BringToFront();
            }
        }

        private void dirTree_DoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //e.Data.GetData(DataFormats.FileDrop);
            //e.Node.Parent.Text;

            //string test = GetRootPathFromTree(e.Node);
            string filePath = (string)e.Node.Tag;// +"\\" + e.Node.Text;

            FileAttributes attr = File.GetAttributes(filePath);
            if (!attr.HasFlag(FileAttributes.Directory))// if true , dir\
            {
                if (codeTabControl.TempTabPage.TabPageFilePath == filePath)
                    codeTabControl.RemoveTab(codeTabControl.TempTabPage);
                CodeTabPage tabpage = FindTabPageByFilePath(filePath);
                if (tabpage != null)
                {
                    this.codeTabControl.SelectTab((TabPage)tabpage);
                }
                else
                {
                    LoadCodeFiles(filePath); //AddNewTab(filePath);
                }
                
            }
                
        }

        //private string GetRootPathFromTree(TreeNode node)
        //{

        //    string path = node.Text;
        //    if(node.Parent != null)
        //    {
        //        path = GetRootPathFromTree(node.Parent) + "\\"+path;
        //    }
            
        //    return path;

        //}
        //private void testMenuToolStripMenuItem_Click(object sender, System.EventArgs e)
        //{
        //    //codeTabControl.SelectedTab.Visible = false;
        //    int idx = codeTabControl.SelectedIndex;
        //    //codeTabControl.Controls.RemoveAt(idx);

        //    codeTabControl.Controls.Add(tempTabPage);
        //    tempTabPage.Controls[0].Invalidate();
        //    tempTabPage.Controls[0].Focus();
        //}

        //private void testMenu2ToolStripMenuItem_Click(object sender, System.EventArgs e)
        //{
        //    int idx = codeTabControl.SelectedIndex;
        //    codeTabControl.Controls.RemoveAt(idx);
        //}

        //private void codeTabControl_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    // tabpage close image 
        //    Rectangle rect = codeTabControl.GetTabRect(e.Index);
        //    Rectangle imageRec = new Rectangle(
        //        rect.Right - closeImage.Width,
        //        rect.Top + (rect.Height - closeImage.Height) / 2,
        //        closeImage.Width,
        //        closeImage.Height);
        //    // size rect
        //    rect.Size = new Size(rect.Width + 24, 38);
        //    Brush br = Brushes.Black;
        //    StringFormat strF = new StringFormat(StringFormat.GenericDefault);
        //    e.Graphics.DrawImage(closeImage, imageRec);
        //    SizeF sz = e.Graphics.MeasureString(codeTabControl.TabPages[e.Index].Text, e.Font);
        //    // tab text
        //    e.Graphics.DrawString(
        //        codeTabControl.TabPages[e.Index].Text,
        //        e.Font, Brushes.Black, e.Bounds.Left+(e.Bounds.Width - sz.Width) / 5,
        //        e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1);

        //}

        //private void LoadIntoSeparateTabs(IEnumerable<string> fileNames, CodeBox openedCode)
        //private void LoadIntoSeparateTabs(string name, CodeBox openedCode)
        //{
        //    //bool isOneFileAlreadyOpen = false;

        //    //foreach (string name in fileNames)
        //    //{
        //    //    if (name.Length == 0)
        //    //    {
        //    //        AddNewTab("untitled");
        //    //        continue;
        //    //      }

        //        // If user decides not to load big file
        //        //if (!LoadLargeFile(name)) continue;
        //        //isOneFileAlreadyOpen = IsAlreadyOpen(name);
        //        //if (hasOneFileAlreadyOpen) continue;

        //        try
        //        {
        //            string strFileName = Path.GetFileName(name);

        //            //codeTabControl.TabPages.Add(strFileName);
        //            //int index = codeTabControl.TabPages.Count - 1;
        //            //openedCode = (CodeBox)codeTabControl.SelectedTab.Controls[0];
        //            //LineNumberText lineNumText = (LineNumberText)codeTabControl.SelectedTab.Controls[1];
        //            //LineNumbers.LineNumbers_For_RichTextBox lineRTB = new LineNumbers.LineNumbers_For_RichTextBox();
        //            //lineRTB.ParentRichTextBox = openedCode;
        //            openedCode.Focus();
        //            openedCode.LoadFile(name, RichTextBoxStreamType.PlainText);
        //            openedCode.FileName = name.TrimEnd();
        //            openedCode.Modified = false;
        //            //lineNumText.SetLineNumbers(openedCode);
        //            this.Text = name;

        //        }
        //        catch (System.Exception ex)
        //        {
        //            codeTabControl.SelectedIndex = codeTabControl.TabCount - 1;
        //            CloseSelectedTab();
        //            MessageBox.Show(ex.Message);
        //        }
        //    //}

        //    //if (!hasOneFileAlreadyOpen)
        //    //{
        //    //    tabControl.SelectedIndex = tabControl.TabCount - 1;
        //    //}

        //    //UpdateAllInfo();
        //}

        //private void codeTabControl_ControlAdded(object sender, ControlEventArgs e)
        //{
        //    if (codeTabControl.TabCount.Equals(1))
        //    {
        //        //UpdateControlAbilitys();
        //    }

        //    if (e.Control == tempTabPage)
        //    {

        //        tempTabPage.Controls[0].Focus();
        //        return;
        //    }


        //    //Setup CodeBox control
        //    //CodeBox newCodeBox = new CodeBox();
        //    //LineNumberText newLineNumberText = new LineNumberText(newCodeBox);
        //    //LineNumbers.LineNumbers_For_RichTextBox lineNumberRTB = new LineNumbers.LineNumbers_For_RichTextBox();
        //    //lineNumberRTB.ParentRichTextBox = newCodeBox;
        //    // set delegate
        //    //newCodeBox.UpdateLineNumber = new UpdateLineNumberDelegate(newLineNumberText.SetLineNumbers);
        //    //newCodeBox.UpdateCurLoc = new UpdateCurrentLocation(this.UpdateStatusStrip);
        //    //newCodeBox.LoadDropFiles = new LoadDropFiles(this.LoadCodeFiles);

        //    //newCodeBox.TextChanged += CodeBox_TextChanged;
        //    //newCodeBox.DragDrop += new DragEventHandler(this.FileDragDropEvent);
        //    //e.Control.Controls.Add(newCodeBox);
        //    //e.Control.Controls.Add(newLineNumberText);
        //    //e.Control.Controls.Add(lineNumberRTB);
        //    //e.Control.Width = 0;
        //    //e.Control.Height = 0;

        //    //newLineNumberText.SetLineNumbers(newCodeBox);
        //}

        //private void codeTabControl_MouseClick(object sender, MouseEventArgs e)
        //{
        //    currentTabCode.Select();
        //    // tab close event process
        //    Rectangle rect = codeTabControl.GetTabRect(codeTabControl.SelectedIndex);
        //    Rectangle imageRec = new Rectangle(
        //        rect.Right - closeImage.Width,
        //        rect.Top + (rect.Height - closeImage.Height) / 2,
        //        closeImage.Width,
        //        closeImage.Height);

        //    if (imageRec.Contains(e.Location))
        //    {
        //        //codeTabControl.TabPages.Remove(codeTabControl.SelectedTab);
        //        CloseSelectedTab();
        //    }

        //}
    }
}
