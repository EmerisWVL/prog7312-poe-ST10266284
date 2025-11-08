using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Custom Graph implementation with Minimum Spanning Tree algorithms
    /// Used for service request relationship mapping and optimization
    /// </summary>
    public class Graph<T> where T : IComparable<T>
    {
        public class GraphEdge : IComparable<GraphEdge>
        {
            public T Source { get; set; }
            public T Destination { get; set; }
            public int Weight { get; set; }

            public int CompareTo(GraphEdge other)
            {
                return Weight.CompareTo(other.Weight);
            }
        }

        private Dictionary<T, List<(T neighbor, int weight)>> adjacencyList;
        private List<GraphEdge> edges;

        public int VertexCount => adjacencyList.Count;
        public int EdgeCount => edges.Count;

        public Graph()
        {
            adjacencyList = new Dictionary<T, List<(T, int)>>();
            edges = new List<GraphEdge>();
        }

        public void AddVertex(T vertex)
        {
            if (!adjacencyList.ContainsKey(vertex))
            {
                adjacencyList[vertex] = new List<(T, int)>();
            }
        }

        public void AddEdge(T source, T destination, int weight = 1)
        {
            AddVertex(source);
            AddVertex(destination);

            adjacencyList[source].Add((destination, weight));
            adjacencyList[destination].Add((source, weight));

            edges.Add(new GraphEdge { Source = source, Destination = destination, Weight = weight });
        }

        public List<T> BreadthFirstSearch(T startVertex)
        {
            var visited = new List<T>();
            var queue = new Queue<T>();
            var visitedSet = new HashSet<T>();

            visitedSet.Add(startVertex);
            queue.Enqueue(startVertex);

            while (queue.Count > 0)
            {
                T current = queue.Dequeue();
                visited.Add(current);

                foreach (var (neighbor, _) in adjacencyList[current])
                {
                    if (!visitedSet.Contains(neighbor))
                    {
                        visitedSet.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return visited;
        }

        public List<T> DepthFirstSearch(T startVertex)
        {
            var visited = new List<T>();
            var visitedSet = new HashSet<T>();
            DFSRecursive(startVertex, visited, visitedSet);
            return visited;
        }

        private void DFSRecursive(T vertex, List<T> visited, HashSet<T> visitedSet)
        {
            visited.Add(vertex);
            visitedSet.Add(vertex);

            foreach (var (neighbor, _) in adjacencyList[vertex])
            {
                if (!visitedSet.Contains(neighbor))
                {
                    DFSRecursive(neighbor, visited, visitedSet);
                }
            }
        }

        // Prim's Algorithm for Minimum Spanning Tree
        public List<GraphEdge> PrimMinimumSpanningTree()
        {
            if (EdgeCount == 0) return new List<GraphEdge>();

            var mst = new List<GraphEdge>();
            var visited = new HashSet<T>();
            var priorityQueue = new MinHeap<GraphEdge>();

            // Start with first vertex
            T startVertex = adjacencyList.Keys.First();
            visited.Add(startVertex);

            // Add all edges from start vertex to priority queue
            foreach (var (neighbor, weight) in adjacencyList[startVertex])
            {
                priorityQueue.Insert(new GraphEdge { Source = startVertex, Destination = neighbor, Weight = weight });
            }

            while (!priorityQueue.IsEmpty && visited.Count < VertexCount)
            {
                GraphEdge minEdge = priorityQueue.ExtractMin();

                if (visited.Contains(minEdge.Destination)) continue;

                visited.Add(minEdge.Destination);
                mst.Add(minEdge);

                // Add all edges from the new vertex to the priority queue
                foreach (var (neighbor, weight) in adjacencyList[minEdge.Destination])
                {
                    if (!visited.Contains(neighbor))
                    {
                        priorityQueue.Insert(new GraphEdge { Source = minEdge.Destination, Destination = neighbor, Weight = weight });
                    }
                }
            }

            return mst;
        }

        // Kruskal's Algorithm for Minimum Spanning Tree
        public List<GraphEdge> KruskalMinimumSpanningTree()
        {
            var mst = new List<GraphEdge>();
            var disjointSet = new DisjointSet<T>();

            // Initialize disjoint set
            foreach (var vertex in adjacencyList.Keys)
            {
                disjointSet.MakeSet(vertex);
            }

            // Sort edges by weight
            var sortedEdges = edges.OrderBy(e => e.Weight).ToList();

            foreach (var edge in sortedEdges)
            {
                if (disjointSet.FindSet(edge.Source).CompareTo(disjointSet.FindSet(edge.Destination)) != 0)
                {
                    mst.Add(edge);
                    disjointSet.Union(edge.Source, edge.Destination);
                }
            }

            return mst;
        }

        public int CalculateTotalWeight(List<GraphEdge> edges)
        {
            return edges.Sum(e => e.Weight);
        }

        public void PrintGraph()
        {
            foreach (var vertex in adjacencyList)
            {
                Console.Write($"{vertex.Key}: ");
                foreach (var (neighbor, weight) in vertex.Value)
                {
                    Console.Write($"{neighbor}({weight}) ");
                }
                Console.WriteLine();
            }
        }

        // Helper method to get all vertices
        public List<T> GetVertices()
        {
            return adjacencyList.Keys.ToList();
        }

        // Helper method to get all edges
        public List<GraphEdge> GetEdges()
        {
            return new List<GraphEdge>(edges);
        }
    }

    // Disjoint Set implementation for Kruskal's algorithm
    public class DisjointSet<T> where T : IComparable<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node Parent { get; set; }
            public int Rank { get; set; }

            public Node(T data)
            {
                Data = data;
                Parent = this;
                Rank = 0;
            }
        }

        private Dictionary<T, Node> nodes = new Dictionary<T, Node>();

        public void MakeSet(T data)
        {
            if (!nodes.ContainsKey(data))
            {
                nodes[data] = new Node(data);
            }
        }

        public T FindSet(T data)
        {
            if (!nodes.ContainsKey(data))
                throw new ArgumentException("Data not found in any set");

            return FindSet(nodes[data]).Data;
        }

        private Node FindSet(Node node)
        {
            if (node.Parent != node)
            {
                node.Parent = FindSet(node.Parent);
            }
            return node.Parent;
        }

        public void Union(T data1, T data2)
        {
            Node root1 = FindSet(nodes[data1]);
            Node root2 = FindSet(nodes[data2]);

            if (root1.Data.CompareTo(root2.Data) == 0) return;

            if (root1.Rank < root2.Rank)
            {
                root1.Parent = root2;
            }
            else if (root1.Rank > root2.Rank)
            {
                root2.Parent = root1;
            }
            else
            {
                root2.Parent = root1;
                root1.Rank++;
            }
        }
    }
}