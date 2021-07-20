using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace BlackFriday
{
    // MST Problem
    class BlackFriday
    {
        static int vCount;
        static int eCount;
        static int[] damage;          // damage memo for each vertex (index -> vertex, value -> accumulative damage from all ligthening)
        static bool[] visited;
        static List<Edge>[] graph;
        static Comparer<Edge> comparer = Comparer<Edge>.Create((f, s) => f.Weight - s.Weight);
        static List<Edge> mstEdges = new List<Edge>();
        static HashSet<int> tree = new HashSet<int>();

        static void Main()
        {
            ProceedInput();

            InitializeCollections();

            GetMSTEdges(0);                           // spanning tree covering all paths with minimal electrical resistance (mstTree)

            Console.WriteLine(mstEdges.Sum(s => s.Weight));
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

        static void InitializeCollections()
        {
            mstEdges = new List<Edge>();
            damage = new int[vCount];
            visited = new bool[vCount];
        }


        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());
            eCount = int.Parse(Console.ReadLine());

            graph = new List<Edge>[vCount];
            for (int i = 0; i < vCount; i++)
                graph[i] = new List<Edge>();

            for (int i = 0; i < eCount; i++)
            {
                var edgeData = Console.ReadLine().Split();

                var from = int.Parse(edgeData[0]);
                var to = int.Parse(edgeData[1]);
                var weight = int.Parse(edgeData[2]);

                var edge = new Edge(from, to, weight);

                graph[from].Add(edge);
                graph[to].Add(edge);
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
