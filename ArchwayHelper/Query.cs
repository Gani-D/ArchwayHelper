using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Text;


namespace ArchwayHelper
{
    class Query
    {

        private const string NSLOOKUP = "nslookup";
        /// <summary>
        /// Gets string and pastes them to cmd.exe
        /// </summary>
        /// <param name="queries"></param>
        /// <returns>Result of the commands sent to cmd.exe</returns>
        private String[] Cmd(String[] queries)  //gets command to run in CMD session
        {
            List<String> output = new List<string>();
            Process cmdMX = new Process();
            cmdMX.StartInfo.FileName = "cmd.exe";
            cmdMX.StartInfo.RedirectStandardInput = true;
            cmdMX.StartInfo.RedirectStandardOutput = true;
            cmdMX.StartInfo.CreateNoWindow = true;
            cmdMX.StartInfo.UseShellExecute = false;
            cmdMX.Start();


            foreach (String query in queries)
            {
                cmdMX.StandardInput.WriteLine(query);
            }

            cmdMX.StandardInput.Flush();
            cmdMX.StandardInput.Close();
            cmdMX.WaitForExit();
            for (int i = 0; i < 9; i++)
                cmdMX.StandardOutput.ReadLine();
            while (cmdMX.StandardOutput.Peek() >= 0)
                output.Add(cmdMX.StandardOutput.ReadLine());
            return output.ToArray();
        }
        /// <summary>
        /// Gets a domain name to check its MX, autodiscover and TXT records
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns>A formatted text from cmd.exe</returns>
        public string GetMXQuery (string domainName)
        { 
            StringBuilder result = new StringBuilder();
            if (domainName.Contains("@"))
            {
                domainName = domainName.Split('@')[1];
            } //removes the username part
            
            result.Append( "***Autodiscover is: ***\n");
            String[] temp = Cmd(new string[] { NSLOOKUP, "autodiscover." + domainName });
            for (int i = 0; i < temp.Length - 2; i++)
            {
                result.Append( temp[i] + "\n");
            }
            result.Append("***MX records are: ***\n");

            temp = Cmd(new string[] { NSLOOKUP, "set q=mx", domainName });
            for (int i = 0; i < temp.Length - 1; i++)
            {
                result.Append( temp[i] + "\n");
            }

            result.Append( "***TXT records are: ***\n");
            temp = Cmd(new string[] { NSLOOKUP, "set q=txt", domainName });
            for (int i = 0; i < temp.Length - 2; i++)
            {
                result.Append( temp[i] + "\n");
            }
            return result.ToString();
        }
    }
}
