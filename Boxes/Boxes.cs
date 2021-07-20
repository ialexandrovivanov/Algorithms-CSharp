using System;
using System.Collections.Generic;

namespace Boxes
{
    class Boxes
    {
        static int[] parents;       // index -> index of sequense numbers, value parent index of sequence numbers
        static Box[] boxes;         // each number in sequence has own index which is used as link in other collections
        static int[] bestSeqCount;  // index -> sequence index, value best increasing sequence count for that sequence index

        static void Main()
        {
            ProceedInput();

            var bestLen = 0;         // save the last index for the best subsequence lenght
            var lastIdx = 0;         // last index of longest increasing subsequence (for path reconstruction)

            for (int i = 0; i < boxes.Length; i++)     // iterate trough sequence numbers
            {

                var currentBox = boxes[i];
                var currentBestLen = 1;                  // currentBestSeqCount when cheking backwards

                for (int j = i - 1; j >= 0; j--)         // iterate trough all prev nums and if num is smaller take its best seq count + 1
                {
                    var prevBox = boxes[j];

                    if (CompareBoxes(currentBox, prevBox) &&     // current width height and depth > prev width height and depth
                        bestSeqCount[j] + 1 >= currentBestLen)   // Leftmost or rightmost cases depends on >= or >
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

        static bool CompareBoxes(Box currentBox, Box prevBox)
        {
            if (currentBox.Width > prevBox.Width &&      // if current box is bigger than prev box return true
                currentBox.Depth > prevBox.Depth &&
                currentBox.Height > prevBox.Height)
                return true;

            return false;
        }

        static void ProceedInput()
        {
            var countLines = int.Parse(Console.ReadLine());

            boxes = new Box[countLines];

            parents = new int[countLines];
            for (int i = 0; i < countLines; i++)
                parents[i] = -1;
          
            bestSeqCount = new int[countLines];

            for (int i = 0; i < countLines; i++)
            {
                var boxData = Console.ReadLine().Split();
                var width = int.Parse(boxData[0]);
                var depth = int.Parse(boxData[1]);
                var height = int.Parse(boxData[2]);

                var box = new Box(width, depth, height);

                boxes[i] = box;
            }
        }

        static void PrintResult(int lastIdx)
        {
            var stack = new Stack<Box>();

            while (lastIdx != -1)
            {
                stack.Push(boxes[lastIdx]);
                lastIdx = parents[lastIdx];
            }

            foreach (var box in stack)
                Console.WriteLine($"{box.Width} {box.Depth} {box.Height}");
        }
    }

    internal class Box
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public Box(int width, int depth, int height)
        {
            Width = width;
            Depth = depth;
            Height = height;
        }
    }
}
