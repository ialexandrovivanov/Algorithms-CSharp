using System;
using System.Collections.Generic;

namespace LongestStringChain
{
    // Find longest increasing subsequence of string lengths in array of strings
    class LongestStringChain
    {
        static int[] len;
        static int[] parent;
        static string[] words;
        static void Main()
        {
            ProceedInput();

            var longestSubseqLen = 0;                  // temp longest subseq length
            var lastIdx = 0;

            for (int current = 0; current < words.Length; current++) // iterate trough all strings from input
            {
                len[current] = 1;                      // initial subsequence length for current string with length 1
                parent[current] = -1;                  // -1 no parent index so far(prev step)
                var currentWord = words[current];   

                for (int prev = current - 1; prev >= 0; prev--)     // iterate way back from current string
                {
                    var prevWord = words[prev];

                    if (currentWord.Length > prevWord.Length &&     // if current string is longer than prev string and
                        len[prev] + 1 >= len[current])   // best subseq len of prev string + 1(current string) is bigger than len[current](1)
                                                         // (>= is for lefmost cases, > rightomst cases)
                    { 
                        len[current] = len[prev] + 1;    // save new best subsequence length for current string 
                        parent[current] = prev;          // save parent for reconstructing the path   
                    }
                }

                if (len[current] > longestSubseqLen)     // if current subseq length is bigger than temp one (mind >= or >)
                {
                    longestSubseqLen = len[current];     // set longestSubLen to current longest subseq length
                    lastIdx = current;                   // set last index for later path reconstruction
                }
            }

            PrintResult(lastIdx);
        }

        static void PrintResult(int idx)
        {
            var stack = new Stack<string>();

            while (idx != - 1)
            {
                stack.Push(words[idx]);
                idx = parent[idx];
            }

            Console.WriteLine(string.Join(" ", stack));
        }

        static void ProceedInput()
        {
            words = Console.ReadLine().Split();

            len = new int[words.Length];

            parent = new int[words.Length];
        }
    }
}
