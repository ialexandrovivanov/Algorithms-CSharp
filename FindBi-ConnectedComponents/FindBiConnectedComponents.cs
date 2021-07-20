using System;
using System.Collections.Generic;

namespace FindBi_ConnectedComponents
{
    // Find all bi-connected componnets (set of maximal bi-connected subgraphs) in a connected undirected graph
    // Bi-Connected component is a graph which has no articulation points in it
    // Articulation point is a vertex which when removed splits the graph into several disconnected subgraphs
    // Two bi-connected graphs can have at most one common vertex which is articulation point (but not a common edge)
    class FindBiConnectedComponents
    {
        static int vCount;        // Count of vertices in the graph
        static int eCount;        // Count of edges in the graph      
        static int[] depths;      // Depth of vertices acheived by traversing the graph using DFS algorithm (index -> vertex, value -> depth)
        static int[] parents;     // Index -> a vertex, value -> parent of the vertex
        static bool[] visited;    // Collection for DFS algorithm
        static int[] lowpoints;   // Min lowpoint of all vertices we can reach using different edge from where we came (if no any low = depth)
        static Stack<int> stack;
        static List<int>[] graph; // Index -> vertex, value -> collection of child vertices
        static List<HashSet<int>> components; // Result -> all bi-connected components

        static void Main()
        {
            ProceedInput();                   // Fill the graph with input data

            for (int vertex = 0; vertex < vCount; vertex++)
            {
                if (!visited[vertex])
                {
                    DFS(vertex, 1);

                    var component = new HashSet<int>();

                    while (stack.Count > 1)
                    {
                        var stackChild = stack.Pop();
                        var stackVertex = stack.Pop();

                        component.Add(stackChild);
                        component.Add(stackVertex);
                    }

                    components.Add(component);
                }
            }

            Console.WriteLine($"Number of bi-connected components: {components.Count}");

            //foreach (var item in components)
            //    Console.WriteLine(string.Join(" ", item));
        }

        static void DFS(int vertex, int depth)
        {
            visited[vertex] = true;
            depths[vertex] = depth;
            lowpoints[vertex] = depth;

            var childrenCount = 0;

            foreach (var child in graph[vertex])
            {
                if (!visited[child])                 // if vertex is not visited
                {
                    stack.Push(vertex);
                    stack.Push(child);

                    parents[child] = vertex;
                    childrenCount += 1;

                    DFS(child, depth + 1);

                    if (parents[vertex] == -1 && childrenCount > 1 ||
                        parents[vertex] != -1 && lowpoints[child] >= depth)
                    {
                        var component = new HashSet<int>();

                        while (true)
                        {
                            var stackChild = stack.Pop();
                            var stackVertex = stack.Pop();

                            component.Add(stackVertex);
                            component.Add(stackChild);

                            if (stackVertex == vertex &&
                                stackChild == child) // If true starting edge is reached
                                break;
                        }

                        components.Add(component);
                    }

                    lowpoints[vertex] = Math.Min(lowpoints[vertex], lowpoints[child]); // Check for backedge and get minimal lowpoint
                }
                else if (parents[vertex] != child && depths[child] < lowpoints[vertex]) // vertex is visited
                {
                    lowpoints[vertex] = depths[child];

                    stack.Push(vertex);
                    stack.Push(child);
                }
            }
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());
            eCount = int.Parse(Console.ReadLine());

            InitializeCollections();

            for (int i = 0; i < eCount; i++)
            {
                var input = Console.ReadLine().Split();  // input comes in format "0 1" (vertex child)
                var data = new int[input.Length];

                for (int j = 0; j < data.Length; j++)    // parse all strnigs from input to numbers
                    data[j] = int.Parse(input[j]);


                graph[data[0]].Add(data[1]);             // undirected grpah (vertex 0 has child 1 and vertex 1 has child 0) 
                graph[data[1]].Add(data[0]);
            }
        }

        static void InitializeCollections()
        {
            stack = new Stack<int>();
            depths = new int[vCount];
            parents = new int[vCount];
            visited = new bool[vCount];
            lowpoints = new int[vCount];
            graph = new List<int>[vCount];
            components = new List<HashSet<int>>();

            for (int i = 0; i < vCount; i++)
                graph[i] = new List<int>(); depths = new int[vCount];

            for (int i = 0; i < vCount; i++)
                parents[i] = -1;
        }
    }
}
