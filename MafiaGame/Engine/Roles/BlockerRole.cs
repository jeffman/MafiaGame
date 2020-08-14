using MafiaGame.Engine.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public class BlockerRole : Role
    {
        public override Ability Ability => Ability.Blocker;
        public override Alignment Alignment { get; }

        public BlockerRole(Alignment alignment) : base("Blocker")
        {
            Alignment = alignment;
        }

        public override void OnRegisterNightActionDependencies(GameState state, NightAction action, DependencyResolver resolver)
        {
            Trace.Assert(action.Targets.Count == 1);

            // If blocker B blocks player P, then P's night action depends on B
            resolver.Dependencies.AddEdge(action.Targets.First(), action.Source);
        }

        public override void OnResolveNightAction(GameState state, NightAction action, NightResolver resolver)
        {
            Trace.Assert(action.Targets.Count == 1);
            if (!resolver.IsBlocked(action.Source))
                resolver.Block(resolver.GetActualTarget(action.Targets.First()));
        }
    }
}
