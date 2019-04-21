using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    class SmtpSender
    {
        /// <summary>
        /// The method to send emails
        /// </summary>
        /// <param name="server">Mail relay server</param>
        /// <param name="port">Port number</param>
        /// <param name="from">The sender's email address</param>
        /// <param name="recipient">The recipient's email address</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="mailText">The text in the message</param>
        /// <param name="auth">Does mail relay require authentification</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public string SendEmail(string server, string port, string from, string recipient, string subject, string mailText, bool auth, string username, string password)
        {
            if (server.Length < 3 || port.Length < 1 || from.Length < 4 || recipient.Length < 4) return "Please fill all required fields";
            
            SmtpClient client = new SmtpClient();
            client.Port = Convert.ToInt32(port);
            client.Host = server;
            client.EnableSsl = auth;
            client.Timeout = 4000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (auth)
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(username,password);

            }
            else
            {
                client.UseDefaultCredentials = true;
            }
            MailMessage message;
            try
            {
                message = new MailMessage(from, recipient, subject, mailText);
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            }
            catch { return "Failed to deliver the email, please check if the settings are correct"; }

            try
            {
                client.Send(message);
                return "Email has been sent";
                
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        return "Mailbox is unavailable";
                    }
                }
            }
            catch (Exception ex)
            {
               return (ex.ToString()).Split('\n')[0].Substring(30).Trim();
            }
            return "Failed to deliver the email";
        }
    }
}
