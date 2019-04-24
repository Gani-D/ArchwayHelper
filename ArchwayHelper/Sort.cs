using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    class Sort
    {
        public string SortText (string text)
        {
            string[] tempText = text.Split('\n');

            Array.Sort(tempText);
            return string.Join("\n",tempText);
        }
    }
}
