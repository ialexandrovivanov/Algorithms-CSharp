using System;

namespace TestProject
{
    // Find most profitable way to cut up and sell a rod with a given length and given catalog lengths -> prices
    // Example: What is the most profitable way to sell 4 meters of rod (3 possible sell variants -> 1m and 3m; 2m and 2m; 4m)

    class RodCutting
    {
        static int length;              // total length of the rod we want to sell
        static int[] memo;              // index -> index of prices, value -> best price
        static int[] prices;            // catalog lengths prices (for this particular task lengths are from 1 to 10 meters)
        static int[] bestCutLengths;    // keep best cuts when max profit is found (used for keeping all cutted lengths)  
        static void Main()
        {
            ProceedInput();             // get length for selling and catalog length prices

            //CutRodRecursive(length);    // recursive approach

            CutRodIterative(length);    // iterative approach

            PrintResult();
        }

        static int CutRodRecursive(int length)  // start building recursive tree
        {
            if (length == 0)                    // bottom
                return 0;

            if (memo[length] != 0)              // skip recursive calls using memorization
                return memo[length];

            var bestPrice = prices[length];     // assumption best price is whole rod (this is first possibility(child))
            var bestCutLength = length;         // assume most profitable cut length is whole rod

            for (int i = 1; i <= length; i++)   // iterate trough all possible lengths starting from 1 meter 
            {
                var price = prices[i] + CutRodRecursive(length - i);              // (this is second possibility(child))

                if (price > bestPrice)          // choose biggest between first and second possibilities 
                {
                    bestPrice = price;
                    bestCutLength = i;
                }
            }

            memo[length] = bestPrice;           // add to memorization
            bestCutLengths[length] = bestCutLength;

            return bestPrice;
        }

        static void CutRodIterative(int length)
        {
            for (int i = 1; i <= length; i++)
            {
                var bestPrice = prices[i];
                var bestCombo = i;

                for (int j = 1; j <= i; j++)
                {
                    if (memo[j] + memo[i - j] > bestPrice)
                    {
                        bestPrice = memo[j] + memo[i - j];
                        bestCombo = j;
                    }
                }

                memo[i] = bestPrice;
                bestCutLengths[i] = bestCombo;
            }
        }

        static void ProceedInput()
        {
            var catalogData = Console.ReadLine().Split(); 
            length = int.Parse(Console.ReadLine());       // length of rod we want to sell

            prices = new int[catalogData.Length];
            memo = new int[catalogData.Length + 1];       // (catalogData.Length + 1) is to cover all lengths to sell length
            bestCutLengths = new int[catalogData.Length + 1];

            for (int i = 0; i < prices.Length; i++)       // fill prices for different lengs (index -> length, value -> price)
                prices[i] = int.Parse(catalogData[i]);
        }

        static void PrintResult()
        {
            var result = "";
            var total = 0;

            while (length != 0)
            {
                total += prices[bestCutLengths[length]];
                result += $"{bestCutLengths[length]} ";
                length -= bestCutLengths[length];
            }

            Console.WriteLine(total);
            Console.WriteLine(result);
        }
    }
}
