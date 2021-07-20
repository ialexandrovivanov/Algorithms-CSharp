using System;
using System.Collections.Generic;
using System.Linq;

namespace ArticulationPoint
{
    // In a connected undirected graph, an articulation point is a vertex which when removed splits the graph into several disconnected subgraphs
    // This progtram find all (cut vertices) in a certain network (graph) which are articulation points
    // This solution is using Hopcroft-Tarjan algorithm
    // Time Complexity: (O(V+E)) 
    class ArticulationPoint
    {
        static int vCount;
        static int lCount;
        static int[] depths;
        static int[] parents;
        static bool[] visited;
        static int[] lowpoints;
        static List<int> points;

        static List<int>[] graph;
        static void Main()
        {
            ProceedInput();     // Fills the graph(represented as array with index -> vertex and value -> list of vertices(children))

            InitializeCollections();    // Initializes collections with sizes defined from input data

            DFS(0, 1);          // The algorithm is using DFS to traverse vertices

            Console.WriteLine("Articulation points: " + string.Join(", ", points));
        }

        static void InitializeCollections()
        {
            points = new List<int>();
            depths = new int[vCount];
            parents = new int[vCount];
            visited = new bool[vCount];
            lowpoints = new int[vCount];

            Array.Fill(parents, -1);
        }

        static void DFS(int vertex, int depth)
        {
            visited[vertex] = true;
            depths[vertex] = depth;
            lowpoints[vertex] = depth;

            var children = 0;
            var isArticulation = false;

            foreach (var child in graph[vertex])
            {
                if (!visited[child])
                {
                    parents[child] = vertex;
                    children += 1;
                    DFS(child, depth + 1);

                    if (lowpoints[child] >= depth)
                        isArticulation = true;

                    lowpoints[vertex] = Math.Min(lowpoints[vertex], lowpoints[child]);
                }
                else if (parents[vertex] != child)
                    lowpoints[vertex] = Math.Min(lowpoints[vertex], depths[child]);
            }
            ;
            if (parents[vertex] == -1 && children > 1 ||
                parents[vertex] != -1 && isArticulation)
                points.Add(vertex);
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());
            lCount = int.Parse(Console.ReadLine());

            graph = new List<int>[vCount];

            for (int i = 0; i < vCount; i++)
                graph[i] = new List<int>();

            for (int i = 0; i < lCount; i++)
            {
                var data = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();

                for (int j = 1; j < data.Length; j++)
                    graph[data[0]].Add(data[j]);
            }
        }
    }
}
