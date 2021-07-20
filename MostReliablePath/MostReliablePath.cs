using System;
using System.Collections.Generic;
using System.Linq;

namespace MostReliablePath
{
    class MostReliablePath
    {
        // Find most reliable path in a non directed graph(reliability(weight) is represented as percentage)
        // Current vertex % combined with an edge % is acheived by formula(current % * edge % / 100.0)
        // Start vertex has initialy 100% reliability
        // Detects when negative cycles in the graph (cycles in which the current weight decreases at each cycle)

        static int start;
        static int final;
        static int[] prev;
        static int vCount;
        static int eCount;
        static bool noChanges;
        static double[] weights;
        static List<Edge> edges;        // Graph is represented as list of edges
        static void Main()
        {
            ProceedInput();                  

            InitializeCollections();

            weights[start] = 100;

            for (int i = 0; i < vCount - 1; i++)      // Bellman-Ford algorithm will iterate vertices - 1 times 
            {
                noChanges = true;

                foreach (var edge in edges)
                {
                    if (weights[edge.V2] < weights[edge.V1] * edge.Weight / 100.0)
                    {
                        weights[edge.V2] = weights[edge.V1] * edge.Weight / 100.0;
                        prev[edge.V2] = edge.V1;
                        noChanges = false;
                    }
                }

                if (noChanges)        // Saves some iterations if no changes in weights(constant changes means negative cycle)
                    break;
            }

            PrintResult();
        }
            
        static void PrintResult()
        {
            if (noChanges)
            {
                Console.WriteLine($"Most reliable path reliability: {Math.Round(weights[final], 2):F2}%");
                Console.WriteLine(string.Join(" -> ", ReconstructPath()));
            }
            else
                Console.WriteLine("Negative cycle is detected in the graph!");
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

        static void InitializeCollections()
        {
            weights = Enumerable.Repeat(double.NegativeInfinity, vCount).ToArray();
            prev = Enumerable.Repeat(-1, vCount).ToArray();
        }

        static void ProceedInput()
        {
            edges = new List<Edge>();

            vCount = int.Parse(Console.ReadLine());
            eCount = int.Parse(Console.ReadLine());


            for (int i = 0; i < eCount; i++)
            {
                var data = Console.ReadLine().Split().Select(int.Parse).ToArray();

                var v1 = data[0];
                var v2 = data[1];
                var weight = data[2];

                edges.Add(new Edge(v1, v2, weight));        // for non directed graph add both directions
                edges.Add(new Edge(v2, v1, weight));
            }

            start = int.Parse(Console.ReadLine());
            final = int.Parse(Console.ReadLine());
        }
    }
    class Edge
    {
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int Weight { get; set; }

        public Edge(int firstVertex, int SecondVertex, int edgeWeight)
        {
            V1 = firstVertex;
            V2 = SecondVertex;
            Weight = edgeWeight;
        }
    }

}
