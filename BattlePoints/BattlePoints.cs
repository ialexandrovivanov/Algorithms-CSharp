using System;
using System.Collections.Generic;
using System.Linq;

namespace BattlePoints
{
    class BattlePoints
    {
        static int[,] dp;           // rows -> items, cols -> capacities (size is items + 1 by maxCapacity + 1)
        static int maxCapacity;     // given capacity of the contaier
        static bool[,] included;    // included items are true cells
        static int[] catalogEnergy; // index -> enemy id, value -> energy taken to defeat this enemy
        static int[] catalogPoints; // index -> enemy id, value -> points given if you defeat this enemy

        static void Main()
        {
            ProceedInput();

            for (int enemyIdx = 1; enemyIdx < dp.GetLength(0); enemyIdx++)
            {
                var currentEnemyEnergy = catalogEnergy[enemyIdx - 1];
                var currentEnemyPoints = catalogPoints[enemyIdx - 1];

                for (int capacityIdx = 1; capacityIdx < dp.GetLength(1); capacityIdx++)
                {
                    var skipVal = dp[enemyIdx - 1, capacityIdx]; // cell above

                    if (currentEnemyEnergy > capacityIdx)        // if no enough capacity to take the item get value from above cell
                    {
                        dp[enemyIdx, capacityIdx] = skipVal;     // update dp
                        continue;
                    }
                    //  take value for dp cell is current item's value + prev row, current capacity - current item's weight
                    var takeVal = currentEnemyPoints + dp[enemyIdx - 1, capacityIdx - currentEnemyEnergy];

                    if (takeVal > skipVal)    // take bigger value from both
                    {
                        dp[enemyIdx, capacityIdx] = takeVal;     // update dp with takeVal
                        included[enemyIdx, capacityIdx] = true;  // update included matrix for path reconstruction
                    }
                    else
                        dp[enemyIdx, capacityIdx] = skipVal;     // update dp with skipVal

                }
            }

            //BackTrack();
            Console.WriteLine(dp[catalogEnergy.Length, maxCapacity]);
        }

        static void ProceedInput()
        {
            catalogEnergy = Console.ReadLine().Split().Select(int.Parse).ToArray();

            catalogPoints = Console.ReadLine().Split().Select(int.Parse).ToArray();

            maxCapacity = int.Parse(Console.ReadLine());

            dp = new int[catalogEnergy.Length + 1, maxCapacity + 1];
            included = new bool[catalogEnergy.Length + 1, maxCapacity + 1];
        }

        static void BackTrack()
        {
            var totalValue = dp[catalogEnergy.Length, maxCapacity];             // start from most right/down dp cell
            var includedItems = new SortedSet<string>();
            var totalWeight = 0;                                                // total weight of all included items

            for (int rowIdx = included.GetLength(0) - 1; rowIdx >= 0; rowIdx--) // go up rows(items)
            {
                if (included[rowIdx, maxCapacity])                              // check is it included and if so
                {
                    includedItems.Add(rowIdx - 1 + " " + catalogEnergy[rowIdx - 1] + " " + catalogPoints[rowIdx - 1]); 

                    maxCapacity -= catalogEnergy[rowIdx - 1];                    // jump on a column maxCapacity - includedItem.Weight
                    totalWeight += catalogEnergy[rowIdx - 1];                    // add to total weight
                }
            }

            Console.WriteLine($"Total Weight: {totalWeight}");
            Console.WriteLine($"Total Value: {totalValue}");
            Console.WriteLine("three numbers are: enemy idx, energy taken, points given");
            Console.WriteLine(string.Join("\n", includedItems));
        }
    }
}
