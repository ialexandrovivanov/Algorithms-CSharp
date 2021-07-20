using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace DijkstraAndMSTAlgorithms
{
    // This program finds the path with minimum weight
    // Weights on edges must be non-negative
    // Graph can be directed or not
    // Path with minimum weight is not necessarily unique
    // Not all edges need to be reachable

    class Program
    {
        static int start;
        static int final;
        static int result;
        static int[] steps;
        static int[] weights;
        static bool[] visited;
        static Stack<int> path;
        static OrderedBag<int> queue;
        static Dictionary<int, List<Edge>> graph;
        static Comparer<Edge> comparer = Comparer<Edge>.Create((f, s) => f.Weight - s.Weight);
        static void Main()
        {
            ProceedInput();

            InitializeCollections();
            ;
            foreach (var edge in graph[start])
                queue.Add(edge);

            while (queue.Count != 0)  // BFS algorithm,  
            {
                var edge = queue.RemoveFirst();

                steps[edge.Second] = edge.First;   // save steps
                weights[edge.Second] = edge.Weight + weights[edge.First];
                visited[edge.First] = true;
                visited[edge.Second] = true;

                if (edge.Second == final)
                {
                    result = weights[edge.Second];
                    ExtractSteps(edge.Second);
                    break;
                }

                foreach (var e in graph[edge.Second]) // edges are passed by weight in descending order
                {
                    if (visited[e.Second])
                        continue;
                 
                    queue.Add(e);
                }
            }

            Console.WriteLine(result);
            Console.WriteLine(string.Join(" ", path));
        }

        static void ProceedInput()
        {
            graph = new Dictionary<int, List<Edge>>();

            var e = int.Parse(Console.ReadLine());

            while (e-- > 0)
            {
                var tokens = Console.ReadLine()
                   .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(int.Parse)
                   .ToArray();

                var edge = new Edge(tokens[0], tokens[1], tokens[2]);

                if (!graph.ContainsKey(edge.First))
                    graph.Add(edge.First, new List<Edge>());

                if (!graph.ContainsKey(edge.Second))
                    graph.Add(edge.Second, new List<Edge>());

                graph[edge.First].Add(edge);
                graph[edge.Second].Add(edge);
            }

            start = int.Parse(Console.ReadLine());
            final = int.Parse(Console.ReadLine());
        }
        private static void InitializeCollections()
        {
            steps = Enumerable.Repeat(-1, graph.Count).ToArray();
            queue = new OrderedBag<Edge>(comparer);
            visited = new bool[graph.Count];
            weights = new int[graph.Count];
            path = new Stack<int>();
            result = int.MaxValue;
            visited[start] = true;
            weights[start] = 0;
        }
        private static void ExtractSteps(int start)
        {
            while (start >= 0)
            {
                path.Push(start);
                start = steps[start];
            }
        }
    }

    class Edge
    {
        public int First { get; set; }
        public int Second { get; set; }
        public int Weight { get; set; }

        public Edge(int first, int second, int weight)
        {
            First = first;
            Second = second;
            Weight = weight;
        }
    }
}