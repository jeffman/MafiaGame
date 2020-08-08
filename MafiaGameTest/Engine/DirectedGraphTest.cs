using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MafiaGame.Engine;
using System.Linq;

namespace MafiaGameTest.Engine
{
    public class DirectedGraphTest
    {
        private readonly DirectedGraph<string> graph = new DirectedGraph<string>();

        private string GetIndependentNodesString()
        {
            return string.Join("; ", graph.GetLeafValues().OrderBy(n => n)
                .Select(n =>
                    $"{n} <- {string.Join(", ", graph.GetIncomingEdges(n).OrderBy(s => s))}"));
        }

        private string GetGraphString() => graph.ToString().Replace(Environment.NewLine, "; ");

        private string Join(IEnumerable<string> values)
        {
            return string.Join(", ", values.OrderBy(v => v));
        }

        private static bool SetEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstSet = new HashSet<T>(first);
            var secondSet = new HashSet<T>(second);
            if (firstSet.Count != secondSet.Count)
                return false;
            return firstSet.Intersect(secondSet).Count() == firstSet.Count;
        }

        [Fact]
        public void AddOneDependency()
        {
            graph.AddEdge("A", "B");
            Assert.Equal("A -> B; B", GetGraphString());
            Assert.Equal(2, graph.Count);
            Assert.Equal(1, graph.EdgeCount);
            Assert.Equal("B <- A", GetIndependentNodesString());
        }

        [Fact]
        public void AddSameDependencyTwice()
        {
            graph.AddEdge("A", "B");
            graph.AddEdge("A", "B");
            Assert.Equal("A -> B; B", GetGraphString());
            Assert.Equal(2, graph.Count);
            Assert.Equal(1, graph.EdgeCount);
            Assert.Equal("B <- A", GetIndependentNodesString());
        }

        [Fact]
        public void AddThenRemoveDependency()
        {
            graph.AddEdge("A", "B");
            Assert.True(graph.RemoveEdge("A", "B"));
            Assert.Equal("A; B", GetGraphString());
            Assert.Equal(2, graph.Count);
            Assert.Equal(0, graph.EdgeCount);
        }

        [Fact]
        public void RemoveNonExistentDependency()
        {
            Assert.False(graph.RemoveEdge("A", "B"));
        }

        [Fact]
        public void AddSelfDependency()
        {
            graph.AddEdge("A", "A");
            Assert.Equal("A -> A", GetGraphString());
            Assert.Equal(1, graph.Count);
            Assert.Equal(1, graph.EdgeCount);
            Assert.Equal("", GetIndependentNodesString());
        }

        [Fact]
        public void AddSelfDependencyTwice()
        {
            graph.AddEdge("A", "A");
            graph.AddEdge("A", "A");
            Assert.Equal("A -> A", GetGraphString());
            Assert.Equal(1, graph.Count);
            Assert.Equal(1, graph.EdgeCount);
        }

        [Fact]
        public void AddThenRemoveSelfDependency()
        {
            graph.AddEdge("A", "A");
            Assert.True(graph.RemoveEdge("A", "A"));
            Assert.Equal("A", GetGraphString());
            Assert.Equal(1, graph.Count);
            Assert.Equal(0, graph.EdgeCount);
        }

        [Fact]
        public void IndependentNodes()
        {
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("D", "E");
            graph.AddEdge("F", "C");
            Assert.Equal("C <- B, F; E <- D", GetIndependentNodesString());
        }

        [Fact]
        public void CircularDependency()
        {
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "A");
            Assert.Equal(2, graph.Count);
            Assert.Equal(2, graph.EdgeCount);
            Assert.Empty(graph.GetLeafValues());
        }

        [Fact]
        public void GetDependenciesOf()
        {
            graph.AddEdge("A", "B");
            graph.AddEdge("A", "C");
            graph.AddEdge("C", "D");
            Assert.Equal("B, C", Join(graph.GetOutgoingEdges("A")));
        }

        [Fact]
        public void GetDependenciesOn()
        {
            graph.AddEdge("A", "B");
            graph.AddEdge("C", "B");
            graph.AddEdge("A", "D");
            Assert.Equal("A, C", Join(graph.GetIncomingEdges("B")));
        }

        [Fact]
        public void RemoveNode()
        {
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            Assert.True(graph.RemoveValue("C"));
            Assert.Equal("A -> B; B", GetGraphString());
            Assert.Equal(2, graph.Count);
            Assert.Equal(1, graph.EdgeCount);
        }

        [Fact]
        public void RemoveNodeWithDependencies()
        {
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            Assert.Throws<InvalidOperationException>(() => graph.RemoveValue("B"));
        }

        [Fact]
        public void GetStronglyConnectedComponents()
        {
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("C", "D");
            graph.AddEdge("D", "E");
            graph.AddEdge("E", "C");
            graph.AddEdge("C", "A");
            graph.AddEdge("B", "F");
            graph.AddEdge("D", "F");
            graph.AddEdge("F", "G");
            graph.AddEdge("G", "H");
            graph.AddEdge("H", "I");
            graph.AddEdge("F", "I");
            graph.AddEdge("H", "J");
            graph.AddEdge("I", "J");
            graph.AddEdge("I", "G");
            graph.AddEdge("F", "K");
            graph.AddEdge("K", "L");
            graph.AddEdge("K", "J");
            graph.AddEdge("L", "K");
            graph.AddEdge("M", "N");
            graph.AddEdge("N", "O");
            graph.AddEdge("O", "P");
            graph.AddEdge("P", "M");
            graph.AddEdge("N", "P");
            graph.AddEdge("O", "K");
            graph.AddEdge("D", "N");
            graph.AddEdge("E", "M");
            graph.AddValue("X");

            var superGraph = graph.GetStronglyConnectedComponents();
            var components = superGraph
                .Values
                .OrderBy(c => c.Min())
                .ToArray();

            Assert.Equal(7, components.Length);
            Assert.True(SetEqual(components[0], new[] { "A", "B", "C", "D", "E" }));
            Assert.True(SetEqual(components[1], new[] { "F" }));
            Assert.True(SetEqual(components[2], new[] { "G", "H", "I" }));
            Assert.True(SetEqual(components[3], new[] { "J" }));
            Assert.True(SetEqual(components[4], new[] { "K", "L" }));
            Assert.True(SetEqual(components[5], new[] { "M", "N", "O", "P" }));
            Assert.True(SetEqual(components[6], new[] { "X" }));
            Assert.True(SetEqual(superGraph.GetOutgoingEdges(components[0]), new[] { components[1], components[5] }));
            Assert.True(SetEqual(superGraph.GetOutgoingEdges(components[1]), new[] { components[2], components[4] }));
            Assert.True(SetEqual(superGraph.GetOutgoingEdges(components[2]), new[] { components[3] }));
            Assert.True(SetEqual(superGraph.GetOutgoingEdges(components[3]), new IEnumerable<string>[] { }));
            Assert.True(SetEqual(superGraph.GetOutgoingEdges(components[4]), new[] { components[3] }));
            Assert.True(SetEqual(superGraph.GetOutgoingEdges(components[5]), new[] { components[4] }));
            Assert.True(SetEqual(superGraph.GetOutgoingEdges(components[6]), new IEnumerable<string>[] { }));
        }

        [Fact]
        public void GetStronglyConnectedComponentsPerformance()
        {
            var bigGraph = new DirectedGraph<object>();
            foreach (var _ in Enumerable.Range(0, 1000))
            {
                bigGraph.AddValue(new object());
            }

            foreach (var a in bigGraph.Values)
            {
                foreach (var b in bigGraph.Values)
                {
                    bigGraph.AddEdge(a, b);
                }
            }

            var superGraph = bigGraph.GetStronglyConnectedComponents();
        }
    }
}
