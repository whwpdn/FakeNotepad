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
            ListDirectory(this, path);
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            //treeView.Nodes.Clear();
            
            var rootDirectoryInfo = new DirectoryInfo(path);
            bool test = treeView.Nodes.ContainsKey(rootDirectoryInfo.FullName);
            if (!treeView.Nodes.ContainsKey(rootDirectoryInfo.FullName))
                treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo)); // recursive call
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {

            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Nodes.Add(new TreeNode(file.Name));
            directoryNode.Name = directoryInfo.FullName;
            return directoryNode;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DirTreeView
            // 
            this.AllowDrop = true;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Dock = System.Windows.Forms.DockStyle.Left;
            this.Indent = 8;
            this.LineColor = System.Drawing.Color.Black;
            this.Name = "treeView1";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DirTreeView_DragDrop);
            this.ResumeLayout(false);

        }

        private void DirTreeView_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        }
    }
}
