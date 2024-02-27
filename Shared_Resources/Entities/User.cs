using Shared_Resources.Interfaces;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Shared_Resources.Entities;

public class User : IEntity, ISSESubscriber
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public virtual ICollection<Player> Players { get; set; } = [];
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    public virtual ICollection<UserLobby> UserLobbies { get; set; } = [];

    [NotMapped]
    public List<Lobby> Lobbies => UserLobbies.Any() ? UserLobbies.Select(x => x.Lobby).ToList() : new List<Lobby>();

    [NotMapped]
    public List<Game> Games => Players.Any()
        ? Players.Select(x => x.Game).ToList()
        : new List<Game>();

}

public static class FunExt
{
    public static IEnumerable<T2> SelectSafely<T, T2>(this IEnumerable<T> self, Func<T, T2> selector)
    {
        if (self.Any())
        {
            IEnumerable<T2> selected = self.Select(selector);
            return selected;
        }

        return new List<T2>();
    }
}
