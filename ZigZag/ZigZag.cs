using System;
using System.Collections.Generic;

namespace ZigZag
{
    // Find longest leftmost zigzag subsequence of numbers out of given sequence of numbers
    // Zigzag subsequences are (2 3 1 4 0) or (3 2 4 1 5) (num < num > num < num) or (num > num < num > num)

    class ZigZag
    {
        static int[] numbers;            // initial sequence of numbers from input
        static int[] parentsBigger;      // index -> index of sequense numbers, value -> parent index of sequence numbers
        static int[] parentsSmaller;     // index -> index of sequense numbers, value -> parent index of sequence numbers
        static int[] bestSubseqBigger;   // index -> numbers index, value best bigger subseq count
        static int[] bestSubseqSmaller;  // index -> numbers index, value best smaller subseq count

        static void Main()
        {
            ProceedInput();

            var bestSize = 0;           // shows when any subsequence count is increased
            var lastColIdx = 0;         // for path reconstruction (last column index to look at when reconstruct)
            var isLastBigger = false;   // for path reconstruction (last collection to look in when reconstruct)

            for (int currentIdx = 0; currentIdx < numbers.Length; currentIdx++)      // iterate trough sequence numbers
            {
                var currentNum = numbers[currentIdx];

                if (currentIdx == 0)                                 // first number from sequence
                {
                    bestSubseqBigger[currentIdx] = 1;                // initial longest subsequence       
                    bestSubseqSmaller[currentIdx] = 1;               // initial longest subsequence  
                }
                else                                                 // different from first number in sequence
                {
                    for (int prevIdx = currentIdx - 1; prevIdx >= 0; prevIdx--) // iterate trough all prev nums
                    {
                        var prevNum = numbers[prevIdx];
                        
                        if (currentNum > prevNum &&   // if current num is bigger than prev num look for best prev smaller subsequence (zigzag)
                            bestSubseqSmaller[prevIdx] + 1 >= bestSubseqBigger[currentIdx])  // >= leftmost cases, > rightmost cases
                        {
                            bestSubseqBigger[currentIdx] = bestSubseqSmaller[prevIdx] + 1;
                            parentsBigger[currentIdx] = prevIdx;
                        }

                        if (currentNum < prevNum &&   // if current num is smaller than prev num look for best prev bigger subsequence (zigzag)
                            bestSubseqBigger[prevIdx] + 1 >= bestSubseqSmaller[currentIdx])  // >= leftmost cases, > rightmost cases
                        {
                            bestSubseqSmaller[currentIdx] = bestSubseqBigger[prevIdx] + 1;
                            parentsSmaller[currentIdx] = prevIdx;
                        }
                    }
                }

                if (bestSubseqBigger[currentIdx] > bestSize) // If best bigger subsequence count is increased
                { 
                    bestSize = bestSubseqBigger[currentIdx];
                    isLastBigger = true;
                    lastColIdx = currentIdx;
                }

                if (bestSubseqSmaller[currentIdx] > bestSize)  // If best smaller subsequence count is increased
                {
                    bestSize = bestSubseqSmaller[currentIdx];
                    isLastBigger = false;
                    lastColIdx = currentIdx;
                }
                
            }

            PrintResult(isLastBigger, lastColIdx);
        }

        static void ProceedInput()
        {
            var input = Console.ReadLine().Split();

            numbers = new int[input.Length];
            bestSubseqBigger = new int[input.Length];
            parentsBigger = new int[input.Length];
            bestSubseqSmaller = new int[input.Length];
            parentsSmaller = new int[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                parentsBigger[i] = -1;
                parentsSmaller[i] = -1;
                numbers[i] = int.Parse(input[i]);
            }
        }

        static void PrintResult(bool bigger, int lastColIdx)
        {
            var zigzagSubseq = new Stack<int>();

            while (lastColIdx != -1)
            {
                zigzagSubseq.Push(numbers[lastColIdx]);

                if (bigger)             // look in parentsBigger for prev col index (zigzag)
                {
                    lastColIdx = parentsBigger[lastColIdx];
                    bigger = !bigger;   // next time look in smaller (zigzag)
                }
                else                    // look in parentsSmaller for prev col index (zigzag)
                {
                    lastColIdx = parentsSmaller[lastColIdx];
                    bigger = !bigger;   // next time look in bigger (zigzag)
                }
            }

            Console.WriteLine(string.Join(" ", zigzagSubseq));
        }
    }
}
