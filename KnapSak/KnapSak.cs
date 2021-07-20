using System;
using System.Collections.Generic;

namespace KnapSak
{
    // Find most profitable(valuable) combination of items that can be fitted in a container with given capacity
    // Items have both value and weight
    // Container capacity works with item's weight 

    class KnapSak
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

          
            Console.WriteLine($"Total Weight: {totalWeight}");
            Console.WriteLine($"Total Value: {totalValue}");
            Console.WriteLine(string.Join("\n", includedItems));
        }

        static void ProceedInput()
        {
            maxCapacity = int.Parse(Console.ReadLine());
            items = new List<Item>();

            var input = Console.ReadLine();

            while (input != "end")
            {
                var tokens = input.Split();  // input line -> "Item1 5 30" <- Name Weight Value of the item separated by spaces 
                items.Add(new Item(tokens[0], int.Parse(tokens[1]), int.Parse(tokens[2])));
                input = Console.ReadLine();
            }

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
