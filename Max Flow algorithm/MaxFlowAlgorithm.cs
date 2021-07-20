using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxFlowAlgorithm
{
    // Max Flow Problem for weighted directed / undirected graph with capacities assigned to the edges
    // Goal: compute the maximum flow from start vertex to final vertex
    // This solution is useing Edmonds-Karp algorithm
    // Time complexity: O(VE2)

    class MaxFlowAlgorithm
    {
        static int start;       // start vertex
        static int final;       // end vertex
        static int vCount;      // count vertices
        static int minFlow;
        static int maxFlow;
        static int[,] graph;    // matrix[x, y] value on [x, y] is weight of the edge with (from vertex) x and (to vertex) y
        static int[] parents;

        static void Main()
        {
            ProceedInput();

            parents = new int[vCount];              // initial values -1 (when reconstruct path start vertex is -1 (no parent))
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
                    
                while (from != -1 && to != -1)                      // if only one is -1 start vertex is reached
                {
                    minFlow = Math.Min(minFlow, graph[from, to]);   // Set minFlow as the smallest edge capacity c in the augmenting path
                    to = parents[to];
                    from = parents[to];
                }

                ModifyGraph();                                      // Modify the capacities of the edges in the path

                maxFlow += minFlow;                                 // Add the flow of the path to the maximum flow
            }
        }

        static void ModifyGraph()  // Find what is left in the path after runnig the max path capacity(minFlow)
        {
            var to = final;
            var from = parents[final];

            while (from != -1 && to != -1)
            {
                graph[from, to] -= minFlow;  // What is left as capacity in the path after running the max path capacity(minFlow)

                to = parents[to];
                from = parents[to];
            }
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());

            graph = new int[vCount, vCount];    // matrix[x, y] -> edge capacity between vertices x and y

            for (int row = 0; row < vCount; row++)
            {
                var data = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();

                for (int col = 0; col < vCount; col++)
                    graph[row, col] = data[col];
            }

            start = int.Parse(Console.ReadLine());
            final = int.Parse(Console.ReadLine());
        }

        static void PrintMaxFlow()
        {
            Console.WriteLine($"Max flow = {maxFlow}");
        }
    }
}
