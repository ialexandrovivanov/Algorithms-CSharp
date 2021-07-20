using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectricalSubstationNetwork
{
    class ElectricalSubstationNetwork
    {
        static int vCount;                  // total count of the vertices
        static int lCount;                  // lines of input
        static bool[] visited;              // temp array for storing visited vertices
        static List<int>[] graph;           // graph is represented as arrays with index -> vertex and value -> list of vertices(children)
        static List<int>[] reversedGraph;   // reversedGraph is represented as arrays with index -> vertex and value -> list of vertices(children)
        static Stack<int> topologicalSorted;    // the result of DFS method (list could be used instead of stack (different order of output))
        static List<Stack<int>> allSubgraphs;   // final result (all strongly connected components(subgraphs))

        static void Main()
        {
            ProceedInput();

            for (int i = 0; i < vCount; i++)
            {
                if (visited[i])
                    continue;

                DFS(graph, visited, topologicalSorted, i);      // gets all vertices topological sorted using DFS (in topologicalSorted stack)
            }

            visited = new bool[vCount];                         // clears visited temp array (for reversed topological sorting)

            while (topologicalSorted.Count != 0)                // foreach vertex of reversed topological sorting
            {
                var vertex = topologicalSorted.Pop();

                if (visited[vertex])
                    continue;

                var subgraph = new Stack<int>();

                DFS(reversedGraph, visited, subgraph, vertex);  // foreach vertex from the reversed list apply DFS to find subgraph
                                                                // (topological sorting on reversed topological sorted list)  
                allSubgraphs.Add(subgraph);
            }

            PrintAllSubgraphs();

        }

        private static void PrintAllSubgraphs()
        {
            foreach (var sub in allSubgraphs)
                Console.WriteLine(string.Join(", ", sub));
        }

        private static void DFS(List<int>[] graph, bool[] visited, Stack<int> topologicalSorted, int vertex)
        {
            if (visited[vertex])
                return;

            visited[vertex] = true;

            foreach (var child in graph[vertex])
                DFS(graph, visited, topologicalSorted, child);

            topologicalSorted.Push(vertex);
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());
            lCount = int.Parse(Console.ReadLine());

            InitializeCollections();

            for (int i = 0; i < lCount; i++)
            {
                var data = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();

                for (int j = 1; j < data.Length; j++)
                { 
                    graph[data[0]].Add(data[j]);
                    reversedGraph[data[j]].Add(data[0]);
                }
            }
        }

        static void InitializeCollections()
        {
            visited = new bool[vCount];
            graph = new List<int>[vCount];
            topologicalSorted = new Stack<int>();
            reversedGraph = new List<int>[vCount];
            allSubgraphs = new List<Stack<int>>();

            for (int i = 0; i < vCount; i++)
                graph[i] = new List<int>();

            for (int i = 0; i < vCount; i++)
                reversedGraph[i] = new List<int>();
        }
    }
}
