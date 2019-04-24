using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    public class MessageTrace 
    {
        public string Time { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string FromIP { get; set; }



        public MessageTrace(string time,  string sender, string recipient, string subject, string status, string ip)
        {
            Sender = sender;
            Recipient = recipient;
            Status = status;
            Time = time;
            FromIP = ip;
            Subject = subject;
        }
        public MessageTrace(string time)
        {
            Time = time;
        }
        

    }
}
