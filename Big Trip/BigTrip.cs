using System;
using System.Collections.Generic;
using System.Linq;

namespace BigTrip
{
    // Finds heaviest path in directed wighted graph(possitive and negative edges)

    class BigTrip
    {
        static int start;
        static int final;
        static int[] prev;
        static int vCount;
        static int eCount;
        static Edge[] graph;        // Graph is represented as array of edges
        static double[] weights;
        static bool noNegativeCycle;

        static void Main()
        {
            ProceedInput();

            weights = Enumerable.Repeat(double.NegativeInfinity, vCount + 1).ToArray();
            weights[start] = 0;
            prev = Enumerable.Repeat(-1, vCount + 1).ToArray();

            for (int i = 0; i < graph.Length - 1; i++)      // Bellman-Ford algorithm will iterate (vertices - 1) 
            {
                noNegativeCycle = true;

                foreach (var edge in graph)
                {
                    if (weights[edge.V2] < weights[edge.V1] + edge.Weight &&
                        prev[edge.V2] != double.NegativeInfinity)
                    {
                        weights[edge.V2] = weights[edge.V1] + edge.Weight;
                        prev[edge.V2] = edge.V1;
                        noNegativeCycle = false;
                    }
                }

                if (noNegativeCycle)        // Saves some iterations if no negative cycle
                    break;
            }

            PrintResult();
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());
            eCount = int.Parse(Console.ReadLine());

            graph = new Edge[eCount];

            for (int i = 0; i < eCount; i++)
            {
                var data = Console.ReadLine().Split().Select(int.Parse).ToArray();

                var firstVertex = data[0];
                var secondVertex = data[1];
                var edgeWeight = data[2];
                var edge = new Edge(firstVertex, secondVertex, edgeWeight);

                graph[i] = edge;
            }

            start = int.Parse(Console.ReadLine());
            final = int.Parse(Console.ReadLine());
        }

        static IEnumerable<int> ReconstructPath()
        {
            var steps = new Stack<int>();
            var val = final;

            while (val != -1)
            {
                steps.Push(val);
                val = prev[val];
            }

            return steps;
        }

        static void PrintResult()
        {
            if (noNegativeCycle)
            {
                var steps = ReconstructPath();

                Console.WriteLine(weights[final]);
                Console.WriteLine(string.Join(" ", steps));
            }
            else
                Console.WriteLine("Undefined");
        }

    }
    internal class Edge
    {
        public int V1;
        public int V2;
        public int Weight;

        public Edge(int firstVertex, int secondVertex, int edgeWeight)
        {
            V1 = firstVertex;
            V2 = secondVertex;
            Weight = edgeWeight;
        }
    }
}
