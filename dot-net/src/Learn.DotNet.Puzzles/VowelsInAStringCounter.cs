using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn.DotNet.Puzzles
{
    public class VowelsInAStringCounter
    {
        static char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };

        public static int GetVowelCount(string input)
        {
            int result = 0;
            char[] inputArray = input.ToCharArray();

            foreach (var item in inputArray)
            {
                if (vowels.Contains(item))
                {
                    result++;
                }
            }

            return result;
        }
    }
}
