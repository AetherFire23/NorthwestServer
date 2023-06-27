﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using WebAPI.Game_Actions;
using WebAPI.Temp_settomgs;

namespace WebAPI
{
    public class PlayerContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<PrivateChatRoomParticipant> PrivateChatRoomParticipants { get; set; }
        public DbSet<PrivateInvitation> Invitations { get; set; }
        public DbSet<AdjacentRoom> AdjacentRooms { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<GameAction> GameActions { get; set; }
        public DbSet<TriggerNotification> TriggerNotifications { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MenuNotification> MenuNotifications { get; set; }
        public DbSet<FriendPair> FriendPairs { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameSetting> GameSetting { get; set; }
        public DbSet<RoleSetting> RoleSettings { get; set; }
        public DbSet<TaskSetting> TaskSettings { get; set; }
        public DbSet<LogAccessPermissions> LogAccessPermission { get; set; }
        public DbSet<PrivateChatRoom> PrivateChatRooms { get; set; }
        public DbSet<Expedition> Expeditions { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Landmass> Landmass { get; set; }
        public DbSet<ShipState> ShipStates { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Season> Season { get; set; }

        //There is constructor injection. This must exist to be configurable in Program.cs
        public PlayerContext(DbContextOptions<PlayerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // denis says fluent api is for advanced shit
        {
            modelBuilder
                .Entity<GameAction>()
                .Property(e => e.GameActionType)
                .HasConversion(new EnumToStringConverter<GameActionType>());

            modelBuilder
                .Entity<TriggerNotification>()
                .Property(e => e.NotificationType)
                .HasConversion(new EnumToStringConverter<NotificationType>());

            modelBuilder
                .Entity<Room>()
                .Ignore(e => e.AdjacentRoomNames);

            modelBuilder
                .Entity<Room>()
                .Ignore(e => e.CardImpact);
        }
    }
}
