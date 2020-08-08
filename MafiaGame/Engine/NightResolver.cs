using MafiaGame.Engine.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine
{
    public class NightResolver
    {
        private readonly HashSet<Player> protectedPlayers = new HashSet<Player>();
        private readonly HashSet<NightAction> actions = new HashSet<NightAction>();

        public NightResolver(IEnumerable<NightAction> actions)
        {
            this.actions = actions.ToHashSet();
        }

        public void Protect(Player player)
        {
            protectedPlayers.Add(player);
        }

        public Resolution Resolve(GameState state)
        {
            throw new NotImplementedException();
        }
    }
}
