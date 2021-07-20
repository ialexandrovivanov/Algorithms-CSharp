using System;
using System.Collections.Generic;
using System.Linq;

namespace BellmanFordAlgorithm
{
    class BellmanFordAlgorithm
    {
        // Finds lightest path in directed wighted graph(possitive and negative edges)(Can be both directed or undirected)
        // Detects when negative cycles in the graph (cycles in which the current weight decreases at each cycle)

        static int start;
        static int final;
        static int vCount;
        static int eCount;
        static int[] prev;
        static Edge[] graph;        // Graph is represented as list of edges
        static double[] weights;
        static bool noNegativeCycle;
        static void Main()
        {
            ProceedInput();         // Fill the graph with input data

            InitializeCollections();

            for (int i = 0; i < vCount - 1; i++)      // Bellman-Ford algorithm will iterate (vertices - 1) 
            {
                noNegativeCycle = true;

                foreach (var edge in graph)
                {
                    if (weights[edge.V2] > weights[edge.V1] + edge.Weight && // if memo of child vertex is bigger than memo of parent vertex +
                        prev[edge.V2] != double.PositiveInfinity)            // current weight and the memo for prev of child vertex is visited
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

        static void PrintResult()
        {
            if (noNegativeCycle)
            {
                var steps = ReconstructPath();

                Console.WriteLine(string.Join(" ", steps));
                Console.WriteLine(weights[final]);
            }
            else
                Console.WriteLine("Negative Cycle Detected");
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
            weights = Enumerable.Repeat(double.PositiveInfinity, vCount + 1).ToArray(); // initial weights from start v to current v (unknown)
            weights[start] = 0;                                                             

            prev = Enumerable.Repeat(-1, vCount + 1).ToArray();                         // initial prev value -1 (means no parent)
        }

        static void ProceedInput()
        {

            vCount = int.Parse(Console.ReadLine());
            eCount = int.Parse(Console.ReadLine());

            graph = new Edge[eCount];

            for (int i = 0; i < eCount; i++)
            {
                var edgeData = Console.ReadLine().Split().Select(int.Parse).ToArray();

                graph[i] = new Edge(edgeData[0], edgeData[1], edgeData[2]);
            }

            start = int.Parse(Console.ReadLine());
            final = int.Parse(Console.ReadLine());
        }

        class Edge
        {
            public int V1 { get; set; }
            public int V2 { get; set; }
            public int Weight { get; set; }

            public Edge(int vertex1, int vertex2, int weight)
            {
                V1 = vertex1;
                V2 = vertex2;
                Weight = weight;
            }
        }
    }
}
