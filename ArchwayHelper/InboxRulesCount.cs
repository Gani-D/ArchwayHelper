using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    public class InboxRulesCount
    {
        public string  Email { get; set; }
        public int RulesCount { get; set; }
        public InboxRulesCount(string email, int rulesCount)
        {
            Email = email;
            RulesCount = rulesCount;
        }
       
    }
}
