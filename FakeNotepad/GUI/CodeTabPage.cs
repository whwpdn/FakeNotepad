using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FakeNotepad
{
    class CodeTabPage : TabPage
    {
        public CodeTabPage(string name)
        {
            this.Name = name;
            this.BorderStyle = BorderStyle.None;
            this.Width = 0;
            this.Height = 0;
            
            //this.Fo = false;            
            //Location = new System.Drawing.Point(0, 0);
            //Name = "tabPage1";
            //Padding = new Padding(3);
            //Size = new System.Drawing.Size(200, 100);
            //TabIndex = 0;
            //UseVisualStyleBackColor = true;
            Text = "untitled";
            //base.Dock = DockStyle.Fill;
            
        }
        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }
    }
}
