using MafiaGame.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MafiaGame.Engine
{
    public abstract class GameAction
    {
        public PlayerSlot Source { get; }
        public IReadOnlyCollection<PlayerSlot> Targets { get; }

        protected GameAction(PlayerSlot source, IEnumerable<PlayerSlot> targets)
        {
            Source = source;
            Targets = targets.ToReadOnlyCollection();
        }
    }
}
