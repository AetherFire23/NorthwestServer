using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using WebAPI.Game_Actions;
using WebAPI.GameState_Management;
using WebAPI.Main_Menu.Models;
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
        public DbSet<PrivateChatRoomParticipant> PrivateChatRooms { get; set; }
        public DbSet<PrivateInvitation> Invitations { get; set; }
        public DbSet<AdjacentRoom> AdjacentRooms { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<GameAction> GameActions { get; set; }
        public DbSet<TriggerNotification> TriggerNotifications { get; set; }
        public DbSet<RoomLog> RoomLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MenuNotification> MenuNotifications { get; set; }
        public DbSet<FriendPair> FriendPairs { get; set; }

        //There is constructor injection. This must exist to be configurable in Program.cs
        public PlayerContext(DbContextOptions<PlayerContext> options) : base(options)
        {
        }
    }
}
