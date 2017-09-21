using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace FakeNotepad
{
    //class DirTreePanel : System.Windows.Forms.Panel
    class DirTreeView : System.Windows.Forms.TreeView
    {
       // private System.Windows.Forms.TreeView treeView1;

        public DirTreeView()
        {

            InitializeComponent();
            TreeNode svrNode = new TreeNode("test");
            TreeNode side_Node = new TreeNode("asdf");
            side_Node.Nodes.Add("101", "aefe");
            side_Node.Nodes.Add("123wefe");

            svrNode.Nodes.Add(side_Node);

            //treeView1.Nodes.Add(svrNode);
            //treeView1.ExpandAll();
            //treeView1.BringToFront();
            this.Nodes.Add(svrNode);
            this.ExpandAll();
            this.BringToFront();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DirTreePanel
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Dock = System.Windows.Forms.DockStyle.Left;
            this.Indent = 8;
            this.LineColor = System.Drawing.Color.Black;
            this.Name = "treeView1";
            this.ResumeLayout(false);

        }
    }
}
