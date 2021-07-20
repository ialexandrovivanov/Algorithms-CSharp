using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace CableNetwork
{
    // Extend existing cable network(graph) by connecting as many customers(vertices) as possible within a fixed budget limit
    // To connect a customer(vertex) edge-weight is spent from the budget
    // Print actual budget spent extending the network
    class CableNetwork
    {
        static int spent;
        static int budget;
        static int vCount;
        static int eCount;
        static HashSet<int> spanningTree = new HashSet<int>();
        static List<Edge>[] graph;

        static void Main()
        {
            ProceedInput();

            Prim();

            Console.WriteLine($"Budget used: {spent}");  
        }

        static void ProceedInput()
        {
            budget = int.Parse(Console.ReadLine());
            vCount = int.Parse(Console.ReadLine());
            eCount = int.Parse(Console.ReadLine());

            graph = new List<Edge>[vCount];

            for (int i = 0; i < vCount; i++)
                graph[i] = new List<Edge>();
          
            for (int i = 0; i < eCount; i++)
            {
                var data = Console.ReadLine().Split().ToArray();

                var vertex1 = int.Parse(data[0]);
                var vertex2 = int.Parse(data[1]);
                var edgeWeight = int.Parse(data[2]);
                var edge = new Edge(vertex1, vertex2, edgeWeight);

                if (data.Length == 4)            // edge is already connected (part of the existing network)
                {
                    spanningTree.Add(vertex1);   //  add connected vertices to spanning tree (MST)
                    spanningTree.Add(vertex2);
                    graph[vertex1].Add(edge);
                    graph[vertex2].Add(edge);
                }
                else
                { 
                    graph[vertex2].Add(edge);    // edge is not connected (not part of the existing network)
                    graph[vertex1].Add(edge);
                }
            }
        }

        static void Prim()
        {
            var queue = new OrderedBag<Edge>(Comparer<Edge>.Create((f, s) => f.Weight - s.Weight));

            foreach (var vertex in spanningTree)
                queue.AddMany(graph[vertex]);   
           
            while (queue.Count != 0)
            {
                var edge = queue.RemoveFirst();

                if (edge.Weight > budget)                    // if edge weight is bigger than buddget keep searching
                    continue;

                var nonTreeVertex = GetNonTreeVertex(edge);  // method returns non tree vertex or -1 if both are part of the network

                if (nonTreeVertex == -1)                     // if both vertices of the edge are already connected, keep searching
                    continue;

                spent += edge.Weight;                        
                budget -= edge.Weight;
                spanningTree.Add(nonTreeVertex);            
                queue.AddMany(graph[nonTreeVertex]);
            }
        }

        static int GetNonTreeVertex(Edge edge)
        {
            if (spanningTree.Contains(edge.V1) && !spanningTree.Contains(edge.V2))
                return edge.V2;
            else if (spanningTree.Contains(edge.V2) && !spanningTree.Contains(edge.V1))
                return edge.V1;
            else
                return -1;
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
