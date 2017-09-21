using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FakeNotepad
{
    class CodeTabControl : TabControl
    {
        public CodeTabControl()
        {
            InitializeComponent();

            //this.BackColor = System.Drawing.Color.Black;        
            //Location = new System.Drawing.Point(0, 120);
            //Name = "";
            //Size = new System.Drawing.Size(200, 100);
            
            //this.BorderStyle = BorderStyle.None; 
           
        }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        private void InitializeComponent()
        {
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SuspendLayout();
            this.TabStop = false;
            this.Dock = DockStyle.Fill;
            this.AllowDrop = true;
            this.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Multiline = true;
            this.Name = "codeTabControl";
            this.Padding = new System.Drawing.Point(0, 0);
            this.SelectedIndex = 0;
            this.Size = new System.Drawing.Size(564, 418);
            this.TabIndex = 1;
            this.ResumeLayout(false);


        }
    //protected override void WndProc(ref Message m)
    //{
    //    if (m.Msg == 0x1328 && !this.DesignMode)
    //        m.Result = new IntPtr(1);
    //    else
    //        base.WndProc(ref m);
    //}
    }
}
