using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace FoodProgramme
{
    // Find fastest way possible between two vertices
    // The tricky part here is that you can use both directions in this graph 
    class FoodProgramme
    {
        static Comparer<int> comparer = Comparer<int>.Create((f, s) =>
           weights[f].CompareTo(weights[s]));               // compares two vertices by weight from start using weights collection
        static List<Edge>[] graph;                           // key -> vertex, value list of edges
        static OrderedBag<int> queue;                       // keeps vertices sorted descending by weight from start to current vertex
        static Stack<int> steps;                            // push vertices from final to start when reconstructing the path
        static int[] weights;                               // keeps best weights from start to current vertex (index)
        static int[] prev;
        static int start;
        static int final;

        static void Main(string[] args)
        {
            ProceedInput();

            InitializeCollections();

            queue.Add(start);
            prev[start] = -1;
            weights[start] = 0;

            while (queue.Count > 0)                          // BFS
            {
                var currentV = queue.RemoveFirst();          // first is the vertex with smallest weight from start
                var edges = graph[currentV];                 // list of all currentV edges

                if (currentV == final)                       // final is found
                    break;

                foreach (var edge in edges)
                {
                    var child = edge.V1 == currentV ? edge.V2 : edge.V1;  // get other vertex from the edge then smallestV

                    if (weights[child] == int.MaxValue)                   // if not saved in weights memo add to the queue  
                        queue.Add(child);

                    var currentWeight = edge.Weight + weights[currentV];  // get current weight up to current vertex

                    if (currentWeight < weights[child])                   // if current weight is smaller than existing record  
                    {
                        weights[child] = currentWeight;                   // update memo  
                        prev[child] = currentV;                           // update prev for path reconstruction  
                        queue = new OrderedBag<int>(queue, comparer);     // re-sort queue with recently added child
                    }
                }
            }

            if (weights[final] == int.MaxValue)                           // unreachable vertex from start
                Console.WriteLine("There is no such path.");
            else
            {
                Console.WriteLine(weights[final]);
                GetPath();
                Console.WriteLine(string.Join(" ", steps));
            }

        }

        static void ProceedInput()
        {
            var vCount = int.Parse(Console.ReadLine());
            var eCount = int.Parse(Console.ReadLine());
            var dataStartFinal = Console.ReadLine().Split().Select(int.Parse).ToArray();
           
            start = dataStartFinal[0];
            final = dataStartFinal[1];

            graph = new List<Edge>[vCount];
            for (int i = 0; i < vCount; i++)
                graph[i] = new List<Edge>();

            for (int i = 0; i < eCount; i++)
            {
                var dataEdges = Console.ReadLine().Split().Select(int.Parse).ToArray();

                var from = dataEdges[0];
                var to = dataEdges[1];
                var weight = dataEdges[2];
                var edge = new Edge(from, to, weight);

                graph[from].Add(edge);
                graph[to].Add(edge);
            }
        }

        static void InitializeCollections()
        {
            steps = new Stack<int>();
            prev = new int[graph.Length];
            queue = new OrderedBag<int>(comparer);
            weights = Enumerable.Repeat(int.MaxValue, graph.Length).ToArray();
        }

        static void GetPath()
        {
            var val = final;

            while (val != -1)
            {
                steps.Push(val);
                val = prev[val];
            }
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
