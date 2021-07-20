using System;
using System.Collections.Generic;
using System.Linq;

namespace DataTransfer
{
    class DataTransfer
    {
        static int start;       // start vertex
        static int final;       // end vertex
        static int vCount;      // count vertices
        static int eCount;      // count edges
        static int minFlow;
        static int maxFlow;
        static int[,] graph;    // value on [x, y] is edge weight from x to y vertices
        static int[] parents;

        static void Main()
        {
            ProceedInput();

            parents = new int[vCount];
            Array.Fill(parents, -1);

            // First find path from start to final vertices
            var hasPath = BFS(start, final);        // Find an 'augmenting path' from start to final using BFS (saved in parents)

            if (hasPath)
            {
                ReconstructPath();
                PrintMaxFlow();
            }
        }

        static bool BFS(int start, int final)
        {
            var visited = new bool[vCount];
            var queue = new Queue<int>();

            visited[start] = true;
            queue.Enqueue(start);

            while (queue.Count > 0)                 // BFS traverse
            {
                var vert = queue.Dequeue();

                for (int child = 0; child < graph.GetLength(1); child++)
                {
                    if (!visited[child] && graph[vert, child] > 0)
                    {
                        queue.Enqueue(child);
                        visited[child] = true;
                        parents[child] = vert;
                    }
                }
            }

            return visited[final];
        }

        static void ReconstructPath()               // Reconstruct the 'augmenting path' using parents
        {
            int to;
            int from;
            maxFlow = 0;

            while (BFS(start, final))
            {
                minFlow = int.MaxValue;

                to = final;
                from = parents[final];

                while (from != -1 && to != -1)
                {
                    minFlow = Math.Min(minFlow, graph[from, to]);   // Set minFlow as the smallest capacity c in the augmenting path

                    to = parents[to];                               // step up in the parents memo
                    from = parents[to];
                }

                ModifyGraph();                                      // Modify the capacities of the edges in the path

                maxFlow += minFlow;                                 // Add the flow of the path to the maximum flow
            }
        }

        static void PrintMaxFlow()
        {
            Console.WriteLine(maxFlow);
        }

        static void ModifyGraph()  // Find what is left in the path after runnig the max path capacity(minFlow)
        {
            var to = final;
            var from = parents[final];

            while (from != -1 && to != -1)
            {
                graph[from, to] -= minFlow;  // What is left as capacity in the path after running the max path capacity(minFlow)

                to = parents[to];            // step up in parents children hierarchy
                from = parents[to];
            }
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());
            eCount = int.Parse(Console.ReadLine());
            var dataSatrtFinal = Console.ReadLine().Split();

            start = int.Parse(dataSatrtFinal[0]);
            final = int.Parse(dataSatrtFinal[1]);

            graph = new int[vCount, vCount];    // matrix[x, y] -> edge capacity between vertices x and y

            for (int i = 0; i < eCount; i++)    // iterate count of edges times to get edge data as input
            {
                var dataEdges = Console.ReadLine().Split().Select(int.Parse).ToArray();
                var from = dataEdges[0];
                var to = dataEdges[1];
                var weight = dataEdges[2];

                graph[from, to] = weight;      
            }
        }
    }
}
