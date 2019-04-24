using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
     public class MailboxForward
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ForwardTo { get; set; }
        public string KeepCopy { get; set; }

        public MailboxForward(string name, string email, string forwardto, string keepCopy)
        {
            Name = name;
            Email = email;
            ForwardTo = forwardto;
            KeepCopy = keepCopy;
        }
        public MailboxForward(string name)
        {
            Name = name;
        }
    }
    
}
