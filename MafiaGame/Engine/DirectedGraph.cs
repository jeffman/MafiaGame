using MafiaGame.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine
{
    /// <summary>
    /// Represents an arbitrary directed graph.
    ///
    /// The public methods of this class are not thread-safe.
    /// </summary>
    public sealed class DirectedGraph<T> where T : notnull
    {
        private class Node
        {
            public T Value { get; }
            public Dictionary<T, Edge> OutgoingEdges { get; } = new Dictionary<T, Edge>();
            public Dictionary<T, Edge> IncomingEdges { get; } = new Dictionary<T, Edge>();

            public Node(T value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return $"[{Value}]";
            }
        }

        private class Edge
        {
            public T Value { get; }

            public Edge(T value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return $"{Value}";
            }
        }

        private readonly Dictionary<T, Node> nodes = new Dictionary<T, Node>();

        public IEnumerable<T> Values => nodes.Keys;
        public int Count => nodes.Count;
        public int EdgeCount => nodes.Sum(n => n.Value.OutgoingEdges.Count);
        public IEnumerable<(T from, T to)> Edges => nodes.Values.SelectMany(f => f.OutgoingEdges.Values.Select(t => (f.Value, t.Value)));

        public DirectedGraph() : this (Enumerable.Empty<T>()) { }

        public DirectedGraph(IEnumerable<T> initialValues)
        {
            foreach (var value in initialValues)
            {
                AddValue(value);
            }
        }

        public DirectedGraph(DirectedGraph<T> copyFrom)
        {
            foreach (var edge in copyFrom.Edges)
            {
                AddEdge(edge.from, edge.to);
            }
        }

        public bool Contains(T value)
        {
            return nodes.ContainsKey(value);
        }

        public bool ContainsOutgoingEdge(T from, T to)
        {
            if (!nodes.TryGetValue(from, out var fromNode))
                return false;

            return fromNode.OutgoingEdges.ContainsKey(to);
        }

        /// <summary>
        /// Returns all outgoing edges from <paramref name="value"/>.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown if <paramref name="value"/> doesn't exist in the graph.</exception>
        public IEnumerable<T> GetOutgoingEdges(T value)
        {
            if (!nodes.TryGetValue(value, out var node))
                throw new KeyNotFoundException();

            return node.OutgoingEdges.Keys.ToArray();
        }

        /// <summary>
        /// Returns all incoming edges to <paramref name="value"/>.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown if <paramref name="value"/> doesn't exist in the graph.</exception>
        public IEnumerable<T> GetIncomingEdges(T value)
        {
            if (!nodes.TryGetValue(value, out var node))
                throw new KeyNotFoundException();

            return node.IncomingEdges.Keys.ToArray();
        }

        /// <summary>
        /// Adds an edge from <paramref name="fromValue"/> to <paramref name="toValue"/>.
        ///
        /// If either value doesn't exist, it is implicitly created.
        ///
        /// If the given edge already exists, no modification is made.
        /// </summary>
        public void AddEdge(T fromValue, T toValue)
        {
            if (!nodes.TryGetValue(fromValue, out var fromNode))
            {
                fromNode = new Node(fromValue);
                nodes.Add(fromValue, fromNode);
            }

            if (!nodes.TryGetValue(toValue, out var toNode))
            {
                toNode = new Node(toValue);
                nodes.Add(toValue, toNode);
            }

            if (!fromNode.OutgoingEdges.ContainsKey(toValue))
            {
                var outgoingEdge = new Edge(toValue);
                fromNode.OutgoingEdges.Add(toValue, outgoingEdge);
            }

            if (!toNode.IncomingEdges.ContainsKey(fromValue))
            {
                var incomingEdge = new Edge(fromValue);
                toNode.IncomingEdges.Add(fromValue, incomingEdge);
            }
        }

        /// <summary>
        /// Adds <paramref name="value"/> to the graph.
        ///
        /// If the value already exists, no modification is made.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="value"/> was added, <c>false</c> if <paramref name="value"/> already exists.</returns>
        public bool AddValue(T value)
        {
            if (nodes.TryGetValue(value, out var node))
                return false;

            nodes.Add(value, new Node(value));
            return true;
        }

        /// <summary>
        /// Removes an edge from the graph.
        ///
        /// If either <paramref name="fromValue"/> or <paramref name="toValue"/> doesn't exist, or if no edge exists between them, no modification is made.
        /// </summary>
        /// <returns><c>true</c> if an edge was removed, <c>false</c> if no modification was made.</returns>
        public bool RemoveEdge(T fromValue, T toValue)
        {
            if (!nodes.TryGetValue(fromValue, out var fromNode))
                return false;

            if (!nodes.TryGetValue(toValue, out var toNode))
                return false;

            if (!fromNode.OutgoingEdges.TryGetValue(toValue, out var outgoingEdge))
                return false;

            // It should be impossible for the outgoing edge to exist but the incoming edge to not exist
            Trace.Assert(toNode.IncomingEdges.TryGetValue(fromValue, out var incomingEdge));

            fromNode.OutgoingEdges.Remove(toValue);
            toNode.IncomingEdges.Remove(fromValue);

            return true;
        }

        /// <summary>
        /// Removes <paramref name="value"/> from the graph.
        ///
        /// All edges to and from <paramref name="value"/> are removed.
        ///
        /// If <paramref name="value"/> doesn't exist in the graph, no modifications are performed.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="value"/> was removed, <c>false</c> if no modifications were performed.</returns>
        public bool RemoveValue(T value)
        {
            if (!nodes.Remove(value, out var node))
                return false;

            foreach (var outgoingEdge in node.OutgoingEdges.Values)
            {
                Trace.Assert(nodes.TryGetValue(outgoingEdge.Value, out var outgoingNode));
                Trace.Assert(outgoingNode!.IncomingEdges.Remove(value));
            }

            foreach (var incomingEdge in node.IncomingEdges.Values)
            {
                Trace.Assert(nodes.TryGetValue(incomingEdge.Value, out var incomingNode));
                Trace.Assert(incomingNode!.OutgoingEdges.Remove(value));
            }

            return true;
        }

        /// <summary>
        /// Enumerates all values with no outgoing edges.
        /// </summary>
        public IEnumerable<T> GetLeafValues()
        {
            return nodes.Values
                .Where(n => n.OutgoingEdges.Count == 0)
                .Select(n => n.Value);
        }

        /// <summary>
        /// Returns the strongly connected components of the graph.
        ///
        /// Represented by a supergraph where each node is a strongly connected component.
        /// </summary>
        public DirectedGraph<HashSet<T>> GetStronglyConnectedComponents()
        {
            var unvisited = new HashSet<Node>(nodes.Values);
            var traversal = new LinkedList<Node>();
            var assigned = new HashSet<Node>();
            var components = new Dictionary<T, HashSet<T>>(); // key is root

            foreach (var node in nodes.Values)
            {
                Visit(node);
            }

            foreach (var node in traversal)
            {
                Assign(node, node);
            }

            var superGraph = new DirectedGraph<HashSet<T>>(components.Values);

            foreach (var component in components.Values)
            {
                foreach (var otherComponent in components.Values.Except(component))
                {
                    if (component.Any(v => otherComponent.Any(w => ContainsOutgoingEdge(v, w))))
                    {
                        superGraph.AddEdge(component, otherComponent);
                    }
                }
            }

            return superGraph;

            void Visit(Node u)
            {
                if (unvisited.Contains(u))
                {
                    unvisited.Remove(u);
                    foreach (var outgoingNode in u.OutgoingEdges.Values)
                    {
                        Visit(nodes[outgoingNode.Value]);
                    }
                    traversal.AddFirst(u);
                }
            }

            void Assign(Node u, Node root)
            {
                if (!assigned.Contains(u))
                {
                    if (!components.TryGetValue(root.Value, out var component))
                    {
                        component = new HashSet<T>();
                        components.Add(root.Value, component);
                    }
                    component.Add(u.Value);
                    assigned.Add(u);
                    foreach (var incomingNode in u.IncomingEdges.Values)
                    {
                        Assign(nodes[incomingNode.Value], root);
                    }
                }
            }
        }

        public void Clear()
        {
            nodes.Clear();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var node in nodes.Values)
            {
                builder.Append(node.Value);

                if (node.OutgoingEdges.Count > 0)
                {
                    builder.Append(" -> ");
                    builder.Append(string.Join(", ", node.OutgoingEdges
                        .Select(e => e.Key.ToString())
                        .OrderBy(s => s)));
                }

                builder.AppendLine();
            }

            return builder.ToString().TrimEnd();
        }
    }
}
