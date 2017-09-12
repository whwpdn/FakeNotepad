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
            this.BackColor = System.Drawing.Color.Black;
            this.TabStop = false;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //Location = new System.Drawing.Point(0, 120);
            //Name = "";
            //Size = new System.Drawing.Size(200, 100);
            SelectedIndex = 0;
            //this.BorderStyle = BorderStyle.None;
            this.Dock = DockStyle.Fill;
            
        }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
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
