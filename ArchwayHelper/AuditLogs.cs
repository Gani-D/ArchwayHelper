using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    public class AuditLogs
    {
        public string Time { get; set; }
        public string Operation { get; set; }
        public string AuditData { get; set; }

        public AuditLogs(string time, string operation, string auditData)
        {
            Time = time;
            Operation = operation;
            AuditData = auditData;
        }
        public AuditLogs(string time)
        {
            Time = time;
        }
    }
}
