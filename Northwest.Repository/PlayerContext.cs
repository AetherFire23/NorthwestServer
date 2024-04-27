using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Northwest.Persistence.Entities;
using Northwest.Persistence.Enums;
using SharedUtils.Extensions;
using System.Reflection;

namespace Northwest.Persistence;

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
    public DbSet<Log> Logs { get; set; }
    public DbSet<FriendPair> FriendPairs { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<LogAccessPermissions> LogAccessPermission { get; set; }
    public DbSet<PrivateChatRoom> PrivateChatRooms { get; set; }
    public DbSet<Expedition> Expeditions { get; set; }
    public DbSet<Landmass> Landmass { get; set; }
    public DbSet<ShipState> ShipStates { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }
    public DbSet<UserLobby> UserLobbies { get; set; }

    // authentication
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }

   // There is constructor injection.This must exist to be configurable in Program.cs
   // Dont need constructor if OnConfiguring is overriden instead of configuring upon injection in the web infrastructure layer.
    //public PlayerContext(DbContextOptions<PlayerContext> options) : base(options)
    //{

    //}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? playerContextConnectionString = "Server=localhost\\SQLEXPRESS;Database=Northwest;Trusted_Connection=True;TrustServerCertificate=True";

        optionsBuilder.UseSqlServer(playerContextConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) // denis says fluent api is for advanced shit
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder
            .Entity<GameAction>()
            .Property(e => e.GameActionType)
            .HasConversion(new EnumToStringConverter<GameActionType>());

        modelBuilder
            .Entity<Room>()
            .Ignore(e => e.AdjacentRoomNames);

        _ = modelBuilder.Entity<Role>()
            .Property(p => p.RoleName)
            .HasConversion(
            v => v.ToString(),
            v => v.ToEnum<RoleName>());

        modelBuilder.Entity<Lobby>()
            .HasMany(x => x.UserLobbies)
            .WithOne(x => x.Lobby);

        modelBuilder.Entity<Role>().HasData(new Role()
        {
            Id = Guid.NewGuid(),
            RoleName = RoleName.PereNoel,
            UserRoles = [],
        });
    }
}
