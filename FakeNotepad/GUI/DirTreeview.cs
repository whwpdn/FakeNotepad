using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace FakeNotepad
{
    //class DirTreePanel : System.Windows.Forms.Panel

    //폴더 넣ㅎ었을때 , 보이도록 , 
    class DirTreeView : System.Windows.Forms.TreeView
    {
       // private System.Windows.Forms.TreeView treeView1;

        public DirTreeView()
        {

            InitializeComponent();
            //TreeNode svrNode = new TreeNode("test");
            //TreeNode side_Node = new TreeNode("asdf");
            //side_Node.Nodes.Add("101", "aefe");
            //side_Node.Nodes.Add("123wefe");

            //svrNode.Nodes.Add(side_Node);

            //treeView1.Nodes.Add(svrNode);
            //treeView1.ExpandAll();
            //treeView1.BringToFront();
            //this.Nodes.Add(svrNode);
            this.ExpandAll();
            this.BringToFront();
        }
        
        public void SetData(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))// if true , dir\
                ListDirectory(path);
        }

        private void ListDirectory(string path)
        {
            //treeView.Nodes.Clear();
            ListDirectory(this, path);
        }
        private void ListDirectory(TreeView treeView, string path)
        {
            //treeView.Nodes.Clear();
           
            var rootDirectoryInfo = new DirectoryInfo(path);
            //bool test = treeView.Nodes.ContainsKey(rootDirectoryInfo.FullName);
            // root path
            if (!treeView.Nodes.ContainsKey(rootDirectoryInfo.FullName))
                treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo)); // recursive call
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {

            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())   // add dir node
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            foreach (var file in directoryInfo.GetFiles())      // add file node
            {
                TreeNode fileNode = new TreeNode(file.Name);
                //fileNode.Tag = Path.GetFullPath(file.Name);
                fileNode.Tag = directoryInfo.FullName +"\\"+ file.Name;
                //directoryNode.Nodes.Add(new TreeNode(file.Name));
                directoryNode.Nodes.Add(fileNode);
            }
                
            
            directoryNode.Name = directoryInfo.FullName;
            directoryNode.Tag = directoryInfo.FullName;
            return directoryNode;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DirTreeView
            // 
            this.AllowDrop = true;
            //this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //this.Dock = System.Windows.Forms.DockStyle.Left;
            this.Indent = 8;
            this.LineColor = System.Drawing.Color.Black;
            //this.Name = "treeView1";
            //this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DirTreeView_DragDrop);
            //this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DirTreeView_DragEnter);
            this.ResumeLayout(false);

        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            string[] files = (string[])drgevent.Data.GetData(DataFormats.FileDrop);

            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(drgevent.Data.GetData(DataFormats.FileDrop));
                foreach (string fileLoc in filePaths)
                {
                    FileAttributes attr = File.GetAttributes(fileLoc);
                    if (attr.HasFlag(FileAttributes.Directory))// if true , dir\
                        ListDirectory(fileLoc);
                }
            }

            base.OnDragDrop(drgevent);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                drgevent.Effect = DragDropEffects.Copy;
            }
            else
            {
                drgevent.Effect = DragDropEffects.None;
            }

 	        base.OnDragEnter(drgevent);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
        }

        //private void DirTreeView_DragDrop(object sender, DragEventArgs e)
        //{
        //    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
        //        foreach (string fileLoc in filePaths)
        //        {

        //            ListDirectory(fileLoc);
        //            //// Code to read the contents of the text file
        //            //if (File.Exists(fileLoc))
        //            //{
        //            //    using (TextReader tr = new StreamReader(fileLoc))
        //            //    {
        //            //        MessageBox.Show(tr.ReadToEnd());
        //            //    }
        //            //}

        //        }
        //    }
        //}

        //private void DirTreeView_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        e.Effect = DragDropEffects.Copy;
        //    }
        //    else
        //    {
        //        e.Effect = DragDropEffects.None;
        //    }
        //}
    }
}
