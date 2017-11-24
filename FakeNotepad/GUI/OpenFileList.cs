using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FakeNotepad.Gui
{
    class OpenFileList : System.Windows.Forms.ListBox
    {
        private List<string> listOpenfiles;
        public List<string> OpenFiles
        {
            get { return this.listOpenfiles; }
            //set { this.listOpenfiles = value; }
        }

        public OpenFileList()
        {
            InitializeComponent();
            listOpenfiles = new List<string>();
            this.DisplayMember = "name";
            this.ValueMember = "path";
            //this.Dock = System.Windows.Forms.DockStyle.Fill;

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OpenFileList
            // 
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResumeLayout(false);

        }

        public void AddListItem(string strFilePath)
        {
            listOpenfiles.Add(strFilePath);
            string filename = System.IO.Path.GetFileName(strFilePath);
            Data.FileInfo item = new Data.FileInfo();
            item.Name = filename;
            item.Path = strFilePath;
            this.Items.Add(item);
        }

        public void RemoveListItem(string strFilePath)
        {
            string filename = System.IO.Path.GetFileName(strFilePath);
            listOpenfiles.Remove(strFilePath);

            for(int i =0; i<this.Items.Count ; i++)
            {
                Data.FileInfo item = this.Items[i] as Data.FileInfo;
                if(item.Path == strFilePath)
                {
                    this.Items.Remove(item);
                    break;
                }
            }
            
            
            //System.Windows.Forms.Lis
        }
    }
}
