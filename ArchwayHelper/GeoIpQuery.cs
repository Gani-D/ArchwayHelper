
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace ArchwayHelper
{
    public sealed class GeoIpQuery
    {
        private GeoIpQuery ()
        {
            
        }
        private static GeoIpQuery instance = null;
        private static readonly object padlock = new object();
        public static GeoIpQuery Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GeoIpQuery();
                    }
                    return instance;
                }
            }
        }

        static string queryResult = "";
        public async Task<string> LoadIpInformation(string ip)
        {
            if (!IsIpValid(ip)) return queryResult;
            string url = "http://ip-api.com/json/"+ip;
            queryResult = "";

            using (HttpResponseMessage response = await ApiConnector.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    GeoIpModel result = await response.Content.ReadAsAsync<GeoIpModel>();
                    
                        queryResult = 
                        $"IP address: {result.Query}  \n" +
                        $"Name1: {result.As} \n" +
                        $"Name2: {result.Org} \n" +
                        $"Name3: {result.Isp} \n" +
                        $"Country: {result.Country} \n" +
                        $"Region Name: {result.RegionName} \n" +
                        $"City: {result.City} \n";
                 }
                else
                {
                    queryResult = "Cannot connect to the API server";
                }
                return queryResult;
            }
        }

        public bool IsIpValid (string ip)
        {
            int[] ipNumbers = new int[4];
            int index = 0;
            StringBuilder tempIp = new StringBuilder();
            foreach(char character in ip)
            {
                if (character-'0'>=0&&character-'0'<10) // it should contain only digits
                {
                    tempIp.Append(character);
                }
                else if (character == '.')
                {
                    if (index>3 || tempIp.Length > 3) // if IP address had more than 3 dots or the num has more than 3 digits, i.e. 1234.x.x.x
                    {
                        return WrongIpAddress(0);
                    }
                    ipNumbers[index++] = int.Parse(tempIp.ToString());
                    tempIp.Clear();
                }
                else
                {
                   return WrongIpAddress(1);
                }
                
            }
            if (tempIp.Length<1)
            {
                return WrongIpAddress(0);
            }
            else
            {
                ipNumbers[index++] = int.Parse(tempIp.ToString());
            }

            for (int i = 0; i < 4; i++) // IP shouldn't be more than 2^8
            {
                if (ipNumbers[i]>255)
                {
                  return WrongIpAddress(0);
                }
                
            }
            
            if (ipNumbers[0] == 10 || 
                (ipNumbers[0] == 172 && (ipNumbers[1]>15 && ipNumbers[1] < 32 )) || 
                (ipNumbers[0]==192 && ipNumbers[1] == 168) 
                )
            {
                return WrongIpAddress(2);
            }

            return true;
        }


        private bool WrongIpAddress(int reason)
        {
            switch (reason)
            {
                case 0: queryResult = "Wrong IP address"; break;
                case 1: queryResult = "The IP contains wrong symbols"; break;
                case 2: queryResult = "The IP address is in private range"; break;
            }
            return false;
        }

    }
}
