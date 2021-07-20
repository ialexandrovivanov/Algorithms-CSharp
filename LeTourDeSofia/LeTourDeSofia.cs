using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace LeTourDeSofia
{
    // Find lightest route from start vertex to the same start vertex in directed graph with positive vertices
    // If there is no such a path print count of visited vertices while searching (length of the route)

    class LeTourDeSofia
    {
        static Comparer<int> comparer = Comparer<int>.Create((f, s) =>
        weights[f].CompareTo(weights[s]));                   // compares two vertices by weight from start using weights from start memo
        static List<Edge>[] graph;                           // index -> vertex, value list of edges
        static OrderedBag<int> queue;                       // keeps vertices sorted descending by weight from start to current vertex
        static double[] weights;                               // keeps best(smallest) weights from start to current vertex (index)
        static int[] prev;
        static int start;
        static int visited;
        static void Main()
        {
            ProceedInput();

            InitializeCollections();

            var startEdges = graph[start];

            foreach (var edge in startEdges)                // modify input data of Dijkstra algorithm and add start children first in queue
            {
                weights[edge.To] = edge.Distance;           // add to memo weight of child vertex (from start to child weight)
                queue.Add(edge.To);                         // add all start children to queue with their weights from start
            }

            visited = 1;                                    // count of visited vertices (needed for the second condition of the task)

            while (queue.Count > 0)                         // Dijkstra (BFS traverse)
            {
                var currentV = queue.RemoveFirst();         // first is the vertex with smallest weight from start
                visited++;                                  // on vertex popup increase count visited    

                var edges = graph[currentV];                // list of all currentV edges to be added instead of start vertex

                if (currentV == start)                      // final is found(in this case start vertex is also final)
                    break;

                foreach (var edge in edges)
                {
                    var child = edge.To;  // get other vertex from the edge then smallestV

                    if (weights[child] == double.PositiveInfinity)         // not saved in weights memo add to the queue  
                        queue.Add(child);

                    var currentWeight = edge.Distance + weights[currentV]; // get current weight up to current vertex

                    if (currentWeight < weights[child])                    // if current weight is smaller than existing record  
                    {
                        weights[child] = currentWeight;                    // update memo  
                        prev[child] = currentV;                            // update prev for path reconstruction  
                        queue = new OrderedBag<int>(queue, comparer);     // re-sort queue with recently added child
                    }
                }
            }

            if (weights[start] == double.PositiveInfinity)
                Console.WriteLine(visited);
            else
                Console.WriteLine(weights[start]);
        }

        static void ProceedInput()
        {

            var numV = int.Parse(Console.ReadLine());
            var numE = int.Parse(Console.ReadLine());
            start = int.Parse(Console.ReadLine());

            graph = new List<Edge>[numV];

            for (int i = 0; i < graph.Length; i++)
                graph[i] = new List<Edge>();
         
            for (int i = 0; i < numE; i++)
            {
                var data = Console.ReadLine().Split().Select(int.Parse).ToArray();

                var edge = new Edge(data[0], data[1], data[2]);

                graph[edge.From].Add(edge);
            }
        }

        static void InitializeCollections()
        {
            prev = new int[graph.Length];
            queue = new OrderedBag<int>(comparer);
            weights = new double[graph.Length + 1];

            for (int i = 0; i < graph.Length + 1; i++)
                weights[i] = double.PositiveInfinity;
        }

        internal class Edge
        {
            public int From { get; set; }
            public int To { get; set; }
            public int Distance { get; set; }

            public Edge(int from, int to, int distance)
            {
                From = from;
                To = to;
                Distance = distance;
            }
        }
    }
}
