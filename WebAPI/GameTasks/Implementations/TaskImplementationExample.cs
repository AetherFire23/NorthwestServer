using Shared_Resources.GameTasks;
using Shared_Resources.GameTasks.Implementations_Unity;
using WebAPI.Interfaces;

namespace WebAPI.GameTasks.Executions;

[GameTask(GameTaskCodes.ChargeCannon)]
public class CookTaskExecute : FireCannon
{
    private const string _stationNameParam = "stationName";
    private const int _stationCost = 3;

    private readonly IStationRepository _stationRepo;
    private readonly PlayerContext _playerContext;
    public CookTaskExecute(PlayerContext playerContext, IStationRepository stationRepository)
    {
        _stationRepo = stationRepository;
        _playerContext = playerContext;
    }

    public override async Task Execute(GameTaskContext context)
    {
        //var cookStation = await _stationRepo.RetrieveStationAsync<CookStationProperties>(context.GameState.GameId, nameof(StationsTemplate.Wheel));
        //var props = cookStation.ExtraProperties as CookStationProperties;

        await _playerContext.SaveChangesAsync();
    }
}
