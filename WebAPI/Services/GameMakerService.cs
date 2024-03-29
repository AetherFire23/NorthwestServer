﻿using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Enums;
using WebAPI.Extensions;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Strategies;
namespace WebAPI.Services;

public class GameMakerService
{
    private readonly RoomRepository _roomRepository;
    private readonly StationRepository _stationRepository;
    private readonly PlayerContext _playerContext;
    private readonly LandmassService _landmassService;
    private readonly LandmassCardsService _landmassCardsService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ShipService _shipStasusesService;
    private readonly LobbyRepository _lobbyRepository;
    private readonly UserRepository _userRepository;
    private readonly GameRepository _gameRepository;

    public GameMakerService(PlayerContext playerContext,
        RoomRepository roomRepository,
        StationRepository stationRepository,
        LandmassService landmassService,
        LandmassCardsService landmassCardsService,
        IServiceProvider serviceProvider,
        ShipService shipStasusesService,
        LobbyRepository lobbyRepository,
        UserRepository userRepository,
        GameRepository gameRepository)
    {
        _roomRepository = roomRepository;
        _stationRepository = stationRepository;
        _playerContext = playerContext;
        _landmassService = landmassService;
        _landmassCardsService = landmassCardsService;
        _serviceProvider = serviceProvider;
        _shipStasusesService = shipStasusesService;
        _lobbyRepository = lobbyRepository;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
    }
    public async Task CreateGameFromLobby(Guid lobbyId)
    {
        NewGameInfo newGameInfo = await CreateGameInfoPreparation(lobbyId);
        await InitializeNewGame(newGameInfo);
    }

    private async Task<NewGameInfo> CreateGameInfoPreparation(Guid lobbyId)
    {
        // I need to know username and player role 
        Lobby? lobby = await _lobbyRepository.GetLobbyById(lobbyId);
        List<UserDto> userDtos = await _userRepository.GetUserDtosFromUser(lobby.UsersInLobby);
        List<UserGamePreparation> usersGamePrep = await ShufflePlayerRoles(lobbyId);

        // having the bug shit thing
        NewGameInfo newGameInfo = new NewGameInfo()
        {
            Game = Game.CreateInitialGame(),
            UserGamePreparation = usersGamePrep,
            Users = userDtos,
        };

        return newGameInfo;
    }

    private async Task<List<UserGamePreparation>> ShufflePlayerRoles(Guid lobbyId)
    {
        List<RoleType> randomRoles = EnumFunExtensions.GetAllEnumValues<RoleType>().Shuffle();
        List<UserGamePreparation> userSelections = await GetPlayerGamePrepFromLobby(lobbyId);

        // sets the randomly selected roles into the usergameprep
        foreach ((RoleType role, UserGamePreparation? userGamePreparation) in randomRoles.Zip(userSelections))
        {
            userGamePreparation.RoleType = role;
        }
        return userSelections;
    }

    private async Task InitializeNewGame(NewGameInfo newGameInfo)
    {
        // adds an empty game to the databse
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

    private async Task InitializePlayersAsync(NewGameInfo gameInfo)
    {
        /* Will need to clarify what gets initialized before what.
          for example, class cannot depend on starting room(for knowing which bonuses is applying), and starting room also depend on class. One needs to be
          created before the other. */
        foreach (UserDto userDto in gameInfo.Users)
        {
            User? userEntity = await _userRepository.GetUserById(userDto.Id); // entity doit etre tracked encore une fois. jpas sur daimer ca 
            Game gameEntity = await _gameRepository.GetGameById(gameInfo.GameId);
            // ouache 
            UserGamePreparation selection = gameInfo.UserGamePreparation.First(x => x.UserId == userDto.Id);
            Player newPlayer = new Player()
            {
                Id = userDto.Id, // users not saved yet in db, so can use user key as primary key. Will have to Guid.NewGuid() some day
                ActionPoints = 0,
                GameId = gameInfo.Game.Id,
                HealthPoints = 0,
                Name = selection.Name,
                Profession = gameInfo.UserGamePreparation.First(x => x.UserId == userDto.Id).RoleType,
                X = 0f,
                Y = 0f,
                Z = 0f,
                User = userEntity,
                Game = gameEntity,
            };

            // wtf quand tu add un player sanss User  ca cree un ostie de user wtf entity framework de chien sale 

            await InitializePlayerRoleSettings(newPlayer);
            _ = await _playerContext.Players.AddAsync(newPlayer);
            _ = await _playerContext.SaveChangesAsync();
        }
    }

    private async Task InitializePlayerRoleSettings(Player player)
    {
        IRoleInitializationStrategy roleStrategy = ResolveRoleStrategy(player.Profession);
        await roleStrategy.InitializePlayerFromRoleAsync(player);
    }

    private async Task InitializeItemsAsync(NewGameInfo info) // problems with disappearing items : check landmass creation that swaps the landmasses...
    {
        List<Room> rooms = await _roomRepository.GetRoomsInGamesync(info.Game.Id);

        Item item2 = new Item()
        {
            Id = Guid.NewGuid(),
            ItemType = ItemType.Hose,
            OwnerId = rooms.First(x => x.Name == nameof(RoomsTemplate.CrowsNest)).Id,
        };

        Item item3 = new Item()
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
        Lobby? lobby = await _lobbyRepository.GetLobbyById(lobbyId);
        List<UserGamePreparation> playerSelections = lobby.UserLobbies.Select(x => new UserGamePreparation
        {
            UserId = x.User.Id,
        }).ToList();

        return playerSelections;
    }

    private IRoleInitializationStrategy ResolveRoleStrategy(RoleType role)
    {
        Type strategType = RoleStrategyMapper.GetStrategyTypeByRole(role);
        IRoleInitializationStrategy? strategy = _serviceProvider.GetService(strategType) as IRoleInitializationStrategy;
        return strategy;
    }
}
