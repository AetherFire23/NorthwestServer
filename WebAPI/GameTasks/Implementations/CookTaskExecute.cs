//using Shared_Resources.DTOs;
//using Shared_Resources.Entities;
//using Shared_Resources.GameTasks;
//using Shared_Resources.GameTasks.Implementations_Unity;
//using Shared_Resources.Models;
//using WebAPI.Game_Actions;
//using WebAPI.Interfaces;

//namespace WebAPI.GameTasks.Executions
//{
//    [GameTask(GameTaskCodes.ChargeCannon)]
//    public class CookTaskExecute : FireCannon
//    {
//        private const string _stationNameParam = "stationName";
//        private const int _stationCost = 3;

//        private readonly IStationRepository _stationRepo;
//        private readonly PlayerContext _playerContext;
//        public CookTaskExecute(PlayerContext playerContext, IStationRepository stationRepository)
//        {
//            _stationRepo = stationRepository;
//            _playerContext = playerContext;
//        }

//        public override async Task Execute(GameTaskContext context)
//        {
//            int i = 0;

//            var cookStation = await _stationRepo.RetrieveStationAsync<CookStationProperties>(context.GameState.GameId, nameof(StationsTemplate.Wheel));
//            var props = cookStation.ExtraProperties as CookStationProperties;

//            props.MoneyMade = 919191;

//            await _stationRepo.SaveStation(cookStation);

//            await _playerContext.SaveChangesAsync();
//        }

//        public async Task CreateLog(GameTaskContext gameTaskContext)
//        {
//            var gameAction = new GameAction()
//            {
//                GameActionType = Shared_Resources.Enums.GameActionType.Task,
//                GameId = gameTaskContext.GameState.GameId,
//                CreatedBy = gameTaskContext.Player.Name,
//            };

//            var log = new Log()
//            {
//                CreatedBy = gameTaskContext.Player.Name,
//                TriggeringPlayerId = gameTaskContext.Player.Id,
//                EventText = $"CookStation Was executed by {gameTaskContext.Player.Name}",
//                IsPublic = true,
//                RoomId = gameTaskContext.GameState.Room.Id,

//            };

//            await _playerContext.GameActions.AddAsync(gameAction);
//            await _playerContext.Logs.AddAsync(log);
//            await _playerContext.SaveChangesAsync();
//        }
//    }
//}
