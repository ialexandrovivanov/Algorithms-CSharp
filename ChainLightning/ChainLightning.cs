using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace ChainLightning
{
    // Find the vertex with most damage after given count of lightning hits on given vertices in undirected weighted graph
    // Lightning hit is covering all vertices trough smallest resistance edges starting from given vertex (direct hit vertex)
    // Than lightning is traveling from start vertex down to children and on each of the next levels of depth the power = power / 2

    class ChainLightning
    {
        static int[] damage;          // damage memo for each vertex (index -> vertex, value -> accumulative damage from all ligthening)
        static bool[] visited;        // temp memo for visited vertices when traversing the graph with GetMstTree and CalculateDamage methods
        static int countLightning;    // total count of lightning  
        static List<Edge>[] graph;    // non directed graph (index -> vertex, value -> list of vertex edges)
        static List<Edge>[] mstTree;  // mst tree (cover all vertices with smaller electrical resistance when lightening hits)

        static void Main()
        {
            ProceedInput();           // fill graph with resistance paths in both directions (undirected graph)

            for (int i = 0; i < countLightning; i++)           // iterate trough all lightning hits and calculate damage for each vertex
            {
                mstTree = new List<Edge>[graph.Length];        // initialize new mstTree with empty lists for each loop
                for (int idx = 0; idx < mstTree.Length; idx++) 
                    mstTree[idx] = new List<Edge>();

                visited = new bool[graph.Length];              // clear visited memo for each loop (GetMstTree relay on it)

                var lightComponents = Console.ReadLine().Split().Select(int.Parse).ToArray();
                var hitVert = lightComponents[0];              // direct hitted vertex
                var hitPower = lightComponents[1];

                GetMstTree(hitVert);                           // spanning tree covering all paths with minimal electrical resistance (mstTree)

                visited = new bool[graph.Length];              // clear visited memo for reuse from CalculateDamage method

                CalculateDamage(hitVert, hitPower);            // traverse with dfs and add damage to each vertex (each deeper level damage / 2)
            }
            
            Console.WriteLine(damage.Max());                   // print maximum damaged vertex from damage memo
        }

        static void ProceedInput()
        {
            var countVertices = int.Parse(Console.ReadLine());
            var countEdges = int.Parse(Console.ReadLine());
            countLightning = int.Parse(Console.ReadLine());

            damage = new int[countVertices];
            graph = new List<Edge>[countVertices];

            for (int i = 0; i < graph.Length; i++)
                graph[i] = new List<Edge>();

            for (int i = 0; i < countEdges; i++)
            {
                var data = Console.ReadLine().Split().Select(int.Parse).ToArray();
                var v1 = data[0];
                var v2 = data[1];
                var weight = data[2];

                graph[v1].Add(new Edge(v1, v2, weight));
                graph[v2].Add(new Edge(v1, v2, weight));
            }
        }

        static void CalculateDamage(int vertex, int power)
        {
            if (visited[vertex])
                return;
          
            visited[vertex] = true;

            foreach (var edge in mstTree[vertex])
            {
                var oppositeV = edge.V1 == vertex ? edge.V2 : edge.V1;
                CalculateDamage(oppositeV, power / 2);
            }

            damage[vertex] += power;       
        }

        static void GetMstTree(int startVertex)
        {
            visited[startVertex] = true;

            //  queue is ordered bag of edges compared by weight (smallest first)
            var queue = new OrderedBag<Edge>(Comparer<Edge>.Create((f, s) => f.Weight - s.Weight)); 
            queue.AddMany(graph[startVertex]);

            while (queue.Count > 0)
            {
                var edge = queue.RemoveFirst();

                if (visited[edge.V1] && !visited[edge.V2]) //  
                {
                    mstTree[edge.V1].Add(edge);
                    queue.AddMany(graph[edge.V2]);         // Union current edges with edges of the recently added vertex
                    visited[edge.V2] = true;
                }
                else if (visited[edge.V2] && !visited[edge.V1]) // turned edge check 
                {
                    mstTree[edge.V2].Add(edge);
                    queue.AddMany(graph[edge.V1]);        // Union current edges with edges of the recently added vertex
                    visited[edge.V1] = true;
                }
            }
        }
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
