using System;
using System.Collections.Generic;

namespace MaximumTasksAssignment
{
    // Find the maximum assignment that assigns tasks to people to complete a maximum number of tasks
    // One person can do one task and one task can be done by one person
    // Table of all people with their capabilities of doing all tasks is given 
    class MaximumTasksAssignment
    {
        static int start;       // 0
        static int final;       // vCount - 1
        static int vCount;      // total count of vertices
        static int pCount;      // count of people
        static int tCount;      // count of tasks
        static int[,] graph;    // matrix[pCount + tCount + 2, pCount + tCount + 2]
        static int[] parents;   // index -> vertex, value -> parent
        static bool[] visited;   // visited vertices when finding paths with BFS

        static void Main()
        {
            ProceedInput();     // Create and fill bipartite graph with input data

            InitializeCollections();    // Create visited and parents arrays and fill parents with - 1

            while (BFS(start, final))
            {
                var vertex = final;

                while (vertex != start)
                {
                    var parent = parents[vertex];

                    graph[vertex, parent] = 1;      // make reversed edges from path edges with value 1(to reconstruct )
                    graph[parent, vertex] = 0;      // set all original edges from the path to 0

                    vertex = parent;
                }

                InitializeCollections();
            }

            PrintResult();
        }

        static void InitializeCollections()
        {
            parents = new int[vCount];
            Array.Fill(parents, -1);

            visited = new bool[vCount];
        }

        static bool BFS(int source, int target)
        {
            visited[source] = true;

            var queue = new Queue<int>();
            queue.Enqueue(source);

            while (queue.Count != 0)
            {
                var vertex = queue.Dequeue();

                if (vertex == target)
                    return true;

                for (int child = 0; child < vCount; child++)
                {
                    if (!visited[child] && graph[vertex, child] == 1)
                    {
                        parents[child] = vertex;
                        visited[child] = true;
                        queue.Enqueue(child);
                    }
                }
            }

            return false;
        }

        static void ProceedInput()
        {
            pCount = int.Parse(Console.ReadLine());
            tCount = int.Parse(Console.ReadLine());

            vCount = pCount + tCount + 2; // needed vertices(people + tasks + 2 extra vertices for start and final)
            graph = new int[vCount, vCount];       // graph[people + tasks + 2, people + tasks+ 2,] 

            // Representation of people and tasks in the matrix 
            // S A B C 1 2 3 F                     // S -> start, A B C -> 3 people, 1 2 3 -> 3 tasks, F -> final 
            // 0 1 2 3 4 5 6 7                     // numeric representation of all vertices (people tasks start final)

            start = 0;                             // indexes
            final = vCount - 1;

            for (int pIndex = 1; pIndex <= pCount; pIndex++)            // connect start vertex(0) to all people vertices(1 2 3)
                graph[start, pIndex] = 1;
            ;
            for (int tIndex = pCount + 1; tIndex <= pCount + tCount; tIndex++)    // connect all tasks vertices(4 5 6) to final vertex(7)
                graph[tIndex, final] = 1;

            for (int pIndex = 1; pIndex <= pCount; pIndex++)
            {
                var data = Console.ReadLine();                          // data comes as string of Y and N symbols index of symbol is person

                for (int tIndex = 0; tIndex < data.Length; tIndex++)
                    if (data[tIndex] == 'Y')                            // Y -> task can be done by this person N -> cannot be done
                        graph[pIndex, pCount + 1 + tIndex] = 1;         // matrix[x, y] x -> person, y -> task(tasks indexes are 4 5 6)
            }
        }

        static void PrintResult()
        {
            for (int person = 1; person <= pCount; person++)
                for (int task = pCount + 1; task <= pCount + tCount; task++)
                    if (graph[task, person] == 1)
                    {
                        Console.WriteLine($"{(char)(64 + person)}-{task - pCount}");
                        break;
                    }
        }
    }
}
