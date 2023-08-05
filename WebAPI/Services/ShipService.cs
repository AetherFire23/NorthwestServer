using Shared_Resources.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Services;

public class ShipService : IShipService
{
    private readonly PlayerContext _playerContext;
    private readonly IShipRepository _shipRepository;
    public ShipService(PlayerContext playerContext,
        IShipRepository shipRepository)
    {
        _playerContext = playerContext;
        _shipRepository = shipRepository;
    }

    public async Task InitializeShipStatusesAndResources(Guid gameId)
    {
        // rajouter une varialbe Speed I guess ^^
        var shipStatus = new ShipState()
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            HullInPercentage = 0,
            DeviationInDegrees = 0,
            AdvancementInKilometersReal = 0,
            AdvancementInKilometersConfirmed = 0,
            AdvancementInKilometersExpected = 0,

            SpeedInKilometers = 10,

            Cans = 10,
            Coal = 10,
            Flour = 10,
            Gunpowder = 10,
            Iron = 10,
            Wood = 10,
        };

        _ = await _playerContext.ShipStates.AddAsync(shipStatus);
        _ = await _playerContext.SaveChangesAsync();
    }


    public async Task TickShipAdvancement(Guid gameId)
    {
        var shipState = await _shipRepository.GetShipStateAsync(gameId);
        if (shipState.HullInPercentage < 75)
        {
            shipState.SpeedInKilometers -= 1; // placeHolder
        }

        // Advancement should be checked elsehwere since 
        // its a winning condition

        if (shipState.DeviationInDegrees != 0)
        {
            // math formula to calculate expected vs real
        }

        _ = await _playerContext.SaveChangesAsync();
    }
}
