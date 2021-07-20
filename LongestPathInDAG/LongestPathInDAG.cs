using System;
using System.Collections.Generic;
using System.Linq;

namespace LongestPathInDAG
{
    class LongestPathInDAG
    {
        // This program works only on Directed Acyclic Graphs(DAG (edges are pointing by definition V1 -> V2))
        // Finds longest distance(path with heaviest weight) from given vertex to all other vertices
        // For this problem vertices start from 1 so all array collections are [vCount + 1]

        static int start;
        static int final;
        static int vCount;
        static int eCount;
        static int[] prev;                             // memo for parents index -> vertex, value -> parent of this vertex  
        static double[] weights;                       // memo for weights from start v to current v with initial values negative infinity
        static Stack<int> sorted;                      // topologicaly sorted
        static HashSet<int> visited;                   // memo for visited vertices
        static int[] predecessors;                     // index -> vertex, value -> predecessors(dependencies) count
        static List<Edge>[] graph;                     // index -> vertex, value -> edges of this vertex
        static void Main()
        {
            ProceedInput();

            InitializeCollections();

            GetPredecessors();

            TopoSortDFS(GetFirstVertWithNoParents());  // pass first vertex with no dependencies (result in sorted)

            foreach (var vertex in sorted)             // topological sorted vertices
            {
                foreach (var edge in graph[vertex])
                    if (weights[edge.V2] < weights[edge.V1] + edge.Weight)           // (triggers on -infinity) and checks for havier weight
                    {
                        weights[edge.V2] = weights[edge.V1] + edge.Weight;
                        prev[edge.V2] = edge.V1;
                    }
            }

            Console.WriteLine(weights[final]);
            // Console.WriteLine(string.Join(" ", ReconstructPath()));  // program hangs when trying to reconstruct path
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

        static int GetFirstVertWithNoParents()
        {
            for (int i = 1; i < predecessors.Length; i++)                   // vertices start from 1...n
                if (predecessors[i] == 0)
                    return i;

            return 0;
        }

        static void TopoSortDFS(int vertex)
        {
            if (visited.Contains(vertex))
                return;

            visited.Add(vertex);

            foreach (var edge in graph[vertex])
                TopoSortDFS(edge.V2);

            sorted.Push(vertex);
        }

        static void InitializeCollections()
        {
            sorted = new Stack<int>();
            visited = new HashSet<int>();
            predecessors = new int[vCount + 1];
            prev = Enumerable.Repeat(-1, vCount + 1).ToArray();
            weights = Enumerable.Repeat(double.NegativeInfinity, vCount + 1).ToArray();
            weights[start] = 0;
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());
            eCount = int.Parse(Console.ReadLine());

            graph = new List<Edge>[vCount + 1];
            for (int i = 0; i < graph.Length; i++)
                graph[i] = new List<Edge>();

            for (int i = 0; i < eCount; i++)
            {
                var edgeData = Console.ReadLine().Split().Select(int.Parse).ToArray();

                var from = edgeData[0];
                var to = edgeData[1];
                var weight = edgeData[2];

                graph[from].Add(new Edge(from, to, weight));
            }

            start = int.Parse(Console.ReadLine());
            final = int.Parse(Console.ReadLine());
        }

        static void GetPredecessors()    // For each vertex gets number of predecessors (TopoSortDFS requires vertex with no dependencies)
        {
            foreach (var edge in graph.SelectMany(edges => edges)) 
                predecessors[edge.V2]++; // increase for child vertex predecessors count
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
