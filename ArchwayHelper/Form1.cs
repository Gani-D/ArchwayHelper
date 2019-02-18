using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;

namespace ArchwayHelper
{

    public partial class FormMain : Form
    {

        public FormMain()
        {
            InitializeComponent();
            formMain = this;
        }

        public static string [] GetOwnTexts ()
        {
            return new string[] { formMain.textBoxOwnText.Text, formMain.textBoxOwnTextTwo.Text};
        }

        public static void SetLabelText (string labelText)
        {
            formMain.labelCurrentLineNum.Text = labelText;
        }
        
        
        private void buttonAccept_Click(object sender, EventArgs e)
        {
            Thread queryThread = new Thread(QueryResult);
            queryThread.Start();
        }

        public static string GetRichText ()
        {
            return formMain.textToCopy.Text;
        }

        private void QueryResult ()
        {
            richTextBoxResult.Text="Please wait...";
            Query temp = new Query();
            richTextBoxResult.Text = temp.GetMXQuery(comboBoxDomainName.Text);
        }
        
        
      

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelCSTTime.Text = TimeZones.CSTTime();
            labelPSTTime.Text = TimeZones.PSTTime();
            labelESTTime.Text = TimeZones.ESTTime();
            if (Tasks.CheckTasks(checkBoxMute.Checked)) { timer1.Interval = 60000; }
            else timer1.Interval = 10000;
            if (groupBoxUpcEvents.Visible)
            {
                labelUpcEvent.Text = Tasks.GetFirstTask();
                labelUpcEventTwo.Text = Tasks.GetSecondTask();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBoxHideUpcEvents.Checked = true;
        }


        

        private void buttonChangeFont_Click(object sender, EventArgs e)
        {
            DialogResult result = fontDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                Font font = fontDialog1.Font;
                this.richTextBoxResult.Font = font;
            }
        }

        private void buttonCopyTPerm_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("Please let me know when I could connect to you.");
        }

        private void buttonCopyTClose_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("This request has been completed. Please let us know if there is anything further we can assist with. Thank you!");
            
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Delay!");
        }
        
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cancel!");
        }

       
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHideUpcEvents.Checked)
            {
                groupBoxUpcEvents.Visible = false;
                int height = richTextBoxResult.Height;
                int width = richTextBoxResult.Width;
                richTextBoxResult.Size = new System.Drawing.Size(width, height + 55);
            }
            else
            {
                groupBoxUpcEvents.Visible = true;
                int height = richTextBoxResult.Height;
                int width = richTextBoxResult.Width;
                richTextBoxResult.Size = new System.Drawing.Size(width, height - 55);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 58)
            {
                e.Handled = true;
            }
        }

        private void buttonLineAccept_Click(object sender, EventArgs e)
        {
            TextCopy textCopy = new TextCopy();
            
           string [] ret = textCopy.LinNumChange(textToCopy.Text,textBoxLineChange.Text);
            textBoxLineChange.Text = "";
            labelCurrentLineNum.Text = ret[0];
            labelCopyStatus.Text = ret[1];

        }

        private void wordWrap_CheckedChanged(object sender, EventArgs e)
        {
            textToCopy.WordWrap = checkBoxWordWrap.Checked;
        }

        private void buttonRemoveChar_Click(object sender, EventArgs e)
        {
            TextCopy removeChar = new TextCopy();
            textToCopy.Text = removeChar.RemoveCharacter(textToCopy.Text, textRemChar.Text);
            labelRemChar.Text = "Character '" + textRemChar.Text + "' was removed";
            textRemChar.Text = "";

        }

        private void comboBoxDomainName_Click(object sender, EventArgs e)
        {
            if (comboBoxDomainName.Text == "Please type the domain name") comboBoxDomainName.Text = null ;
        }

        private void alwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBoxAlwaysOnTop.Checked;
        }

        private void checkBoxEnableCopy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnableCopy.Checked)
            {
                buttonCNextLine.Text = "Copy next line F8";
                buttonCPrevLine.Text = "Copy previous line F6";
                buttonCopy.Text = "Copy F7";
                buttonCopyOwnText.Text = "Copy F9";
                buttonCopyOwnTextTwo.Text = "Copy F10";
                this.globalHotkey = new GlobalHotkey();
                globalHotkey.Show();
            }
            else {
                globalHotkey.Close();
                buttonCNextLine.Text = "Copy next line";
                buttonCPrevLine.Text = "Copy previous line";
                buttonCopy.Text = "Copy";
                buttonCopyOwnText.Text = "Copy";
                buttonCopyOwnTextTwo.Text = "Copy";
            }
        }

         private void buttonCPrevLine_Click(object sender, EventArgs e)
        {
            TextCopy prevLine = new TextCopy();
            labelCopyStatus.Text = prevLine.GetPreviousLine(textToCopy.Text);
            
        }

        private void buttonCNextLine_Click(object sender, EventArgs e)
        {
            TextCopy nextLine = new TextCopy();
            labelCopyStatus.Text = nextLine.GetNextLine(textToCopy.Text);
            
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            TextCopy getLine = new TextCopy();
            labelCopyStatus.Text = getLine.GetCurrentLine(textToCopy.Text);
            labelCurrentLineNum.Text = TextCopy._lineNum.ToString();
        }

        

        private void checkBoxRemovLines_CheckedChanged(object sender, EventArgs e)
        {
            TextCopy removeLines = new TextCopy();
            textToCopy.Text = removeLines.RemoveEmptyLines(textToCopy.Text);
            checkBoxRemovLines.Checked = true;
            
        }

        private void textToCopy_TextChanged(object sender, EventArgs e)
        {
            checkBoxRemovLines.Enabled = true;
            
        }

        private void comboBoxDomainName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                buttonAccept_Click(this, new EventArgs());
            }
        }

        private void checkBoxRemoveTabs_CheckedChanged(object sender, EventArgs e)
        {
            TextCopy remTab = new TextCopy();
            textToCopy.Text = remTab.RemoveTabs(textToCopy.Text);
            checkBoxChangeTabs.Checked = true;
        }

        private void checkBoxChangeTabs_CheckedChanged(object sender, EventArgs e)
        {
            TextCopy changeTab = new TextCopy();
            textToCopy.Text = changeTab.ChangeTabsToEnter(textToCopy.Text);
            checkBoxChangeTabs.Checked = true;
        }

        

        private void timebox1_Leave(object sender, EventArgs e)
        {
           timebox1.Text= Tasks.TimeBoxLeave(timeReturn(), 0);
        }

        private string[] timeReturn ()
        {
            return new string[] { timebox1.Text, timebox2.Text, timebox3.Text, timebox4.Text, timebox5.Text };
        }

        private void CheckTimeClick (CheckBox checkbox, TextBox textbox)
        {
            if (textbox.Text.Length > 4) { }
            else checkbox.Checked = false;
        }

        private void checktime1_Click(object sender, EventArgs e)
        {
            CheckTimeClick(checktime1, timebox1);
        }

        private void checktime4_Click(object sender, EventArgs e)
        {
            CheckTimeClick(checktime4, timebox4);
        }

        private void timebox4_Leave(object sender, EventArgs e)
        {
            timebox4.Text = Tasks.TimeBoxLeave(timeReturn(), 3);
        }

        private void buttonSortSave_Click(object sender, EventArgs e)
        {
            Tasks tasks = new Tasks();
            tasks.SortTasks(new string[] { timebox1.Text, timebox2.Text, timebox3.Text, timebox4.Text, timebox5.Text},
                new string[] { timetext1.Text, timetext2.Text, timetext3.Text, timetext4.Text, timetext5.Text},
                new bool[] { checktime1.Checked, checktime2.Checked, checktime3.Checked, checktime4.Checked, checktime5.Checked }
                );
            
        }

        private void timebox2_Leave(object sender, EventArgs e)
        {
            timebox2.Text = Tasks.TimeBoxLeave(timeReturn(), 1);
        }

        private void timebox3_Leave(object sender, EventArgs e)
        {
            timebox3.Text = Tasks.TimeBoxLeave(timeReturn(), 2);
        }

        private void timebox5_Leave(object sender, EventArgs e)
        {
            timebox5.Text= Tasks.TimeBoxLeave(timeReturn(), 4);
        }

        private void checktime2_Click(object sender, EventArgs e)
        {
            CheckTimeClick(checktime2, timebox2);
        }

        private void checktime3_Click(object sender, EventArgs e)
        {
            CheckTimeClick(checktime3, timebox3);
        }

        private void checktime5_Click(object sender, EventArgs e)
        {
            CheckTimeClick(checktime5, timebox5);
        }

        private void buttonCopyOwnText_Click(object sender, EventArgs e)
        {
            TextCopy copyOwnText = new TextCopy();
            copyOwnText.CopyOwnText(textBoxOwnText.Text);
        }

        private void buttonCopyOwnTextTwo_Click(object sender, EventArgs e)
        {
            TextCopy copyOwnText = new TextCopy();
            copyOwnText.CopyOwnText(textBoxOwnTextTwo.Text);
            
        }

    }
}
