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
        private bool bTempPage = false;
        public bool IsTempPage
        {
            get { return bTempPage; }
            set { bTempPage = value; }
        }

        public CodeTabPage(string name, bool bMode = false)
        {
            this.Name = name;
            this.BorderStyle = BorderStyle.None;
            this.Width = 0;
            this.Height = 0;
            this.Padding = new System.Windows.Forms.Padding(0);
            this.Margin = new System.Windows.Forms.Padding(0);
            bTempPage = bMode;  // side bar에서 click 으로 load될 경우
            //this.Fo = false;            
            //Location = new System.Drawing.Point(0, 0);
            //Name = "tabPage1";
            //Padding = new Padding(3);
            //Size = new System.Drawing.Size(200, 100);
            //TabIndex = 0;
            //UseVisualStyleBackColor = true;
            this.Text = name;
            
            //base.Dock = DockStyle.Fill;
            
        }
        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }
        
        public void SetItalic(bool bItalic)
        {
            this.SetItalic(bItalic);
        }
    }
}
