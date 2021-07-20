using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace EmergencyPlan
{
    class EmergencyPlan
    {
        static int vCount;
        static int eCount;
        static int timeToLeave;
        static int[] exitVertices;
        static Comparer<int> comparer = Comparer<int>.Create((f, s) =>
          weights[f].CompareTo(weights[s]));                 // compares two vertices by weight from start using weights collection
        static List<Edge>[] graph;                           // key -> vertex, value list of edges
        static OrderedBag<int> queue;                        // keeps vertices sorted descending by weight from start to current vertex
        static int[] weights;                                // keeps best weights from start to current vertex (index)
        static int[] bestTimes;                              // index -> room, value best time
        static void Main()
        {

            ProceedInput();

            bestTimes = Enumerable.Repeat(int.MaxValue, vCount).ToArray();
            var rooms = Enumerable.Range(0, vCount).Except(exitVertices);

            foreach (var room in rooms)
            {
                InitializeCollections();

                foreach (var exit in exitVertices)
                {
                    queue.Add(room);
                    weights[room] = 0;
                     
                    while (queue.Count > 0)                             // BFS
                    {
                        var currentRoom = queue.RemoveFirst();          // first is the vertex with smallest weight from start
                        var edges = graph[currentRoom];                 // list of all currentV edges

                        if (currentRoom == exit)                        // final is found
                            break;

                        foreach (var edge in edges)
                        {
                            var child = edge.V1 == currentRoom ? edge.V2 : edge.V1;  // get other vertex from edge then smallestV vertex

                            if (weights[child] == int.MaxValue)                      // if not saved in weights memo add to the queue  
                                queue.Add(child);

                            var currentWeight = edge.Weight + weights[currentRoom];  // get current weight up to current vertex

                            if (currentWeight < weights[child])                      // if current weight is smaller than existing record  
                            {
                                weights[child] = currentWeight;                      // update memo  
                                queue = new OrderedBag<int>(queue, comparer);        // re-sort queue with recently added child
                            }
                        }
                    }

                    var currentTime = weights[exit];

                    if (currentTime < bestTimes[room])
                        bestTimes[room] = weights[exit];
                }
            }

            PrintResult();
        }

        static void PrintResult()
        {
            for (int i = 0; i < bestTimes.Length; i++)
            {
                if (exitVertices.Contains(i))
                    continue;

                var t = TimeSpan.FromSeconds(bestTimes[i]);

                if (bestTimes[i] == int.MaxValue)
                    Console.WriteLine($"Unreachable {i} (N/A)");
                else if (bestTimes[i] <= timeToLeave)
                    Console.WriteLine($"Safe {i} ({t})");
                else
                    Console.WriteLine($"Unsafe {i} ({t})");
            }
        }

        static void InitializeCollections()
        {
            queue = new OrderedBag<int>(comparer);
            weights = Enumerable.Repeat(int.MaxValue, vCount).ToArray();
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());

            graph = new List<Edge>[vCount];
            for (int i = 0; i < vCount; i++)
                graph[i] = new List<Edge>();
           
            exitVertices = Console.ReadLine().Split().Select(int.Parse).ToArray();

            eCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < eCount; i++)
            {
                var dataEdges = Console.ReadLine().Split();

                var from = int.Parse(dataEdges[0]);
                var to = int.Parse(dataEdges[1]);
                var weight = GetTimeInSeconds(dataEdges[2]);
                var edge = new Edge(from, to, weight);

                graph[from].Add(edge);
                graph[to].Add(edge);
            }

            timeToLeave = GetTimeInSeconds(Console.ReadLine());
        }

        static int GetTimeInSeconds(string input)
        {
            var timeData = input.Split(new char[] { ':' });
            var minutes = int.Parse(timeData[0]);
            var seconds = int.Parse(timeData[1]);

            return minutes * 60 + seconds;
        }
    }

    class Edge
    {
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int Weight { get; set; }

        public Edge(int firstVertex, int SecondVertex, int edgeWeight)
        {
            V1 = firstVertex;
            V2 = SecondVertex;
            Weight = edgeWeight;
        }
    }
}
