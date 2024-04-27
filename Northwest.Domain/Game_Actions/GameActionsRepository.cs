using Northwest.Domain.Enums;
using Northwest.Persistence;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Game_Actions;

public class GameActionsRepository
{
    private readonly PlayerContext _playerContext;
    private readonly PlayerRepository _playerRepository;

    public GameActionsRepository(PlayerContext playerContext, PlayerRepository playerRepository)
    {
        _playerContext = playerContext;
        _playerRepository = playerRepository;
    }

    public void ChangeRoomAction(Player player, Room from, Room to)
    {
        var roomChangeInfo = new RoomChangeInfo()
        {
            Player = player,
            Room1 = from,
            Room2 = to,
        };

        var gameAction = roomChangeInfo.ToGameAction();
        var roomLog = roomChangeInfo.ToRoomLog();
        _playerContext.GameActions.Add(gameAction);
        _playerContext.Logs.Add(roomLog);

        _playerContext.SaveChanges();
    }
}
