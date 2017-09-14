using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FakeNotepad
{
    class MultiPagePanel : System.Windows.Forms.Panel
    {
        private int iCurrentPageIndex;
        public int CurrendPageIndex
        {
            get { return iCurrentPageIndex; }
            set
            {
                if (value >= 0 && value < Controls.Count)
                {
                    Controls[value].BringToFront();
                    iCurrentPageIndex = value;
                }
            }
        }
        public void AddPage(Control page)
        {
            Controls.Add(page);
            page.Dock = DockStyle.Fill;
        }
    }
}
