using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    public class GeoIpModel
    {
        public string Country { get; set; }
        public string Query { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public string TimeZone { get; set; }
        public string Isp { get; set; }
        public string Org { get; set; }
        public string As { get; set; }
        public string Message { get; set; }

        /* 
         "query": "24.48.0.1",
  "status": "success",
  "country": "Canada",
  "countryCode": "CA",
  "region": "QC",
  "regionName": "Quebec",
  "city": "Montreal",
  "zip": "H1S",
  "lat": 45.5808,
  "lon": -73.5825,
  "timezone": "America/Toronto",
  "isp": "Le Groupe Videotron Ltee",
  "org": "Videotron Ltee",
  "as": "AS5769 Videotron Telecom Ltee"
         */
    }
}
