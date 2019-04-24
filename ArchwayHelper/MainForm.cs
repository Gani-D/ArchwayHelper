using System;

using System.Drawing;
using System.Linq;

using System.Windows.Forms;

using System.Threading;

using System.Collections.Generic;

namespace ArchwayHelper
{

    public partial class FormMain : Form
    {

        public FormMain()
        {
            InitializeComponent();
            formMain = this;
            ApiConnector.InitializeClient();
            dateTimePickerOfficeOOOStartDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerOfficeOOOStartDate.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            dateTimePickerOfficeOOOEndDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerOfficeOOOEndDate.CustomFormat = "MM/dd/yyyy HH:mm:ss";
        }

        public string TypeCreds()
        {
            return "Please type credentials to enable functionality";
        }

        public void UpdateTheFormMailboxes()
        {
            OfficePS officePS = OfficePS.Instance;
            comboBoxOfficeOwners.Items.Clear();
            comboBoxOfficeDelegates.Items.Clear();
            comboBoxOfficeMailboxSettings.Items.Clear();
            comboBoxOfficeInformation.Items.Clear();
            comboBoxOfficeOOOMailbox.Items.Clear();
            comboBoxOfficeForward.Items.Clear();
            comboBoxOfficeLogsAudit.Items.Clear();
            if (officePS.CredsAreValid)
            {
                officePS.Get_Mailboxes();
                string[] listOfMailboxes = officePS.ListOfMailboxes;
                foreach (string mailAddress in officePS.ListOfMailboxes)
                {
                    comboBoxOfficeLogsAudit.Items.Add(mailAddress);
                    comboBoxOfficeOwners.Items.Add(mailAddress);
                    comboBoxOfficeDelegates.Items.Add(mailAddress);
                    comboBoxOfficeMailboxSettings.Items.Add(mailAddress);
                    comboBoxOfficeInformation.Items.Add(mailAddress);
                    comboBoxOfficeOOOMailbox.Items.Add(mailAddress);
                    comboBoxOfficeForward.Items.Add(mailAddress);
                }
                labelOfficeConfig.Text = "The mailbox list has been updated";
            }
            else
            {
                labelOfficeConfig.Text = TypeCreds();
            }
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
            if (comboBoxDomainName.Text.Contains('@'))
            {
                comboBoxDomainName.Text = comboBoxDomainName.Text.Split('@')[1];
            }
            if (!comboBoxDomainName.Items.Contains(comboBoxDomainName.Text)) comboBoxDomainName.Items.Add(comboBoxDomainName.Text);
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
                richTextBoxResult.Font = font;
                textToCopy.Font = font;
                richTextExtractEmails.Font = font;
                richTextBoxPassGen.Font = font;
                richTextBoxSmtpMess.Font = font;
                richTextBoxWhoisResult.Font = font;
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
            checkBoxRemoveTabs.Checked = true;
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

        private void buttonExtract_Click(object sender, EventArgs e)
        {
            EmailExtractor extract = new EmailExtractor();
            richTextExtractEmails.Text = extract.Extract(radioBtnExtEmail.Checked, richTextExtractEmails.Text, checkBoxExtRemoveDup.Checked);
        }

        private void buttonCopyToCopyPaste_Click(object sender, EventArgs e)
        {
            textToCopy.Text = richTextExtractEmails.Text;
            TextCopy textCopy = new TextCopy();

            string[] ret = textCopy.LinNumChange(textToCopy.Text, textBoxLineChange.Text);
            textBoxLineChange.Text = "";
            labelCurrentLineNum.Text = ret[0];
            labelCopyStatus.Text = ret[1];
        }

        private void checkBoxSmtpAuth_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxSmtpEnableAuth.Visible = checkBoxSmtpAuth.Checked;
        }

        private void buttonSmtpReadSet_Click(object sender, EventArgs e)
        {
            SaveSettings readSettings = new SaveSettings();
            string [] result =  readSettings.GetSettings();
            if (result != null)
            {
                labelSmtpInfo.Text = result[0];
                comboBoxSmtpServer.Text = result[1];
                textBoxSmtpPort.Text = result[2];
                textBoxSmtpFrom.Text = result[3];
                textBoxSmtpTo.Text = result[4];
                checkBoxSmtpAuth.Checked = result[5].Length < 5;
                textBoxSmtpUser.Text = result[6];
            }
            else
            {
                labelSmtpInfo.Text = "No settings were detected";
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveSettings writeSettings = new SaveSettings();
            writeSettings.SetSettings(comboBoxSmtpServer.Text, textBoxSmtpPort.Text, textBoxSmtpFrom.Text, textBoxSmtpTo.Text, checkBoxSmtpAuth.Checked, textBoxSmtpUser.Text);
            labelSmtpInfo.Text = "The settings were saved to HKCU\\Software\\ArchwayHelper";
        }

        
        private void DigitsOnly(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxPassGen.Enabled = true;
        }

        private void radioButtonPGStan_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxPassGen.Enabled = false;
        }

        private void buttonPGStart_Click(object sender, EventArgs e)
        {
            //public string PassGen(string quantity, bool upperCase, bool lowerCase, bool inclNumbers, bool inclSymbols,   
           // bool exclCharsDot, bool exclSimilarChars, bool startWithUpper, string length)
            //
            PasswordGenerator passGen = new PasswordGenerator();
            if (radioButtonPGStan.Checked)
            {
                richTextBoxPassGen.Text = passGen.PassGen(textBoxPGQuantity.Text);
            }
            else
            {
               richTextBoxPassGen.Text = passGen.PassGen(textBoxPGQuantity.Text, checkBoxPGUpperCase.Checked, checkBoxPGLowerCase.Checked, checkBoxPGNum.Checked,
               checkBoxPGSym.Checked, checkBoxPGExclChars.Checked, checkBoxPGExclO.Checked, checkBoxPGUpperCaseStart.Checked, comboBoxPGLen.Text);
            }
        }

        private void buttonWhoisGet_Click(object sender, EventArgs e)
        {
            Whois whoisServerData = new Whois();
            textBoxWhoisDomain.Text = textBoxWhoisDomain.Text.ToLower().Trim();
            richTextBoxWhoisResult.Text = whoisServerData.GetWhoisData(textBoxWhoisDomain.Text);
        }

        private void buttonCopyToCopyPaste_Click_1(object sender, EventArgs e)
        {
            textToCopy.Text = richTextExtractEmails.Text;
        }

        private void buttonSmtpSend_Click(object sender, EventArgs e)
        {
            labelSmtpInfo.Text = "";
            SmtpSender sendEmail = new SmtpSender();
            labelSmtpInfo.Text = sendEmail.SendEmail(comboBoxSmtpServer.Text, textBoxSmtpPort.Text, textBoxSmtpFrom.Text, 
                textBoxSmtpTo.Text, textBoxSmtpSubj.Text, richTextBoxSmtpMess.Text, 
                checkBoxSmtpAuth.Checked, textBoxSmtpUser.Text, textBoxSmtpPass.Text);
        }

        
        private void textBoxWhoisDomain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
               buttonWhoisGet_Click(this, new EventArgs());
            }
        }

        private void buttonSmtpRemove_Click(object sender, EventArgs e)
        {
            SaveSettings deleteSettings = new SaveSettings();
            labelSmtpInfo.Text = deleteSettings.RemoveSettings();
        }

        private void checkBoxSettingsDark_CheckedChanged(object sender, EventArgs e)
        {
            this.BackColor = Color.Red;
            richTextBoxResult.BackColor = Color.Gray;
            richTextBoxResult.ForeColor = Color.Wheat;

            MX.BackColor = Color.DarkSlateGray;
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            Sort tmp = new Sort();

            richTextBoxSort.Text = tmp.SortText(richTextBoxSort.Text);
        }

        private void buttonTabToEnter_Click(object sender, EventArgs e)
        {
            TextCopy changeTab = new TextCopy();
            richTextBoxSort.Text = changeTab.ChangeTabsToEnter(richTextBoxSort.Text);
        }

        private void buttonSortRemoveLines_Click(object sender, EventArgs e)
        {
            TextCopy removeLines = new TextCopy();
            richTextBoxSort.Text = removeLines.RemoveEmptyLines(richTextBoxSort.Text);
        }

        private async void buttonGeoQuery_Click(object sender, EventArgs e)
        {
            textBoxGeoIp.Text = textBoxGeoIp.Text.Trim();
            linkLabelGeoIp.Text = "https://mxtoolbox.com/SuperTool.aspx?action=arin%3a" + textBoxGeoIp.Text;
            GeoIpQuery geoIpQuery = GeoIpQuery.Instance;
            try
            {
                richTextBoxGeoIp.Text = await geoIpQuery.LoadIpInformation(textBoxGeoIp.Text);
            }
            catch
            {
                richTextBoxGeoIp.Text = "Cannot connect to the server";
            }
        }

        private void linkLabelGeoIp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabelGeoIp.Text);
        }

        private void buttonOfficeSetPermissions_Click(object sender, EventArgs e)
        {
            OfficePS officePS = OfficePS.Instance;
            
            if (officePS.CredsAreValid)
            {
                richTextBoxOfficeResult.Text = officePS.SelectPermissionAction(comboBoxOfficeOwners.Text, comboBoxOfficeDelegates.Text, checkBoxOfficeRemovPermissions.Checked,
                        radioButtonOfficeFullAccess.Checked, checkBoxOfficeAutoMap.Checked, radioButtonOfficeSendAs.Checked, radioButtonOfficeCalendar.Checked,
                        radioButtonOfficeContact.Checked, comboBoxAccessLevel.Text);
            }
            else
            {
                richTextBoxOfficeResult.Text = TypeCreds();
            }

            
        }

        private void buttonOfficeUpdate_Click(object sender, EventArgs e)
        {
           
                UpdateTheFormMailboxes();
            
        }

        private void radioButtonOfficeFullAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonOfficeFullAccess.Checked)
            {
                if (checkBoxOfficeRemovPermissions.Checked)
                {
                    checkBoxOfficeAutoMap.Visible = true;
                }
            }
            else
            {
                checkBoxOfficeAutoMap.Visible = false;
                
            }
        }

        private void checkBoxOfficeRemovPermissions_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxOfficeRemovPermissions.Checked)
            {
                groupBoxOfficeAccessLvl.Visible = false;
                checkBoxOfficeAutoMap.Visible = false;
            }
            else
            {
                if (radioButtonOfficeCalendar.Checked || radioButtonOfficeContact.Checked)
                {
                    groupBoxOfficeAccessLvl.Visible = true;
                }
                else if (radioButtonOfficeFullAccess.Checked)
                {
                    checkBoxOfficeAutoMap.Visible = true;
                }
            }
        }

        private void radioButtonOfficeCalendar_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonOfficeCalendar.Checked)
            {
                if (!checkBoxOfficeRemovPermissions.Checked)
                {
                    groupBoxOfficeAccessLvl.Visible = false;
                }
                else
                {
                    groupBoxOfficeAccessLvl.Visible = true;
                }
            }
            else
            {
                groupBoxOfficeAccessLvl.Visible = false;
            }
        }

        private void radioButtonContact_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonOfficeContact.Checked)
            {
                if (!checkBoxOfficeRemovPermissions.Checked)
                {
                    groupBoxOfficeAccessLvl.Visible = false;
                }
                else
                {
                    groupBoxOfficeAccessLvl.Visible = true;
                }
            }
            else
            {
                groupBoxOfficeAccessLvl.Visible = false;
            }
            OfficePS officePS = OfficePS.Instance;

            /*
            foreach(string smtpAddress in officePS.Get_MailboxFullAccessPermissions())
            {
                richTextBoxOfficeResult.Text += smtpAddress+'\n';
            }
            */
            
        }

        private void buttonOfficeDispose_Click(object sender, EventArgs e)
        {
            labelOfficeConfig.Text = TypeCreds();
            OfficePS officePS = OfficePS.Instance;
            officePS.DisposeSession();
        }

        private void buttonOfficeAccept_Click(object sender, EventArgs e)
        {
            OfficePS officePS = OfficePS.Instance;

            if (officePS.StartCode(textBoxOfficeLogin.Text.Trim(), textBoxOfficePass.Text.Trim()))
            {
                comboBoxOfficeOwners.Items.Clear();
                labelOfficeConfig.Text = "You are logged in";
                UpdateTheFormMailboxes();
            }
            else
            {
                labelOfficeConfig.Text = "Wrong credentials";
            }

        }


        private void buttonOfficeAlias_Click(object sender, EventArgs e)
        {
            OfficePS officePS = OfficePS.Instance;
            if (officePS.CredsAreValid)
            {
                richTextBoxOfficeAliasOwner.Text = officePS.AliasOwner(textBoxOfficeAlias.Text.Trim());
            }
            else
            {
                richTextBoxOfficeAliasOwner.Text = TypeCreds();
            }
        }

        private void buttonCopyTCall_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText("Please let me know when I could call you. Thank you!");
            LoadingForm t = LoadingForm.Instance;
            t.StartPosition = FormStartPosition.CenterParent;
           // t.Left = 0;
         //   t.Top = 300;
            new Thread(() => t.ShowDialog(this)).Start();
            
            //t.Close();
        }

        private void buttonOfficeForwarding_Click(object sender, EventArgs e)
        {
            OfficePS officePS = OfficePS.Instance;
            if (officePS.CredsAreValid)
            {
                labelOfficeMailboxSettingResult.Text = officePS.ForwardingAddingToGroupAction(comboBoxOfficeMailboxSettings.Text, comboBoxOfficeForward.Text, comboBoxOfficeGroups.Text,
                    checkBoxOfficeForwardKeepCopy.Checked, radioButtonOfficeForward.Checked, radioButtonOfficeRemoveForwarding.Checked, 
                    radioButtonOfficeGroups.Checked, checkBoxOfficeForwardingAddDG.Checked);
            }
        }
        
        private void radioButtonOfficeGroups_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonOfficeGroups.Checked)
            {
                
                OfficePS officePS = OfficePS.Instance;
                if (officePS.CredsAreValid)
                {
                    comboBoxOfficeGroups.Items.Clear();
                    foreach (string group in officePS.GetDistributionGroups())
                    comboBoxOfficeGroups.Items.Add(group);
                }
                else
                {
                    labelOfficeMailboxSettingResult.Text = TypeCreds();
                }
            }
        }

        private void buttonOfficeInfo_Click(object sender, EventArgs e)
        {
            
            checkedListBoxOfficeRuleToDelete.Items.Clear();
            OfficePS officePS = OfficePS.Instance;

            richTextBoxOfficeInformation.Text = officePS.GetInboxRules(comboBoxOfficeInformation.Text);
            foreach(string name in officePS.Rules)
            {
                checkedListBoxOfficeRuleToDelete.Items.Add(name);
            }
            
        }

        private void buttonOfficeRemoveRule_Click(object sender, EventArgs e)
        {
            OfficePS officePS = OfficePS.Instance;

            foreach (int checkedIndices in checkedListBoxOfficeRuleToDelete.CheckedIndices)
            {
                labelOfficeOOOInboxRuleLog.Text = officePS.RemoveInboxRule(comboBoxOfficeInformation.Text, checkedListBoxOfficeRuleToDelete.Items[checkedIndices].ToString());
            }
            buttonOfficeInfo.PerformClick();
        }

        private void buttonOfficeLogStart_Click(object sender, EventArgs e)
        {
            OfficePS officePS = OfficePS.Instance;
            if (radioButtonOfficeLogMailTrace.Checked)
            {
                dataGridViewLogs.DataSource = officePS.GetLogs(textBoxOfficeLogsSender.Text, textBoxOfficeLogsRecipient.Text, textBoxOfficeLogsIP.Text);
            }
            else if (radioButtonOfficeLogsForwarding.Checked)
            {
                dataGridViewLogs.DataSource = officePS.GetMailboxesWithForwarding();
            }
            else if (radioButtonOfficeLogsStat.Checked)
            {
                dataGridViewLogs.DataSource = officePS.GetLogInData(comboBoxOfficeLogsAudit.Text);
            }
            else
            {
                dataGridViewLogs.DataSource = officePS.GetInboxRulesCount();
            }
         }

        

        private void buttonOfficeOOO_Click(object sender, EventArgs e)
        {
            OfficePS officePS = OfficePS.Instance;
            labelOfficeOOOLog.Text = officePS.SetOutOfOffice(comboBoxOfficeOOOMailbox.Text, radioButtonOfficeOOODisable.Checked, radioButtonOfficeOOOEnableNow.Checked,
                radioButtonOfficeOOOSchedule.Checked, dateTimePickerOfficeOOOStartDate.Text, dateTimePickerOfficeOOOEndDate.Text, richTextBoxOfficeOOOText.Text);
        }

        private void radioButtonOfficeOOOSchedule_CheckedChanged(object sender, EventArgs e)
        {
            
                groupBoxOfficeOOOSchedule.Visible = radioButtonOfficeOOOSchedule.Checked;
            
        }

       

        private void comboBoxOfficeInformation_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBoxOfficeRuleToDelete.Items.Clear();
            richTextBoxOfficeInformation.Text = "";
        }

        private void radioButtonOfficeLogsStat_CheckedChanged(object sender, EventArgs e)
        {
            
                labelOfficeLogsAudit.Visible = radioButtonOfficeLogsStat.Checked;
                comboBoxOfficeLogsAudit.Visible = radioButtonOfficeLogsStat.Checked;
            
        }

        private void radioButtonOfficeLogMailTrace_CheckedChanged(object sender, EventArgs e)
        {
            labelOfficeLogsRecipient.Visible = radioButtonOfficeLogMailTrace.Checked;
            labelOfficeLogsSender.Visible = radioButtonOfficeLogMailTrace.Checked;
            textBoxOfficeLogsRecipient.Visible = radioButtonOfficeLogMailTrace.Checked;
            textBoxOfficeLogsSender.Visible = radioButtonOfficeLogMailTrace.Checked;
            labelOfficeLogsFromIP.Visible = radioButtonOfficeLogMailTrace.Checked;
            textBoxOfficeLogsIP.Visible = radioButtonOfficeLogMailTrace.Checked;
        }

        private void radioButtonOfficeLogsForwarding_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
