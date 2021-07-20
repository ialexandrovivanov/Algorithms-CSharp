using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace DijkstraAndMSTAlgorithms
{
    // This program finds the path with minimum weight in weighted undirected graph from start vertex to final vertex
    // Weights on edges must be non-negative
    // Graph can be directed or not
    // Path with minimum weight is not necessarily unique
    // Not all edges are always reachable

    class DijkstraAndMSTAlgorithms
    {
        static Comparer<int> comparer = Comparer<int>.Create((f, s) =>    
            weights[f].CompareTo(weights[s]));               // compares two vertices by weight from start using weights collection
        static Dictionary<int, List<Edge>> graph;            // key -> vertex, value list of edges
        static OrderedBag<int>  queue;                       // keeps vertices sorted descending by weight from start to current vertex
        static Stack<int> steps;                             // push vertices from final to start when reconstructing the path
        static int[] weights;                                // keeps best weights from start to current vertex (index)
        static int[] prev;
        static int start;
        static int final;
        static void Main()
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

        static void InitializeCollections()
        {
            steps = new Stack<int>();
            prev = new int[graph.Count];
            queue = new OrderedBag<int>(comparer);
            weights = Enumerable.Repeat(int.MaxValue, graph.Count).ToArray();
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

        static void ProceedInput()
        {
            graph = new Dictionary<int, List<Edge>>();

            var e = int.Parse(Console.ReadLine());

            while (e-- > 0)
            {
                var tokens = Console.ReadLine()
                   .Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(int.Parse)
                   .ToArray();

                var edge = new Edge(tokens[0], tokens[1], tokens[2]);

                if (!graph.ContainsKey(edge.V1))
                    graph.Add(edge.V1, new List<Edge>());

                if (!graph.ContainsKey(edge.V2))
                    graph.Add(edge.V2, new List<Edge>());

                graph[edge.V1].Add(edge);
                graph[edge.V2].Add(edge);
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
