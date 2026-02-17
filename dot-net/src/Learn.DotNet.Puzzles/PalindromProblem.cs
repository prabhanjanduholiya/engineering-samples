namespace Learn.DotNet.Puzzles
{
    public static class PalindromProblem
    {
        public static bool IsPalindromString(string test)
        {
            var result = false;

            var items = test.ToCharArray();
            
            var middle = items.Length / 2;
            
            for (int i = 0; i < items.Length; i++)
            {
                if (middle < i)
                {
                    break;
                }

                var item = items[i];
            
                var lastItem = items[(items.Length - 1) - i];

                if (item == lastItem)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
