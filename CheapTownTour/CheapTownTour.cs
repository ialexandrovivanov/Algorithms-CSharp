using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace CheapTownTour
{
    class CheapTownTour
    {
        // This program results a minimum spanning tree (MST) using Prim's algorithmr
        // MST is a tree with smallest weight but still connects all vertices 
        // Several MST could exist in some graphs
        // Example problem it solves: Connect all homes to a network using streets (least length of cable installed on streets)

        static int vCount;
        static int eCount;
        static Dictionary<int, List<Edge>> graph = new Dictionary<int, List<Edge>>();
        static Comparer<Edge> comparer = Comparer<Edge>.Create((f, s) => f.Weight - s.Weight);
        static HashSet<int> tree = new HashSet<int>();
        static List<Edge> mstEdges = new List<Edge>();
        static void Main()
        {
            ProceedInput();

            foreach (var vertex in graph.Keys)
            {
                if (!tree.Contains(vertex))
                    GetMSTEdges(vertex);
            }

            Console.WriteLine($"Total cost: {mstEdges.Sum(s => s.Weight)}");
        }
        static void GetMSTEdges(int vertex)
        {
            tree.Add(vertex);

            var queue = new OrderedBag<Edge>(graph[vertex], comparer);

            while (queue.Count > 0)
            {
                var edge = queue.RemoveFirst();

                if (tree.Contains(edge.V1) && !tree.Contains(edge.V2))
                {
                    tree.Add(edge.V2);
                    mstEdges.Add(edge);
                    queue.AddMany(graph[edge.V2]);      // Union current edges with edges of the recently added vertex
                }
                else if (tree.Contains(edge.V2) && !tree.Contains(edge.V1))
                {
                    tree.Add(edge.V1);
                    mstEdges.Add(edge);
                    queue.AddMany(graph[edge.V1]);      // Union current edges with edges of the recently added vertex
                }
            }
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());
            eCount = int.Parse(Console.ReadLine());

            graph = new Dictionary<int, List<Edge>>();

            for (int i = 0; i < eCount; i++)
            {
                var data = Console.ReadLine()
                    .Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                var edge = new Edge(data[0], data[1], data[2]);

                if (!graph.ContainsKey(edge.V1))
                    graph.Add(edge.V1,new List<Edge>());

                if (!graph.ContainsKey(edge.V2))
                    graph.Add(edge.V2, new List<Edge>());

                graph[edge.V1].Add(edge);    
                graph[edge.V2].Add(edge);
            }
        }
    }

    class Edge
    {
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int Weight { get; set; }

        public Edge(int firstVertex, int secondVertex, int edgeWeight)
        {
            V1 = firstVertex;
            V2 = secondVertex;
            Weight = edgeWeight;
        }
    }
}
