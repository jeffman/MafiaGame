using MafiaGame.Utility;
using MafiaGame.Engine.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine
{
    public sealed class GameState
    {
        private Phase phase = Phase.Night;

        public ICollection<Player> Players { get; } = new HashSet<Player>();
        public ICollection<Vote> Votes { get; } = new HashSet<Vote>();
        public ICollection<NightAction> PendingNightActions { get; } = new HashSet<NightAction>();
        public Resolution Resolution { get; set; } = Resolution.Empty;

        public Phase Phase
        {
            get => phase;
            set => phase = value.AsDefinedOrThrow();
        }
    }
}
