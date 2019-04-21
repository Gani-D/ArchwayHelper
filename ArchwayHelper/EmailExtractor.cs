using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchwayHelper
{
    class EmailExtractor
    {
        public string Extract(bool emails, string text, bool removeDups)
        {
            if (emails)
            {
                return (ExtractEmails(text, removeDups));
            }
            else return ExtractDomains(text, removeDups);
        }

        private string ExtractDomains (string text, bool removeDups)
        {
            StringBuilder retText = new StringBuilder();
            StringBuilder temp = new StringBuilder();
            bool gotAt = false;
            foreach (char c in text)
            {
                if (gotAt)
                {
                    if (IsDomainSymbols(c)) temp.Append(c);
                    else
                    {
                        temp = NormalizeDomain(temp);
                        if (temp.Length > 3) { retText.AppendLine(temp.ToString()); }
                        gotAt = false;
                        temp.Clear();
                    }
                }
                if (c == '@') gotAt = true;
            }
                temp = NormalizeDomain(temp);
                if (temp.Length > 3&&gotAt) { retText.AppendLine(temp.ToString()); }
            if (removeDups==true)
            {
               
               var res = retText.ToString().Split('\n').Distinct();
               retText.Clear();
               foreach(string str in res)
               {
                    if (str.Length > 3)
                    {
                        retText.Append(str);
                        retText.Append('\n');
                    }
               }
                return retText.ToString();
            }
            return retText.ToString();

        }
        private string ExtractEmails(string text, bool removeDups)
        {
            
            StringBuilder retText = new StringBuilder();
            StringBuilder temp = new StringBuilder();
            int gotAt = 0;
            foreach (char c in text)
            {
                if (IsEmailSymbols(c))
                {
                    if (c=='@')
                    {
                        gotAt++;
                    }
                    temp.Append(c);
                }
                else
                {
                    if (temp.Length < 6 || gotAt > 1 || gotAt == 0)
                    {
                        temp.Clear();
                    }
                    else
                    {
                        temp = NormalizeDomain(temp);
                        if (temp.Length > 3) { retText.AppendLine(temp.ToString()); }

                        temp.Clear();
                    }
                    gotAt = 0;
                }
                
              //  if (c == '@') gotAt = true;
            }
            if (removeDups == true)
            {
                var res = retText.ToString().Split('\n').Distinct();
                retText.Clear();
                foreach (string str in res)
                {
                    if (str.Length > 3)
                    {
                        retText.Append(str);
                        retText.Append('\n');
                    }
                }
                return retText.ToString();
            }
            return retText.ToString();

        }
        private bool IsDomainSymbols (char c)
        {
            if ((c == 46) || (c > 47 && c < 58) || (c > 64 && c < 91) || (c > 96 && c < 123)) return true;
            else return false;
        }
        private bool IsEmailSymbols(char c)
        {
            if ((c == 46) || (c > 47 && c < 58) || (c > 63 && c < 91) || (c > 96 && c < 123)) return true;
            else return false;
        }
        

        private StringBuilder NormalizeDomain(StringBuilder domainText)
        {
            if (domainText.Length < 3) return new StringBuilder();
            if (domainText[domainText.Length-1]=='.')
            {
                do
                {
                    domainText.Remove(domainText.Length - 1, 1);
                    
                }
                while (domainText[domainText.Length - 1] == '.'&&domainText.Length>0);

            }
            if (domainText.Length < 3) return new StringBuilder();
            bool hasDot = false;
            for (int i=0; i<domainText.Length-1; i++)
            {
                if (domainText[i] == '.')
                {
                    hasDot = true;
                    if (domainText[i + 1] == '.') return new StringBuilder();
                }
                if (hasDot) return domainText;
            }
            return new StringBuilder();
        }
        private StringBuilder NormalizeEmail (StringBuilder emailText)
        {
            if (emailText[emailText.Length - 1] == '.')
            {
                do
                {
                    emailText.Remove(emailText.Length - 1, 1);

                }
                while (emailText[emailText.Length - 1] == '.' && emailText.Length > 5);

            }
            if (emailText.Length < 6) return new StringBuilder();

            bool hasDot = false;
            for (int i = 0; i < emailText.Length - 1; i++)
            {
                if (emailText[i] == '.')
                {
                    hasDot = true;
                    if (emailText[i + 1] == '.') return new StringBuilder();
                }
                if (hasDot) return emailText;
            }
            return new StringBuilder();
            
        }

        

    }
}
