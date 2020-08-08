using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public abstract class DependencyResolver
    {
        public DirectedGraph<Player> Dependencies { get; } = new DirectedGraph<Player>();

        /// <summary>
        /// Given the current dependencies, determine an order of resolution.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<Player> Resolve();
    }
}
