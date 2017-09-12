using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FakeNotepad
{
    public partial class TabDocControl : Control
    {
        private TextBoxForm tbForm = null;
        public TabDocControl()
        {
           
            tbForm = new TextBoxForm();
            InitializeComponent();
            tabPage1.Controls.Add(tbForm);
            //this.Dock = DockStyle.Fill;
            //ResumeLayout(false);

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
