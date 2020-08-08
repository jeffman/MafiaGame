using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MafiaGame.Engine
{
    public class Resolution
    {
        public static Resolution Empty { get; } = new Resolution(
            Enumerable.Empty<Player>(),
            Enumerable.Empty<Player>(),
            Enumerable.Empty<Redirection>());

        public IReadOnlyCollection<Player> BlockedPlayers { get; }
        public IReadOnlyCollection<Player> ProtectedPlayers { get; }
        public IReadOnlyCollection<Redirection> Redirections { get; }

        public Resolution(
            IEnumerable<Player> blocked,
            IEnumerable<Player> @protected,
            IEnumerable<Redirection> redirections)
        {
            BlockedPlayers = blocked.ToImmutableHashSet();
            ProtectedPlayers = @protected.ToImmutableHashSet();
            Redirections = redirections.ToImmutableHashSet();
        }

        public Player GetActualTarget(Player intended)
        {
            return Redirections.FirstOrDefault(r => r.IntendedTarget == intended)?.ActualTarget ?? intended;
        }

        public bool IsBlocked(Player player) => BlockedPlayers.Contains(player);
        public bool IsProtected(Player player) => ProtectedPlayers.Contains(player);
    }
}
