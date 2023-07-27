using Microsoft.EntityFrameworkCore;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models.Requests;
using WebAPI.Extensions;
using WebAPI.Interfaces;

namespace WebAPI.Repository.Users;

public class UserRepository : IUserRepository
{
    private readonly PlayerContext _playerContext;
    private readonly IGameRepository _gameRepository;
    private readonly ILobbyRepository _lobbyRepository;

    public UserRepository(PlayerContext context,
        IGameRepository gameRepository,
        ILobbyRepository lobbyRepository)
    {
        _playerContext = context;
        _gameRepository = gameRepository;
        _lobbyRepository = lobbyRepository;
    }


    public async Task<UserDto> MapUserDtoById(Guid userId)
    {
        var user = await GetUserById(userId);
        if (user is null) throw new ArgumentException(nameof(user));
        var players = user.Players.ToList();

        var roleNames = user.UserRoles.Select(x => x.Role.RoleName).ToList();
        var activeGamesForUser = await GetActiveGameDtosForUser(user.Id);
        var lobbyDtos = await GetLobbyDtosForUser(userId);
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            RoleNames = roleNames,
            ActiveGamesForUser = activeGamesForUser,
            Players = players,
            QueuedLobbies = lobbyDtos,
        };
        return userDto;
    }

    public async Task<User?> GetUserByVerifyingCredentialsOrNull(LoginRequest loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.UserName)) throw new ArgumentNullException(nameof(loginRequest.UserName));
        if (string.IsNullOrEmpty(loginRequest.PasswordAttempt)) throw new ArgumentNullException(nameof(loginRequest.PasswordAttempt));

        User? user = await GetUserByCredentialsOrDefault(loginRequest.UserName);
        if (user is null) return null;
        if (!IsCorrectPassword(loginRequest.PasswordAttempt, user.PasswordHash)) return null;

        return user;
    }

    public async Task<User?> GetUserById(Guid id)
    {
        var user = await _playerContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(u => u.Games)
            .Include(u => u.Players)
            .ThenInclude(p => p.Game)
            .Include(u => u.UserLobbies)
            .ThenInclude(ur => ur.Lobby)
            .FirstOrDefaultAsync(u => u.Id == id);

        return user;
    }

    public async Task<User?> GetUserByCredentialsOrDefault(string userName)
    {
        var user = await _playerContext.Users.FirstOrDefaultAsync(u => u.Name == userName);
        return user;
    }

    private bool IsCorrectPassword(string passwordAttempt, string passwordHash)
    {
        bool isCorrect = BCrypt.Net.BCrypt.Verify(passwordAttempt, passwordHash);
        return isCorrect;
    }

    private async Task<User?> GetUserByNameOrNull(string name)
    {
        var user = await _playerContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(u => u.Name == name);


        return user;
    }

    private async Task<User?> GetUserIfVerifiedOrNull(Guid id, string password)
    {
        var user = await GetUserById(id).ConfigureAwait(false);
        bool isCorrect = BCrypt.Net.BCrypt.Verify(user.PasswordHash, password);
        if (!isCorrect) throw new ArgumentException("Password incorrect");
        return isCorrect ? user : null;
    }

    public async Task<bool> IsUserExists(string userName, string email)
    {
        bool exists = await _playerContext.Users.AnyAsync(x => x.Name == userName && x.Email == email);
        return exists;
    }

    public async Task<UserDto> CreateUser(RegisterRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = request.UserName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        var role = await _playerContext.Roles.SingleOrDefaultAsync(x => x.RoleName == RoleName.PereNoel);

        var userRole = new UserRole
        {
            Id = Guid.NewGuid(),
            Role = role,
            User = user
        };

        _playerContext.Users.Add(user);
        _playerContext.UserRoles.Add(userRole);
        await _playerContext.SaveChangesAsync();

        var dto = await MapUserDtoById(user.Id);

        return dto;
    }

    public async Task<List<GameDto>> GetActiveGameDtosForUser(Guid userId) // dont want that in entity since some games can be inactive
    {
        User? user = await GetUserById(userId);
        var activeGames = user.Games
            .Where(x => x.IsActive)
            .Select(x => x.Id)
            .ToList();

        List<GameDto> gameDtos = new();
        foreach (var game in activeGames)
        {
            var gameDto = await _gameRepository.MapGameDto(game);
            gameDtos.Add(gameDto);
        }

        return gameDtos;
    }

    public async Task<List<LobbyDto>> GetLobbyDtosForUser(Guid userId)
    {
        User? user = await GetUserById(userId);
        List<LobbyDto> lobbyDtos = new();
        foreach (var lobby in user.Lobbies)
        {
            var lobbyDto = await _lobbyRepository.MapLobbyDto(lobby.Id);
            lobbyDtos.Add(lobbyDto);
        }
        return lobbyDtos;
    }

    public async Task<List<UserDto>> GetUserDtosById(List<Guid> userIds)
    {
        List<UserDto> userDtos = await userIds.SelectAsync(MapUserDtoById);
        return userDtos;
    }

    public async Task<List<UserDto>> GetUserDtosFromUser(List<User> user)
    {
        List<Guid> userIds = user.Select(x => x.Id).ToList();
        List<UserDto> userDtos = await userIds.SelectAsync(MapUserDtoById);
        return userDtos;
    }

    
}
