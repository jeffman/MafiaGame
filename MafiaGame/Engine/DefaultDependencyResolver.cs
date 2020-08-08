using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine
{
    /// <summary>
    /// Default dependency resolver.
    /// 
    /// This resolver follows these rules:
    /// 
    ///     - Independent actions resolve first. An action is independent if and only if it does
    ///       not depend on any other action being resolved first. (Equivalently, resolve
    ///       all nodes first which have no outgoing edges.)
    ///     - If there are no independent actions left to resolve:
    ///         - Identify all strongly connected components of the dependency graph. Form a graph
    ///           out of the components themselves. This graph is guaranteed to be acyclic.
    ///         - Find an independent component from this component-graph. Within that component:
    ///             - Resolve the action used by the role with the unique ability that comes highest
    ///               in priority.
    ///             - If no such unique ability exists (e.g. there are two Bus Drivers), pick one
    ///               at random and resolve it first.
    ///         - Only resolve one action in this manner at a time, then go back to the start.
    ///           By resolving such an action, it is likely that it will induce another action
    ///           to become independent, and that action should naturally be resolved next.
    /// </summary>
    public class DefaultDependencyResolver : DependencyResolver
    {
        private static Random random = new Random();
        private Random? testRandom;
        private Random Random => testRandom ?? random;

        private static readonly List<Ability> abilitiesToPrioritizeBeforeDefault = new List<Ability>
        {
            Ability.BusDriver,
            Ability.Redirector,
            Ability.Protector
        };

        private static readonly List<Ability> abilitiesToPrioritizeAfterDefault = new List<Ability>
        {
            Ability.Killer,
            Ability.AlignmentInvestigator
        };

        public DefaultDependencyResolver() { }

        internal DefaultDependencyResolver(int randomSeed)
        {
            testRandom = new Random(randomSeed);
        }

        public override IEnumerable<Player> Resolve()
        {
            // Resolution is destructive, so operate on a copy of the current dependency graph
            var dependencies = new DirectedGraph<Player>(Dependencies);

            while (dependencies.Count > 0)
            {
                // Look for independent actions
                var independentPlayer = dependencies.GetLeafValues().FirstOrDefault();
                if (independentPlayer != null)
                {
                    yield return RemoveAndReturn(independentPlayer);
                    continue;
                }

                // Get an independent strongly connected component
                var superGraph = dependencies.GetStronglyConnectedComponents();
                var independentComponent = superGraph.GetLeafValues().First();

                // Group by prioritized ability
                var playersByPriority = independentComponent
                    .GroupBy(p => GetPrioritizedAbilityIndex(p.Role.Ability))
                    .OrderBy(p => p.Key);

                var highestPriorityGroup = playersByPriority.First().ToArray();

                // If the highest priority player is unique, resolve it
                if (highestPriorityGroup.Length == 1)
                {
                    yield return RemoveAndReturn(highestPriorityGroup[0]);
                    continue;
                }

                // Otherwise, choose at random
                int randomIndex = Random.Next(highestPriorityGroup.Length);
                yield return RemoveAndReturn(highestPriorityGroup[randomIndex]);
            }

            Player RemoveAndReturn(Player player)
            {
                dependencies.RemoveValue(player);
                return player;
            }
        }

        private static int GetPrioritizedAbilityIndex(Ability ability)
        {
            if (abilitiesToPrioritizeBeforeDefault.Contains(ability))
            {
                return -abilitiesToPrioritizeBeforeDefault.Count + abilitiesToPrioritizeBeforeDefault.IndexOf(ability);
            }
            else if (abilitiesToPrioritizeAfterDefault.Contains(ability))
            {
                return abilitiesToPrioritizeAfterDefault.IndexOf(ability) + 1;
            }
            return 0;
        }
    }
}
