using MafiaGame.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public sealed class NightAction
    {
        public Player Source { get; }
        public IReadOnlyCollection<Player> Targets { get; }

        public NightAction(Player source, params Player[] targets) : this(source, (IEnumerable<Player>)targets) { }

        public NightAction(Player source, IEnumerable<Player> targets)
        {
            Source = source;
            Targets = targets.ToReadOnlyCollection();
        }
    }
}
