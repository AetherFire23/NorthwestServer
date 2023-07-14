using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using WebAPI.Extensions;
using WebAPI.Interfaces;
using WebAPI.Repository.Users;
using WebAPI.Strategies;
namespace WebAPI.Services;

public class GameMakerService : IGameMakerService
{
    private readonly IGameMakerRepository _gameMakerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IStationRepository _stationRepository;
    private readonly PlayerContext _playerContext;
    private readonly ILandmassService _landmassService;
    private readonly ILandmassCardsService _landmassCardsService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IShipService _shipStasusesService;
    private readonly ILobbyRepository _lobbyRepository;
    private readonly IUserRepository _userRepository;

    public GameMakerService(PlayerContext playerContext,
        IGameMakerRepository gameMakerRepository,
        IRoomRepository roomRepository,
        IStationRepository stationRepository,
        ILandmassService landmassService,
        ILandmassCardsService landmassCardsService,
        IServiceProvider serviceProvider,
        IShipService shipStasusesService,
        ILobbyRepository lobbyRepository,
        IPlayerRepository playerRepository,
        IUserRepository userRepository)
    {
        _gameMakerRepository = gameMakerRepository;
        _roomRepository = roomRepository;
        _stationRepository = stationRepository;
        _playerContext = playerContext;
        _landmassService = landmassService;
        _landmassCardsService = landmassCardsService;
        _serviceProvider = serviceProvider;
        _shipStasusesService = shipStasusesService;
        _lobbyRepository = lobbyRepository;
        _userRepository = userRepository;
    }
    public async Task CreateGameFromLobby(Guid lobbyId)
    {
        NewGameInfo newGameInfo = await CreateGameInfoPreparation(lobbyId);
        await InitializeNewGame(newGameInfo);
    }

    private async Task<NewGameInfo> CreateGameInfoPreparation(Guid lobbyId)
    {
        // I need to know username and player role 
        var lobby = await _lobbyRepository.GetLobbyById(lobbyId);
        var userDtos = await _userRepository.GetUserDtosFromUser(lobby.UsersInLobby);
        var usersGamePrep = await ShufflePlayerRoles(lobbyId);
        var newGameInfo = new NewGameInfo()
        {
            Game = Game.FactorizeInitialGame(),
            UserGamePreparation = usersGamePrep,
            Users = userDtos,
        };

        return newGameInfo;
    }

    private async Task<List<UserGamePreparation>> ShufflePlayerRoles(Guid lobbyId)
    {
        // sets the randomly selected roles into the usergameprep
        List<RoleType> randomRoles = EnumFunExtensions.GetAllEnumValues<RoleType>().Shuffle();
        List<UserGamePreparation> userSelections = await GetPlayerGamePrepFromLobby(lobbyId);
        randomRoles.ApplyActionOnMutualIndexMatch(userSelections,
            (randomRole, gamePrep) => gamePrep.RoleType = randomRole);

        return userSelections;
    }

    private async Task InitializeNewGame(NewGameInfo newGameInfo)
    {
        await _playerContext.Games.AddAsync(newGameInfo.Game);
        await _playerContext.SaveChangesAsync();


        // Need to initialize rooms before players, because player construction requires a GameRoomId.
        await _roomRepository.CreateNewRoomsAndConnections(newGameInfo.GameId);
        await _shipStasusesService.InitializeShipStatusesAndResources(newGameInfo.Game.Id);
        await InitializePlayersAsync(newGameInfo);
        await InitializeItemsAsync(newGameInfo);
        await _landmassCardsService.InitializeLandmassCards(newGameInfo.Game.Id);

        // Stations are tied to rooms, currently, only in services that interact with them in a hardcoded way. - So stations can be created independently of rooms
        await _stationRepository.CreateAndAddStationsToDb(newGameInfo.GameId);
        await _landmassService.AdvanceToNextLandmass(newGameInfo.GameId);
        await _playerContext.SaveChangesAsync();
    }



    private async Task InitializePlayersAsync(NewGameInfo info)
    {
        /* Will need to clarify what gets initialized before what.
          for example, class cannot depend on starting room(for knowing which bonuses is applying), and starting room also depend on class. One needs to be
          created before the other. */
        foreach (var userDto in info.Users)
        {
            var userEntity = await _userRepository.GetUserById(userDto.Id); // entity doit etre tracked encore une fois. jpas sur daimer ca 
            // cte feature-la serieux 
            UserGamePreparation selection = info.UserGamePreparation.First(x => x.UserId == userDto.Id);
            var newPlayer = new Player()
            {
                Id = userDto.Id, // users not saved yet in db, so can use user key as primary key. Will have to Guid.NewGuid() some day
                ActionPoints = 0,
                GameId = info.Game.Id,
                HealthPoints = 0,
                Name = selection.Name,
                Profession = info.UserGamePreparation.First(x => x.UserId == userDto.Id).RoleType,
                X = 0f,
                Y = 0f,
                Z = 0f,
                User = userEntity
            };

            // wtf quand tu add un player sanss User  ca cree un ostie de user wtf entity framework de chien sale 

            await InitializePlayerRoleSettings(newPlayer);
            await _playerContext.Players.AddAsync(newPlayer);
            await _playerContext.SaveChangesAsync();
        }
    }

    private async Task InitializePlayerRoleSettings(Player player)
    {
        var roleStrategy = ResolveRoleStrategy(player.Profession);
        await roleStrategy.InitializePlayerFromRoleAsync(player);
    }

    private async Task InitializeItemsAsync(NewGameInfo info) // problems with disappearing items : check landmass creation that swaps the landmasses...
    {
        List<Room> rooms = await _roomRepository.GetRoomsInGamesync(info.Game.Id);

        var item2 = new Item()
        {
            Id = Guid.NewGuid(),
            ItemType = ItemType.Hose,
            OwnerId = rooms.First(x => x.Name == nameof(RoomsTemplate.CrowsNest)).Id,
        };

        var item3 = new Item()
        {
            Id = Guid.NewGuid(),
            ItemType = ItemType.Hose,
            OwnerId = rooms.First(x => x.Name == nameof(RoomsTemplate.CrowsNest)).Id,
        };

        await _playerContext.Items.AddAsync(item2);
        await _playerContext.Items.AddAsync(item3);
        await _playerContext.SaveChangesAsync();
    }

    private async Task<List<UserGamePreparation>> GetPlayerGamePrepFromLobby(Guid lobbyId)
    {
        // name is stored in UserLobby upon joining a lobby
        var lobby = await _lobbyRepository.GetLobbyById(lobbyId);
        var playerSelections = lobby.UserLobbies.Select(x => new UserGamePreparation
        {
            UserId = x.User.Id,
            Name = x.NameSelection
        }).ToList();

        return playerSelections;
    }

    private IRoleInitializationStrategy ResolveRoleStrategy(RoleType role)
    {
        var strategType = RoleStrategyMapper.GetStrategyTypeByRole(role);
        var strategy = _serviceProvider.GetService(strategType) as IRoleInitializationStrategy;
        return strategy;
    }
}
