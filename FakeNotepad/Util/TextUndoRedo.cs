using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeNotepad
{
    class TextUndoRedo
    {
        //protected CodeBox txtBox;
        protected List<string> LastData = new List<string>();
        protected int undoCount = 0;

        protected bool undoing = false;
        protected bool redoing = false;

        public int UndoCnt
        {
            get { return undoCount; }
        }
        public TextUndoRedo()
        {
            //this.txtBox = txtBox;
            //LastData.Add(txtBox.Text);
            //LastData.Add("");
        }

        public void undo_Click(object sender, EventArgs e)
        {
            this.Undo();
        }
        public void redo_Click(object sender, EventArgs e)
        {
            this.Redo();
        }

        public string Undo()
        {
            try
            {
                undoing = true;
                ++undoCount;
                //txtBox.Text = LastData[LastData.Count - undoCount - 1];
                return LastData[LastData.Count - undoCount - 1];
            }
            catch { }
            finally 
            {
                this.undoing = false;
            }
            return null;
        }
        public string Redo()
        {
            try
            {
                if (undoCount == 0)
                    return null;

                redoing = true;
                --undoCount;
                //txtBox.Text = LastData[LastData.Count - undoCount - 1];
                return LastData[LastData.Count - undoCount - 1];
            }
            catch { }
            finally { this.redoing = false; }
            return null;
        }

        public void Save(string changedtext)
        {
            if (undoing || redoing)
                return;
            if (LastData.Count==0)
            {
                LastData.Add(changedtext);
                undoCount = 0;
                return;
            }

            //if (LastData[LastData.Count - 1] == txtBox.Text)
            if (LastData[LastData.Count - 1] == changedtext)
                return;

            LastData.Add(changedtext);
            undoCount = 0;
        }
        public bool CanUndo(){
            if (LastData.Count <= undoCount) return false;
            return true;
        }
        public bool CanRedo(){
            if (0== undoCount) return false;
            return true;
        }

    }
}
