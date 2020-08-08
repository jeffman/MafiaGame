using MafiaGame.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine.Actions
{
    public sealed class NightAction
    {
        public Player Source { get; }
        public IReadOnlyCollection<Player> Targets { get; }

        public NightAction(Player source, IEnumerable<Player> targets)
        {
            Source = source;
            Targets = targets.ToReadOnlyCollection();
        }
    }
}
