using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine
{
    public class NightResolver
    {
        private readonly HashSet<Player> protectedPlayers = new HashSet<Player>();
        private readonly HashSet<Player> blockedPlayers = new HashSet<Player>();
        private readonly Dictionary<Player, Player> targetRedirects = new Dictionary<Player, Player>();
        private readonly HashSet<NightResult> results = new HashSet<NightResult>();

        public void Protect(Player player)
        {
            protectedPlayers.Add(player);
        }

        public bool IsProtected(Player player)
            => protectedPlayers.Contains(player);

        public void Block(Player player)
        {
            blockedPlayers.Add(player);
        }

        public bool IsBlocked(Player player)
            => blockedPlayers.Contains(player);

        public void RedirectTarget(Player intendedTarget, Player willActuallyTarget)
        {
            targetRedirects[intendedTarget] = willActuallyTarget;
        }

        public Player GetActualTarget(Player intendedTarget)
        {
            if (targetRedirects.TryGetValue(intendedTarget, out var actualTarget))
                return actualTarget;
            return intendedTarget;
        }

        public void AddResult(NightResult result)
        {
            results.Add(result);
        }
    }
}
