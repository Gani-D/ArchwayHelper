using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchwayHelper
{
    public sealed class OfficePS
    {
        private OfficePS()
        {

        }

        #region Variables (Return texts, CredsAreValid, Rules, command, powershell, result, runspace)
        private const string MESSAGE_NOT_VALID_CREDS = "Please type credentials to enable functionality";
        private const string MESSAGE_ACTION_NOT_SELECTED = "Please select the action you want to perform";
        public bool CredsAreValid { get; set; }
        public ArrayList Rules { get; set; } = new ArrayList();

        PSCommand command = new PSCommand();
        PowerShell powershell = PowerShell.Create();
        Collection<PSObject> result;
        Runspace runspace;
        #endregion

        #region Singleton
        private static OfficePS officePsInstance = null;
        private static readonly object padlock = new object();
        public static OfficePS Instance  //Singleton
        {
            get
            {
                lock (padlock)
                {
                    if (officePsInstance == null)
                    {
                        officePsInstance = new OfficePS();
                    }
                    return officePsInstance;
                }
            }
        }
        #endregion

        /// <summary>
        /// Executes the PS command
        /// </summary>
        /// <param name="script">PS script to execute</param>
        /// <returns></returns>
        private Collection<PSObject> RunCommand(string script)
        {
            powershell = PowerShell.Create();
            command = new PSCommand();
            command.AddCommand("Invoke-Command");
            command.AddParameter("ScriptBlock", ScriptBlock.Create(script));
            command.AddParameter("Session", result[0]);
            powershell.Commands = command; powershell.Runspace = runspace;
            return powershell.Invoke();
        }

        /// <summary>
        /// Choses the necessary method to run and returns result as string
        /// </summary>
        public string SelectPermissionAction(string owner, string delegatePerson, bool addPermissions, 
                                            bool fullAccess, bool autoMap, bool  sendAs, bool calendar, bool contact, string accessLvl)
        {
            if (owner.Length<3 || delegatePerson.Length<3)
            {
                return "Please select the owner of the mailbox and delegate person";
            }
            if (!(fullAccess || sendAs || calendar || contact))
            {
                return MESSAGE_ACTION_NOT_SELECTED;
            }
            
            if (fullAccess)
            {
                return Set_MailboxFullAccessPermissions(owner, delegatePerson, autoMap, addPermissions);
            }
            if (sendAs)
            {
                return Set_SendAsPermissions(owner, delegatePerson, addPermissions);
            }
            if (calendar || contact)
            {
                if (addPermissions)
                {
                    if (accessLvl.Length == 0)
                    {
                        return "Please select the access level";
                    }
                }
                
                    return Set_ContactAndCalendarPermissions(owner, delegatePerson, accessLvl, contact, addPermissions);
             }
            return MESSAGE_ACTION_NOT_SELECTED;
        }
        /// <summary>
        /// Adds the user to group or configures a forwarding
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="delegatePerson"></param>
        /// <param name="group"></param>
        /// <param name="keepCopy"></param>
        /// <param name="addForwarding"></param>
        /// <param name="removeForwarding"></param>
        /// <param name="distGroup"></param>
        /// <param name="addToGroup"></param>
        /// <returns></returns>
        public string ForwardingAddingToGroupAction (string owner, string delegatePerson, string group, bool keepCopy, bool addForwarding, bool removeForwarding, bool distGroup, bool addToGroup)
        {
            if (!CredsAreValid)
            {
                return MESSAGE_NOT_VALID_CREDS;
            }
            if (!(addForwarding || removeForwarding || distGroup))
            {
                return MESSAGE_ACTION_NOT_SELECTED;
            }
            if (owner.Length < 3)
            {
                return "Please select the mailbox you want to configure forwarding";
            }
            if (addForwarding || removeForwarding)
            {
                if (addForwarding && delegatePerson.Length < 3)
                {
                    return "Please select the delegate person";
                }
                return Set_Forwarding(owner, delegatePerson, keepCopy, addForwarding);

            }
            if (distGroup)
            {
                if (group.Length < 1)
                {
                    return "Please select the group";
                }
                if (owner.Length < 3)
                {
                    return "Please select the user";
                }
                return EditDistributionGroup(owner, group, addToGroup);
            }
            return "Not expected case";
        }
        
        public bool StartCode(string login, string password) /// Check if the credentials are valid
        {

            string connectionUri = "https://outlook.office365.com/powershell-liveid/";
           // string connectionUri = "https://ps.outlook.com/powershell/";
            SecureString secpassword = new SecureString();
            foreach (char c in password) { secpassword.AppendChar(c); }
            PSCredential credential = new PSCredential(login, secpassword);

            runspace = RunspaceFactory.CreateRunspace(); 
            command = new PSCommand(); command.AddCommand("New-PSSession");
            command.AddParameter("ConfigurationName", "Microsoft.Exchange");
            command.AddParameter("ConnectionUri", new Uri(connectionUri));
            command.AddParameter("Credential", credential);
            command.AddParameter("Authentication", "Basic");
            powershell.Commands = command; runspace.Open();
            powershell.Runspace = runspace; result = powershell.Invoke();
            if (powershell.Streams.Error.Count > 0 || result.Count != 1) { return false; }
            CredsAreValid = true;
            return true;
            

        }

        public void DisposeSession()
        {

            CredsAreValid = false;
            if (runspace != null)
            {
               runspace.Dispose();
            }
        }

        public void Get_Mailboxes()
        {
            var mailBoxes = RunCommand("Get-Mailbox");
            if (powershell.HadErrors)
            {
                ListOfMailboxes = null;
                return;
            }
            ArrayList sb = new ArrayList();
            foreach (PSObject pr in mailBoxes)
            {
                if (!pr.Properties["Alias"].Value.ToString().StartsWith("DiscoverySearchMailbox"))
                {
                    sb.Add(pr.Properties["PrimarySmtpAddress"].Value.ToString());
                   
                }
            }
            ListOfMailboxes =  (string[])sb.ToArray(typeof(string));
        }
        /// <summary>
        /// Searches for a mailbox alias
        /// </summary>
        /// <param name="alias">The alias you want to find</param>
        /// <returns></returns>
        public string AliasOwner (string alias)
        {
            var mailBoxes = RunCommand("Get-Recipient");
            string res = "Not found";
            foreach (PSObject temp in mailBoxes)
            {
                if (temp.Properties["emailaddresses"].Value.ToString().Contains(alias))
                {
                    res = temp.Properties["DisplayName"].Value.ToString() + '\n' + temp.Properties["PrimarySmtpAddress"].Value + '\n'.ToString() + temp.Properties["emailaddresses"].Value.ToString();
                }
            }
        return res;
        }

        public string[] ListOfMailboxes { get; set; }
        
        /// <summary>
        /// Check if the user already have access to the resource
        /// </summary>
        /// <param name="isContact">Is it a contact (true) or a calendar (false)</param>
        /// <returns></returns>
        public bool  HasAccess (string identity, string delegatePerson, bool isContact)
        {
            
            string res = "Calendar";
            if (isContact)
            {
                res = "Contact";
            }
            string script = "Get-MailboxFolderPermission -Identity " + identity + res + " -User " + delegatePerson;
            var mailBoxes = RunCommand(script);

            if (mailBoxes.Count < 1) return false;
            return true;
        }

        
        
        private string Set_ContactAndCalendarPermissions (string identity, string delegatePerson, string accessLevel, bool isContact, bool addPermissions)
        {
            string resourceType = "Calendar ";
            string addRemovePermissions = "Add";
            string accessRights = " -AccessRight " + accessLevel;
            if (isContact)
            {
                resourceType = "Contacts ";
            }
            
            if (!addPermissions)
            {
                addRemovePermissions = "Remove";
                accessRights = " -Confirm:$false ";
            }
            else if (HasAccess(identity, delegatePerson, isContact))
            {
                addRemovePermissions = "Set";
            }

            string script = addRemovePermissions + "-MailboxFolderPermission -Identity " + identity + ":\\" + resourceType + " -User "+ delegatePerson + accessRights;
            MessageBox.Show(script);
            var mailBoxes = RunCommand(script);
            if (powershell.HadErrors)
            {
                return powershell.Streams.Error.ElementAt(0).ToString();
            }
            if (!addPermissions)
            {
                return "The permission has been removed";
            }

            if (mailBoxes.Count == 0) return "Error";
            return mailBoxes[0].Properties["User"].Value.ToString()+'\n'+ mailBoxes[0].Properties["FolderName"].Value.ToString() + '\n' + mailBoxes[0].Properties["AccessRights"].Value.ToString(); // CHANGE THIS CODE
        }


        private string Set_MailboxFullAccessPermissions(string identity, string delegatePerson, bool autoMap, bool addPermissions)
        {
            string commandType = "Add";
            string commandEnd = "-AutoMapping:$" + autoMap.ToString();
            if (!addPermissions)
            {
                commandType = "Remove";
                commandEnd = "-Confirm:$false";
            }
            string script = commandType+ "-MailboxPermission -Identity " + identity + " -User " + delegatePerson + " -AccessRights FullAccess " + commandEnd;

            var mailBoxes = RunCommand(script);
             if (powershell.HadErrors)
                {
                    return powershell.Streams.Error.ElementAt(0).Exception.ToString() + '\n';
                }
            if (addPermissions)
            {
                return "Owner:" + mailBoxes[0].Properties["Identity"].Value.ToString() + '\n' + "Delegate:" + mailBoxes[0].Properties["User"].Value.ToString() + '\n' + "Full Access permission has been granted";// CHANGE THIS CODE
            }
            else
            {
                return "The full access permissions were removed";
            }
            
        }

        private string Set_SendAsPermissions(string owner, string delegatePerson, bool addPermissions)
        {
            string commandType = "Add";
            if (!addPermissions)
            {
                commandType = "Remove";
            }
            string script = commandType + "-RecipientPermission -Identity " + owner + " -Trustee " + delegatePerson + " -AccessRights SendAs -Confirm:$false";
            
            RunCommand(script);
            if (powershell.HadErrors)
            {
                return powershell.Streams.Error.ElementAt(0).ToString();
            }
            if (addPermissions)
            {
                return "The permission has been granted";
            }
            else
            {
                return "The permission has been removed";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="delegatePerson"></param>
        /// <param name="keepCopy"></param>
        /// <param name="addForwarding"></param>
        /// <returns></returns>
        public string Set_Forwarding(string owner, string delegatePerson, bool keepCopy, bool addForwarding)
        {
            
            if (owner.Length < 3 || delegatePerson.Length < 3)
            {
                return "Please select the Owner and Delegate persons";
            }
            //string forwardingPerson = delegatePerson;
            string deliverToMailbox = " -DeliverToMailboxAndForward $" + keepCopy.ToString();
            if (!addForwarding)
            {
                delegatePerson = "$Null";
                deliverToMailbox = "";
            }
            string script = "Set-Mailbox -Identity " + owner + deliverToMailbox + " -ForwardingSMTPAddress " + delegatePerson;
            RunCommand(script);
            if (powershell.HadErrors)
            {
                return powershell.Streams.Error.ElementAt(0).ToString();
            }
            if (addForwarding)
            {
                return "The forwarding has been configured";
            }
            else
            {
                return "The forwarding has been cancelled";
            }
        }

        public string EditDistributionGroup(string owner, string groupName, bool addMember)
        {
            string commandType = "Add";
            string confirm = "";
            if (!addMember)
            {
                commandType = "Remove";
                confirm = " -Confirm:$false";
            }
            string script = commandType + "-DistributionGroupMember -Identity \"" + groupName + "\" -Member " + owner + confirm;
            RunCommand(script);
            var mailBoxes = powershell.Invoke();
            if (powershell.HadErrors)
            {
                return powershell.Streams.Error.ElementAt(0).ToString();
            }
            if (addMember)
            {
                return "The user has been added to the group";
            }
            else
            {
                return "The user has been removed from the group";
            }
        }
        /// <summary>
        /// Gets all Distribution Groups and returns a list of their smtp addresses
        /// </summary>
        /// <returns>Array of email addresses</returns>
        public string [] GetDistributionGroups ()
        {
            var mailBoxes = RunCommand("Get-DistributionGroup");
            ArrayList dgList = new ArrayList();

            foreach (PSObject pr in mailBoxes)
            {
                 dgList.Add(pr.Properties["PrimarySmtpAddress"].Value.ToString());
            }
            return  (string[])dgList.ToArray(typeof(string));
        }
        /// <summary>
        /// Gets a list of users who has full access to the mailbox
        /// </summary>
        /// <param name="email">The email address of the mailbox</param>
        /// <returns>List of Users with Access rights</returns>
        public string[] Get_MailboxFullAccessPermissionsInfo(string email)
        {
            var mailBoxes = RunCommand("Get-MailboxPermission -Identity " + email);
            ArrayList sb = new ArrayList();
            foreach (PSObject pr in mailBoxes)
            {
                
                    if (pr.Properties["User"].Value.ToString().Contains('@'))
                    {
                        sb.Add(pr.Properties["User"].Value.ToString());
                        sb.Add(pr.Properties["AccessRights"].Value.ToString());
                    }   
            }

            return (string[])sb.ToArray(typeof(string));
        }

        public string[] Add_MailboxFullAccessPermissions(string owner, string delegatePerson, bool autoMappingEnabled, bool addPermissions)
        {
            string commandType = "Add";
            string commandEnd = " -AutoMapping:$" + autoMappingEnabled.ToString();
            
            if (!addPermissions)
            {
                commandType = "Remove";
                commandEnd = " -Confirm:$false";
            }
            string script = commandType + "-MailboxPermission -Identity " + owner + " -User " + delegatePerson + commandEnd;
            var mailBoxes = RunCommand(script);
            
            if (powershell.HadErrors)
            {
                return new string[] { powershell.Streams.Error.ElementAt(0).ToString()};
            }
            if (!addPermissions)
            {
                return new string[] {"The permission has been revoked" };
            }
            ArrayList sb = new ArrayList();
            foreach (PSObject pr in mailBoxes)
            {

                if (pr.Properties["User"].Value.ToString().Contains('@'))
                {
                    sb.Add(pr.Properties["User"].Value.ToString());
                    sb.Add(pr.Properties["AccessRights"].Value.ToString());
                }
            }

            return (string[])sb.ToArray(typeof(string));
        }
        
        public string GetInboxRules(string owner)
        {
            Rules.Clear();
            if (!CredsAreValid)
            {
                return MESSAGE_NOT_VALID_CREDS;
            }
            if (owner.Length < 4)
            {
                return "Plese select the user";
            }
            var rules = RunCommand("Get-InboxRule -Mailbox " + owner);
            if (powershell.HadErrors)
            {
                return powershell.Streams.Error.ElementAt(0).ToString();
            }
            
            string res = "";

            foreach (PSObject pr in rules)
            {
                Rules.Add(pr.Properties["Name"].Value.ToString());
                res += pr.Properties["Name"].Value.ToString() + '\n';
                res += pr.Properties["Description"].Value.ToString() + '\n';
            }
            if (res.Length < 3)
            {
                res = "No Inbox rules" + '\n';
            }
          //  res += "Logs for the mailbox:" + '\n' + GetLogInData(owner);
           
            return res;
        }

        public List<InboxRulesCount> GetInboxRulesCount()
        {
            if (!CredsAreValid)
            {
                return new List<InboxRulesCount> { new InboxRulesCount(MESSAGE_NOT_VALID_CREDS, 0) };
            }
            List<InboxRulesCount> result = new List<InboxRulesCount>();
            foreach(string mail in ListOfMailboxes)
            {
                GetInboxRules(mail);
                if (powershell.HadErrors)
                {
                    return new List<InboxRulesCount> { new InboxRulesCount(powershell.Streams.Error.ElementAt(0).ToString(), 0) };
                }
                result.Add(new InboxRulesCount(mail, Rules.Count));
            }
            return result;
        }

        public string RemoveInboxRule(string owner, string ruleName)
        {
            if (!CredsAreValid)
            {
                return MESSAGE_NOT_VALID_CREDS;
            }
            if (owner.Length < 3)
            {
                return "Please select the user";
            }
            
            string script = $"Remove-InboxRule -Mailbox {owner} -Identity \"{ruleName}\" -Confirm:$false";
            RunCommand(script);
            if (powershell.HadErrors)
            {
                return powershell.Streams.Error.ElementAt(0).ToString();
            }
            else
            {
                return "Done";
            }
        }

        public  List<AuditLogs> GetLogInData (string owner)
        {
            if (!CredsAreValid)
            {
                return new List<AuditLogs> { new AuditLogs(MESSAGE_NOT_VALID_CREDS)};
            }
            if (owner.Length < 3 )
            {
                return new List<AuditLogs> { new AuditLogs("Please select the user") };
            }
            var mailBoxes = RunCommand("Search-UnifiedAuditLog -StartDate " + DateTime.Today.AddDays(-30).ToShortDateString().ToString() + " -EndDate " + DateTime.Today.AddDays(1).ToShortDateString().ToString() + " -UserIds " + owner);
            if (powershell.HadErrors)
            {
                return new List<AuditLogs> {new AuditLogs( powershell.Streams.Error.ElementAt(0).ToString()) };
            }
            List<AuditLogs> res = new  List<AuditLogs>();
            foreach (PSObject pr in mailBoxes)
            {
               // res += (pr.Properties["Operations"].Value.ToString() + '\n' + pr.Properties["CreationDate"].Value.ToString() + '\n' + pr.Properties["AuditData"].Value.ToString() + '\n');
                res.Add(new AuditLogs(pr.Properties["CreationDate"].Value.ToString(), pr.Properties["Operations"].Value.ToString(), pr.Properties["AuditData"].Value.ToString()));
            }
            return res;

        }

        private string GetForwardingAddress(string owner)
        {
            var mailBoxes = RunCommand("Get-Mailbox -Identity " + owner);
            if (powershell.HadErrors)
            {
                return powershell.Streams.Error.ElementAt(0).ToString();
            }
            string res = "";
            foreach (PSObject pr in mailBoxes)
            {
                res += (pr.Properties["Operations"].Value.ToString() + '\n' + pr.Properties["CreationDate"].Value.ToString() + '\n' + pr.Properties["AuditData"].Value.ToString() + '\n');
            }
            return res;
        }

        public string SetOutOfOffice (string owner, bool disableOOO, bool enableOOO, bool scheduleOOO, string startTime, string endTime, string message)
        {
            if (message.Contains('\"'))
            {
                return "The message contains a quote";
            }
            string autoReplyState = "Enabled";
            message = " -InternalMessage \"" + message + "\" -ExternalMessage \"" + message + "\"";
            if (!CredsAreValid)
            {
                return MESSAGE_NOT_VALID_CREDS;
            }
            
            if (owner.Length < 3)
            {
                return "Please select the user";
            }
            if (!(disableOOO || enableOOO || scheduleOOO))
            {
                return MESSAGE_ACTION_NOT_SELECTED;
            }
            if (disableOOO)
            {
                autoReplyState = "Disabled";
                message = "";
            }
            if (scheduleOOO)
            {
                if (DateTime.Compare(Convert.ToDateTime(startTime), Convert.ToDateTime(endTime)) >= 0)
                {
                    return "End time cannot be earlier than a Start time";
                }
                autoReplyState = "Scheduled -StartTime \"" + startTime + "\" -EndTime \"" + endTime + "\"";

            }
            string script = "Set-MailboxAutoReplyConfiguration -Identity " + owner + " -AutoReplyState " + autoReplyState + message;
            RunCommand(script);
            if (powershell.HadErrors)
            {
                return powershell.Streams.Error.ElementAt(0).ToString();
            }

            return "Done";
        }

        public List<MessageTrace> GetLogs (string sender, string recipient, string fromIp)
        {
            if (!CredsAreValid)
            {
                return new List<MessageTrace> { new MessageTrace(MESSAGE_NOT_VALID_CREDS)};
            }
            if ((sender+recipient+fromIp).Contains(' ')|| (sender + recipient + fromIp).Contains('"'))
            {
                return new List<MessageTrace> { new MessageTrace("Sender, Recipient or IP contain not valid characters") };
            }
          
            if ((sender + recipient + fromIp).Length < 5)
            {
                    return new List<MessageTrace> { new MessageTrace("Please type the IP OR sender OR recipient address ") };
            }

            string senderEmail = "";
            string recipientEmail = "";
            string ipAddress = "";
            if (sender.Length > 1)
            {
                senderEmail = "-SenderAddress " + sender;
            }
            if (recipient.Length > 1)
            {
                recipientEmail = "-RecipientAddress " + recipient;
            }
            if (fromIp.Length > 1)
            {
               ipAddress = "-FromIP \"" + fromIp + "\"";
            }
            string script = $"Get-MessageTrace {senderEmail} {recipientEmail} {ipAddress} -StartDate \"{DateTime.Now.AddDays(-7).ToShortDateString()}\" -EndDate \"{DateTime.Now.ToShortDateString()}\" ";
            
            var mailLogs = RunCommand(script);
            List<MessageTrace> email = new List<MessageTrace>();
            if (powershell.HadErrors)
            {
                return new List<MessageTrace> { new MessageTrace(powershell.Streams.Error.ElementAt(0).ToString()) };
            }
            foreach (PSObject pr in mailLogs)
            {
                MessageTrace temp = new MessageTrace(pr.Properties["Received"].Value.ToString(), pr.Properties["SenderAddress"].Value.ToString(),
                    pr.Properties["RecipientAddress"].Value.ToString(), pr.Properties["Subject"].Value.ToString(), pr.Properties["Status"].Value.ToString(), pr.Properties["FromIP"].Value.ToString());
                email.Add(temp);
            }
          
            
            return  email;
        }

        public List<MailboxForward> GetMailboxesWithForwarding ()
        {
            if (!CredsAreValid)
            {
                return new List<MailboxForward> { new MailboxForward(MESSAGE_NOT_VALID_CREDS) };
            }
            var mailBoxes = RunCommand("Get-Mailbox");
            if (powershell.HadErrors)
            {
                return new List<MailboxForward> { new MailboxForward(powershell.Streams.Error.ElementAt(0).ToString()) };
            }
            List<MailboxForward> result = new List<MailboxForward>();
            foreach (PSObject pr in mailBoxes)
            {
                try
                {
                    if (pr.Properties["ForwardingSMTPAddress"].Value.ToString().Length > 2)
                    {
                        result.Add(new MailboxForward(pr.Properties["UserPrincipalName"].Value.ToString(), pr.Properties["PrimarySmtpAddress"].Value.ToString(),
                            pr.Properties["ForwardingSmtpAddress"].Value.ToString(), pr.Properties["DeliverToMailboxAndForward"].Value.ToString()));
                    }
                }
                catch
                {

                }
            }
            if (result.Count == 0)
            {
                return new List<MailboxForward> { new MailboxForward("No mailboxes with forwardings") };
            }
            return result;
        }
        
       
    }
}
