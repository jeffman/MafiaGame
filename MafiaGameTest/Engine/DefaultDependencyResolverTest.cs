using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;
using Xunit;

namespace MafiaGameTest.Engine
{
    public class DefaultDependencyResolverTest : PlayerBase
    {
        private DefaultDependencyResolver resolver = new DefaultDependencyResolver();

        [Fact]
        public void ResolveNoCycles()
        {
            resolver.Dependencies.AddEdge(alice, chris);
            resolver.Dependencies.AddEdge(chris, bob);
            var order = resolver.Resolve().ToArray();
            Assert.True(order.SequenceEqual(new[] { bob, chris, alice }));
        }
    }
}
