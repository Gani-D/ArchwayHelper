using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    class TimeZones
    {
        public static string PSTTime()
        {
            TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return "PST " + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone).ToShortTimeString().ToString();
        }

        public static string CSTTime()
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            return "CST " + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone).ToShortTimeString().ToString();
        }

        public static string ESTTime()
        {
            TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return "EST " + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estZone).ToShortTimeString().ToString();
        }
    }
}
