using Newtonsoft.Json;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using WebAPI.Authentication;
using WebAPI.Interfaces;
using WebAPI.Repository.Users;

namespace WebAPI.Seeding;

public class SeedSequence : ISeedSequence
{
    private readonly IAuthenticationService _authenticationService;
    private readonly PlayerContext _playerContext;
    private readonly ILobbyService _lobbyService;
    private readonly IUserRepository _userRepository;
    private readonly IMainMenuRepository _mainMenuRepository;

    public SeedSequence(IAuthenticationService authenticationService,
        PlayerContext playerContext,
        ILobbyService lobbyService,
        IUserRepository userRepository,
        IMainMenuRepository mainMenuRepository)
    {
        _authenticationService = authenticationService;
        _playerContext = playerContext;
        _lobbyService = lobbyService;
        _userRepository = userRepository;
        _mainMenuRepository = mainMenuRepository;
    }

    public async Task ExecuteSeedSequence()
    {
        await SeedPereNoel();
        //  await SeedRegisterNormalUser();
        //await TestLobbyDestroy();


        await Make5PlayersRegisterAndJoinLobby();
    }

    public async Task SeedPereNoel()
    {
        var user = new User()
        {
            Id = _pereNoelId,
            Name = "Jack",
            Email = "HalloweenTown",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
        };

        var role = new Role()
        {
            Id = Guid.NewGuid(),
            RoleName = RoleName.PereNoel,
        };

        var userRole = new UserRole()
        {
            Id = Guid.NewGuid(),
            Role = role,
            User = user,
        };

        await _playerContext.AddAsync(user);
        await _playerContext.AddAsync(role);
        await _playerContext.AddAsync(userRole);
        await _playerContext.SaveChangesAsync();
    }

    public async Task TestLobbyDestroy()
    {
        var register2 = new RegisterRequest()
        {
            UserName = $"DummyPlayer n",
            Password = $"sexyman",
            Email = $"superhot"
        };
        var result2 = await _authenticationService.TryRegister(register2);
        var userDto2 = result2.DeserializeContent<UserDto>();

        var result = await _lobbyService.CreateAndJoinLobby(userDto2.Id, "wdwdwdw");
        var lobby = result.DeserializeContent<Lobby>();
        await _lobbyService.ExitLobby(userDto2.Id, lobby.Id);
    }

    public async Task SeedRegisterNormalUser()
    {
        var registerRequest = new RegisterRequest()
        {
            UserName = "Fred",
            Email = "fredf@gmail.com",
            Password = "password"
        };

        var t = await _authenticationService.TryRegister(registerRequest);
        var user = t.DeserializeContent<UserDto>();
        // add authorization later to reuse jwtToken

        var loginRequest = new LoginRequest()
        {
            PasswordAttempt = "password",
            UserName = "Fred",
        };

        var createLobbyResult = await _lobbyService.CreateAndJoinLobby(user.Id, "Hardood McLord");
        var createdLobby = createLobbyResult.DeserializeContent<Lobby>();

        var user32 = await _userRepository.GetUserById(user.Id);
        var userDto = await _userRepository.MapUserDtoById(user.Id);

        var mainMenustate = await _mainMenuRepository.GetMainMenuState(user32.Id);

        var serializedTest = JsonConvert.SerializeObject(mainMenustate, new JsonSerializerSettings() // must use special serializer settings to convert nested refs
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
        });

        MainMenuState state = JsonConvert.DeserializeObject<MainMenuState>(serializedTest);
    }

    public async Task Make5PlayersRegisterAndJoinLobby()
    {
        Lobby lobby = new Lobby();
        foreach (int index in Enumerable.Range(0, 5))
        {
            var register = new RegisterRequest()
            {
                UserName = $"DummyPlayer n{index}",
                Password = $"sexyman{index}",
                Email = $"superhot@{index}"
            };
            var result = await _authenticationService.TryRegister(register);
            var userDto = result.DeserializeContent<UserDto>();

            // create lobby if first player
            if (index is 0)
            {
                lobby = (await _lobbyService.CreateAndJoinLobby(userDto.Id, $"CreatorOfYourworld{userDto.Name}{index}"))
                    .DeserializeContent<Lobby>();
            }
            else // all others just join the lobby
            {
                await _lobbyService.JoinLobby(new JoinLobbyRequest()
                {
                    LobbyId = lobby.Id,
                    PlayerName = $"{userDto.Name}{index}",
                    UserId = userDto.Id,
                });
            }

            var mainMenuState = await _mainMenuRepository.GetMainMenuState(userDto.Id);

        }
        var register2 = new RegisterRequest()
        {
            UserName = $"DummyPlayer n",
            Password = $"sexyman",
            Email = $"superhot"
        };
        var result2 = await _authenticationService.TryRegister(register2);
        var userDto2 = result2.DeserializeContent<UserDto>();

        var c = await _lobbyService.JoinLobby(new JoinLobbyRequest()
        {
            LobbyId = lobby.Id,
            PlayerName = "wdwdwd",
            UserId = userDto2.Id
        });
    }
    private static Guid _pereNoelId = new Guid("83866b6c-02f3-4114-b66f-15d5cbbe7ae4");
}
