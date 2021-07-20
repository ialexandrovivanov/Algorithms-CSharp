using System;
using System.Collections.Generic;
using System.Linq;

namespace StronglyConnectedComponents
{
    // This program find all SCC(strongly connected components) in a Direced Graph using Kosaraju-Sharir algorithm 
    // SCC is a subgraph where there is a path from any choosen vertex to any other and from any other to the choosen one 
    // Problems which are sloved with this algorithm are: 
    // Find groups of people who are more closely related in a huge set of data (facebook friends recommendations)
    // Identifying group of people for advertising
    // Finding areas without exit in a graph
    // Time complexity: O(V+E)

    class StronglyConnectedComponents
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
            ProceedInput();                     // fills the graph and the reversed graph

            for (int i = 0; i < vCount; i++)          
            {
                if (visited[i])
                    continue;

                DFS(graph, visited, topologicalSorted, i);          // gets all vertices topological sorted using DFS (in topologicalSorted stack)
            }

            visited = new bool[vCount];                             // clears visited temp array (for reversed topological sorting)

            while (topologicalSorted.Count != 0)                    // foreach vertex of reversed topological sorting
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

        static void DFS(List<int>[] source, bool[] visited, Stack<int> subgraph, int vertex)
        {
            if (visited[vertex])
                return;

            visited[vertex] = true;

            foreach (var child in source[vertex])
                DFS(source, visited, subgraph, child);

            subgraph.Push(vertex);
        }

        static void ProceedInput()
        {
            vCount = int.Parse(Console.ReadLine());
            lCount = int.Parse(Console.ReadLine());

            InitializeCollections();                          // initialize collections using input vCount parameter

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
            topologicalSorted = new Stack<int>();
            visited = new bool[vCount];
            graph = new List<int>[vCount];
            reversedGraph = new List<int>[vCount];
            allSubgraphs = new List<Stack<int>>();

            for (int i = 0; i < vCount; i++)
                graph[i] = new List<int>();

            for (int i = 0; i < vCount; i++)
                reversedGraph[i] = new List<int>();
        }

        static void PrintAllSubgraphs()
        {
            Console.WriteLine("Strongly Connected Components:");
            foreach (var subgraph in allSubgraphs)
                Console.WriteLine($"{{{string.Join(", ", subgraph)}}}");
        }
    }
}
