using MafiaGame.Engine.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public class BusDriverRole : Role
    {
        public override Ability Ability => Ability.BusDriver;
        public override Alignment Alignment { get; }

        public BusDriverRole(Alignment alignment) : base("Bus driver")
        {
            Alignment = alignment;
        }

        public override void OnRegisterNightActionDependencies(GameState state, NightAction action, DependencyResolver resolver)
        {
            (var first, var second) = GetValidatedTargets(action);

            // If the bus driver D targets players A and B, then *any* night action targeting A or B will be depended on by D
            // Likewise, A and B themselves depend on D, since someone could role block A or B
            resolver.Dependencies.AddEdge(first, action.Source);
            resolver.Dependencies.AddEdge(second, action.Source);

            foreach (var dependency in resolver.Dependencies.GetOutgoingEdges(first))
                resolver.Dependencies.AddEdge(action.Source, dependency);
            foreach (var dependency in resolver.Dependencies.GetOutgoingEdges(second))
                resolver.Dependencies.AddEdge(action.Source, dependency);
        }

        public override void OnResolveNightAction(GameState state, NightAction action, NightResolver resolver)
        {
            (var first, var second) = GetValidatedTargets(action);
            if (!resolver.IsBlocked(action.Source))
            {
                var firstActual = resolver.GetActualTarget(first);
                var secondActual = resolver.GetActualTarget(second);
                resolver.RedirectTarget(firstActual, secondActual);
                resolver.RedirectTarget(secondActual, firstActual);
            }
        }

        private (Player first, Player second) GetValidatedTargets(NightAction action)
        {
            Trace.Assert(action.Targets.Count == 2);
            return (action.Targets.First(), action.Targets.Skip(1).First());
        }
    }
}
