using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    class Whois
    {
        /// <summary>
        /// Gets the Whois server name
        /// </summary>
        /// <param name="domain">The domain name</param>
        /// <returns>Returns the whois server name</returns>
        private string HelperServerName(string domain)
        {
            if (domain.Length < 4) return null;
            string domainTopDomain = domain.Substring(domain.Length - 3);
            if (domainTopDomain == "com" || domainTopDomain == "net" || domainTopDomain == "edu")
            {
                return "whois.verisign-grs.com";
            }
           
            if (domainTopDomain == "org" || domainTopDomain == "ngo") return "whois.publicinterestregistry.net";
            return null;
        }

        /// <summary>
        /// Using Whois servers get the information about the domain
        /// </summary>
        /// <param name="domainName">The domain name</param>
        /// <returns>WHOIS info about the domain</returns>
        public string GetWhoisData(string domainName)
        {
            domainName = domainName.Trim();
            string whoisServer = HelperServerName(domainName);
            if (whoisServer == null) return "Cannot resolve the domain";
            StringBuilder result = new StringBuilder();
            
            using (TcpClient tcpClient = new TcpClient())

            {
                //opening a connection to WHOIS server
                tcpClient.Connect(whoisServer.Trim(), 43);
                byte[] domainQueryBytes = Encoding.ASCII.GetBytes(domainName + "\n");
                using (Stream stream = tcpClient.GetStream())
                {
                    //sending request to WHOIS server
                    stream.Write(domainQueryBytes, 0, domainQueryBytes.Length);
                    
                    using (StreamReader sr = new StreamReader(tcpClient.GetStream(), Encoding.UTF8))
                    {
                        string row;
                        while ((row = sr.ReadLine()) != null)
                            result.AppendLine(row);
                    }
                }
            }

            return result.ToString();
        }
    }
}
