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
    public partial class TextBoxForm : System.Windows.Forms.RichTextBox
    {
        public TextBoxForm()
        {
            Location = new System.Drawing.Point(0, 0);
            Name = "richTextBox1";
            Size = new System.Drawing.Size(100, 96);
            TabIndex = 0;
            Text = "";
            ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
