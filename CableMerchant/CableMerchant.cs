using System;
using System.Collections.Generic;
using System.Linq;

namespace CableMerchant
// Find the most profitable way to cut up and sell a cable as a whole whit the help of connectors with given price
// Cable can both be sold as a whole piece or cutted and than joined by 2 connectors per cut (best way as profit is choosen here)
// Catalog with prices is given for each length (1 meter step) starting from 1 and a price for connector
{
    class CableMerchant
    {
        static int[] bestPrices;    // idx -> idx in prices (length), value -> best price (best price for certain length)
        static List<int> prices;    // index -> length, value -> price (on idx 0 val is 0, prices start from index 1 for 1 meter and so on)
        static int connectorPrice;  // each cut needs two connectors to become whole cable

        static void Main()
        {
            ProceedInput();         // get catalog from input

            for (int length = 1; length < prices.Count; length++)   // iterate trough all lengths and find best price for each one
                bestPrices[length] = FindCutPrices(length);         // recursively find best price for each length and save it

            PrintResult();          // print best prices for all lengths
        }

        static int FindCutPrices(int length)    
        {
            if (length == 0)                    // bottom of recursion
                return 0;

            if (bestPrices[length] != 0)        // memorization returns best price for a length if such is saved (skips recursive calls)
                return bestPrices[length];

            var bestPrice = prices[length];     // assume best price is for the whole cable length (this is first option)

            for (int i = 1; i <= length; i++)   // cut cable to all possible lengths starting from 1 meter
            {
                var currentPrice = prices[i] + FindCutPrices(length - i) - 2 * connectorPrice;  // (this is second option)

                if (currentPrice > bestPrice)   // choose bigger between first and second options after recursion result 
                    bestPrice = currentPrice;
            }

            bestPrices[length] = bestPrice;     // add best price for a length to result (also used as memo)

            return bestPrice;                   // return best price to recursive chain
        }

        static void ProceedInput()
        {
            prices = new List<int>() { 0 };     //  initialy insert 0 so indexes from 1 will match rod lengths from input
            prices.AddRange(Console.ReadLine().Split().Select(int.Parse));   // add rod lengths

            connectorPrice = int.Parse(Console.ReadLine());

            bestPrices = new int[prices.Count];
        }

        static void PrintResult()
        {
            for (int i = 1; i < bestPrices.Length; i++)
            {
                if (i == bestPrices.Length - 1)
                    Console.Write(bestPrices[i]);
                else
                    Console.Write(bestPrices[i] + " ");
            }
        }
    }
}
