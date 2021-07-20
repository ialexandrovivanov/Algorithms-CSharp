using System;
using System.Collections.Generic;
using System.Linq;

namespace KruskalAlgorithm
{
    class KruskalAlgorithm
    {
        // This program results a minimum spanning tree (MST) using Kruskal's algorithm
        // MST is a tree with smallest weight but still connects all vertices 
        // Several MST could exist in some graphs
        // Example problem it solves: Connect all homes to a network using streets (least length of cable installed on streets)

        static Dictionary<int, HashSet<Edge>> graph;
        static List<Edge> graphEdgesSortedDesc = new List<Edge>();
        static List<Edge> mstEdges;
        static void Main()
        {
            BuildGraphFromInput();

            mstEdges = GetMstEdges(graph.Count, graphEdgesSortedDesc);

            PrintResult(mstEdges);
        }

        static void PrintResult(List<Edge> allEdges)
        {
            foreach (var edge in allEdges)
                Console.WriteLine($"{edge.V1} - {edge.V2}");
        }

        static List<Edge> GetMstEdges(int vCount, List<Edge> edges)
        {
            var roots = Enumerable.Range(0, vCount).ToArray();          // index -> vertex, value -> root of the vertex (initialy index == value)
            var treeEdges = new List<Edge>();
            
            foreach (var edge in edges.OrderBy(s => s.Weight))
            {
                var rootVertex1 = FindRoot(edge.V1, roots);             // find root to each vertex of the edge and if roots are different
                var rootVertex2 = FindRoot(edge.V2, roots);             // this edge is not connected (added) to mst

                if (rootVertex1 != rootVertex2)                         // if true -> different trees (so add edge)
                {
                    treeEdges.Add(edge);                                // Add edge to spanning tree
                    roots[rootVertex2] = rootVertex1;                   // Update second node root with new common one (rootFirstNode)
                }
            }
            
            return treeEdges;                            
        }

        static int FindRoot(int node, int[] roots)
        {
            var root = node;

            while (root != roots[root])                                 // Goes down to the root vertex (using roots to reconstruct path) 
                root = roots[root];

            return root;
        }

        static void BuildGraphFromInput()
        {
            graph = new Dictionary<int, HashSet<Edge>>();

            var e = int.Parse(Console.ReadLine());

            while (e-- > 0)
            {
                var data = Console.ReadLine()
                   .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(int.Parse)
                   .ToArray();

                var edge = new Edge(data[0], data[1], data[2]);

                if (!graph.ContainsKey(edge.V1))
                    graph.Add(edge.V1, new HashSet<Edge>());

                if (!graph.ContainsKey(edge.V2))
                    graph.Add(edge.V2, new HashSet<Edge>());

                graph[edge.V1].Add(edge);
                graph[edge.V2].Add(edge);

                graphEdgesSortedDesc.Add(edge);
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
