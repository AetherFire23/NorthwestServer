using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using WebAPI.GameState_Management;
using WebAPI.Models;

namespace WebAPI
{
    public class PlayerContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Skill> Skills { get; set; }
        public DbSet<PrivateChatRoomParticipant> PrivateChatRooms { get; set;}
        public DbSet<PrivateInvitation> Invitations { get; set; }

        //There is constructor injection. This must exist to be configurable in Program.cs
        public PlayerContext(DbContextOptions<PlayerContext> options) : base(options)
        {
        }
    }
}
