using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadTrip
{
    // Find most profitable(valuable) combination of items that can be fitted in a container with given capacity
    // Items have both value and weight
    // Container capacity works with item's weight 

    class RoadTrip
    {
        static int[,] dp;           // rows -> items, cols -> capacities (size is items + 1 by maxCapacity + 1)
        static int maxCapacity;     // given capacity of the contaier
        static bool[,] included;    // included items are true cells
        static List<Item> items;    // given items as list of items
        static void Main()
        {
            ProceedInput();

            for (int itemIdx = 1; itemIdx < dp.GetLength(0); itemIdx++) // iterate trough dp rows
            {
                var currentItem = items[itemIdx - 1];  // first item in the list starts from index 0 (in dp from index 1)

                for (int capacityIdx = 1; capacityIdx < dp.GetLength(1); capacityIdx++) // iterate trough dp cols
                {
                    var skipVal = dp[itemIdx - 1, capacityIdx];

                    if (currentItem.Weight > capacityIdx) // if no enough capacity to take the item get value from above cell
                    {
                        dp[itemIdx, capacityIdx] = skipVal; // update dp
                        continue;
                    }
                    // take value for dp cell is current item's value + prev row, current capacity - current item's weight
                    var takeVal = currentItem.Value + dp[itemIdx - 1, capacityIdx - currentItem.Weight];

                    if (takeVal > skipVal)  // take bigger value from both
                    {
                        dp[itemIdx, capacityIdx] = takeVal; // update dp with takeVal
                        included[itemIdx, capacityIdx] = true; // update included matrix
                    }
                    else
                        dp[itemIdx, capacityIdx] = skipVal; // update dp with skipVal
                }
            }

            BackTrack();
        }

        static void BackTrack()
        {
            var totalValue = dp[items.Count, maxCapacity];      // start from most right/down dp cell
            var includedItems = new SortedSet<string>();
            var totalWeight = 0;                                // total weight of all included items

            for (int rowIdx = included.GetLength(0) - 1; rowIdx >= 0; rowIdx--) // go up rows(items)
            {
                if (included[rowIdx, maxCapacity])                              // check is it included and if so
                {
                    var includedItem = items[rowIdx - 1];                       // get actual item from list

                    includedItems.Add(includedItem.Name);                       // add to incuded collection

                    maxCapacity -= includedItem.Weight;                         // jump on a column maxCapacity - includedItem.Weight
                    totalWeight += includedItem.Weight;                         // add to total weight
                }
            }


            Console.WriteLine($"Maximum value: {totalValue}");
        }

        static void ProceedInput()
        {
            var values = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();
            var spaces = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();

            maxCapacity = int.Parse(Console.ReadLine());

            items = new List<Item>();

            var input = Console.ReadLine();
            for (int i = 0; i < values.Length; i++)
                items.Add(new Item(i.ToString(), spaces[i], values[i]));

            dp = new int[items.Count + 1, maxCapacity + 1];
            included = new bool[items.Count + 1, maxCapacity + 1];
        }
    }

    class Item
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }

        public Item(string name, int weight, int value)
        {
            Name = name;
            Weight = weight;
            Value = value;
        }
    }
}
