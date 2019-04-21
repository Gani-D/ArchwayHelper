using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchwayHelper
{
    class PasswordGenerator
    {
        private Random random = new Random();
        /// <summary>
        /// Gets the password requirements to generate a random password
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="upperCase">Is uppercase letters are allowed</param>
        /// <param name="lowerCase">Is lowercase letters are allowed</param>
        /// <param name="inclNumbers">Include numbers</param>
        /// <param name="inclSymbols">Include symbols</param>
        /// <param name="exclCharsDot">Exclude ambigious symbols</param>
        /// <param name="exclSimilarChars">Exclude ambigious letters</param>
        /// <param name="startWithUpper">Start with upper case</param>
        /// <param name="length">Password length</param>
        /// <returns>The list of passwords with specified complexity</returns>
        public string PassGen(string quantity, bool upperCase, bool lowerCase, bool inclNumbers, bool inclSymbols,   
            bool exclCharsDot, bool exclSimilarChars, bool startWithUpper, string length)
        {
            if (quantity.Length == 0) quantity = "2";
            if (length.Length == 0) length = "8";
            int passQuantity = Int32.Parse(quantity);
            int passLength = Int32.Parse(length);
            int symbolTypesQuantity = (upperCase ?  1 : 0) + (lowerCase ? 1 : 0) + (inclNumbers ? 1 : 0) + (inclSymbols ? 1 : 0) + (exclCharsDot ? 0 : 1);
            if (symbolTypesQuantity==0||passLength==0) return "";
            int[] symbolTypes = new int[symbolTypesQuantity];
            int counter = 0;
            // creating an array with password complexity. Each int will be used as a flag to append a symbol.
            if (upperCase) { symbolTypes[counter] = 1; counter++; }
            if (lowerCase) { symbolTypes[counter] = 2; counter++; }
            if (inclNumbers) { symbolTypes[counter] = 3; counter++; }
            if (inclSymbols) { symbolTypes[counter] = 4; counter++; }
            if (!exclCharsDot) { symbolTypes[counter] = 5; counter++; }
            //MessageBox.Show(counter.ToString());
            StringBuilder passTemp = new StringBuilder((passLength+1) * passQuantity);
            for (int i=0; i<passQuantity; i++)
            {
                int j = 0;
                if (startWithUpper)
                {
                    passTemp.Append(GenUpperChar(exclSimilarChars)); j++;
                }
                    for (; j<passLength; j++)
                {
                    int symbType = random.Next(0, symbolTypesQuantity);
                    passTemp.Append(GenEnhancedChar(symbolTypes[symbType], exclSimilarChars));
                }
                passTemp.Append('\n');
            }
            return passTemp.ToString();
        }
        /// <summary>
        /// Generate standard passwords, like Asdfg123
        /// </summary>
        /// <param name="quantity">The needed quantity</param>
        /// <returns>List of passwords</returns>
        public string PassGen(string quantity)
        {
            if (quantity.Length == 0) quantity = "2";
            int passQuantity = Int32.Parse(quantity);
            StringBuilder passTemp = new StringBuilder(9 * passQuantity);

            for (int i = 0; i < passQuantity; i++)
            {
                passTemp.Append(GenUpperChar(false));
                for (int j = 0; j < 4; j++)
                {
                    passTemp.Append(GenLowerChar(false));
                }
                for (int k=0; k<3; k++)
                {
                    passTemp.Append(GenNumber(false));
                }
                passTemp.Append('\n');
            }

            return passTemp.ToString();
        }
        /// <summary>
        /// A helper to create enhanced passwords
        /// </summary>
        /// <param name="type">Type of the char needs to be appened</param>
        /// <param name="exclSimilarChars">Avoid the ambigious characters</param>
        /// <returns>The requested type of char</returns>
        private char GenEnhancedChar(int type, bool exclSimilarChars)
        {
            switch (type)
            {
                case 1: return GenUpperChar(exclSimilarChars);
                case 2: return GenLowerChar(exclSimilarChars);
                case 3: return GenNumber(exclSimilarChars);
                case 4: return GenNormalSymbols();
                case 5: return GenAmbigiousSymbols();
            }
            return 'a';
        }

        private char GenLowerChar (bool excludeSimilar)
        {
            
            int rand;
            do
            {
                rand = random.Next(97, 123);
            } while (excludeSimilar && (rand == 105 || rand == 108 || rand == 111)); //avoiding i,l,o
            return (char)rand;
        }

        private char GenUpperChar (bool excludeSimilar)
        {
            
            int rand;
            do
            {
                rand = random.Next(65, 91);
            } while (excludeSimilar && (rand == 73 || rand == 76 || rand == 79)); //avoiding I, L and O
            return (char) rand;
        }

        private char GenNumber (bool excludeSimilar)
        {
            int rand;
            if (excludeSimilar) //avoiding 0 and 1
            {
                rand = random.Next(50, 58);
            }
            else
            {
                rand = random.Next(48, 58);
            }
            return (char)rand;
        }
        /// <summary>
        /// Gets a random specific char
        /// </summary>
        /// <returns>A random symbol</returns>
        private char GenNormalSymbols ()
        {
            char[] symbols = { '!', '@', '#', '$', '^', '&','*', '=', '-', '+'};
            return (char)symbols[random.Next(0, 10)];
        }
        /// <summary>
        /// Gets ambigious chars like , / etc.
        /// </summary>
        /// <returns>An ambigious char</returns>
        private char GenAmbigiousSymbols ()
        {
            char[] symbols = { '{', '}', '[', ']', '(', ')', '/',  '\'','\\', '"', '~', ',', ';', ':', '.', '<', '>' };
            return (char)symbols[random.Next(0, 16)];
        }
    }
}
