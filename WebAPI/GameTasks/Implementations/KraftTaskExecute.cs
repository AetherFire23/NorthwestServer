using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.GameTasks;
using Shared_Resources.GameTasks.Implementations_Unity;
namespace WebAPI.GameTasks.Implementations;

[GameTask(GameTaskCodes.CraftTask)]
public class KraftTaskExecutea : KraftTaskBase
{
    // should create ItemRepository 
    // to do something like .AddItem(ownerId, ItemTpype)
    private readonly PlayerContext _playerContext;

    public KraftTaskExecutea(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public override async Task Execute(GameTaskContext context)
    {
        Item item = new Item()
        {
            Id = Guid.NewGuid(),
            OwnerId = context.Player.Id,
            ItemType = ItemType.Hose,
        };

        _ = await _playerContext.Items.AddAsync(item);
        _ = await _playerContext.SaveChangesAsync();
    }
}
