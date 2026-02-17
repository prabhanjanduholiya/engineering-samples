using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn.DotNet.Puzzles
{
    public class OpenCloseBracesPatternIdentification
    {
        public static bool IsValidString(string input)
        {
            var chars = input.ToCharArray();

            int balance = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '[')
                {
                    balance++;
                }
                else if (chars[i] == ']')
                {
                    balance--;
                }
                else
                {
                    throw new Exception("Invalid input string");
                }

                if (balance < 0)
                {
                    throw new Exception("Invalid input string");
                }
            }

            return balance == 0;
        }
    }
}
