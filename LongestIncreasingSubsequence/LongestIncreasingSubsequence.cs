using System;
using System.Collections.Generic;

namespace LongestIncreasingSubsequence
{
    // Find leftmost longest non consecutive increasing subsequnce of numbers in a sequence of numbers

    class LongestIncreasingSubsequence
    {
        static int[] parents;       // index -> index of sequense numbers, value parent index of sequence numbers
        static int[] numbers;       // each number in sequence has own index which is used as link in other collections
        static int[] bestSeqCount;  // index -> sequence index, value best increasing sequence count for that sequence index

        static void Main()
        {
            ProceedInput();

            var bestLen = 0;         // save the last index for the best subsequence lenght
            var lastIdx = 0;         // last index of longest increasing subsequence (for path reconstruction)

            for (int i = 0; i < numbers.Length; i++)     // iterate trough sequence numbers
            {
                
                var currentNum = numbers[i];                
                var currentBestLen = 1;                  // currentBestSeqCount when cheking backwards

                for (int j = i - 1; j >= 0; j--)         // iterate trough all prev nums and if num is smaller take its best seq count + 1
                {
                    var prevNum = numbers[j];

                    if (prevNum < currentNum &&
                        bestSeqCount[j] + 1 >= currentBestLen)  // Leftmost or rightmost cases depends on >= or >
                    { 
                        currentBestLen = bestSeqCount[j] + 1;
                        parents[i] = j;
                    }
                }

                if (currentBestLen > bestLen)
                {
                    bestLen = currentBestLen;
                    lastIdx = i;
                }

                bestSeqCount[i] = currentBestLen;         // add to memorization
            }

            PrintResult(lastIdx);

        }

        static void PrintResult(int lastIdx)
        {
            var stack = new Stack<int>();

            while (lastIdx != -1)
            {
                stack.Push(numbers[lastIdx]);
                lastIdx = parents[lastIdx];
            }

            Console.WriteLine(string.Join(" ", stack));
        }

        static void ProceedInput()
        {
            var input = Console.ReadLine().Split();

            numbers = new int[input.Length];
            parents = new int[input.Length];
            bestSeqCount = new int[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                parents[i] = -1;
                numbers[i] = int.Parse(input[i]);
            }
        }
    }
}
