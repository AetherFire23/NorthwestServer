﻿using Northwest.Domain.Repositories;
using Northwest.Persistence;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Services;

public class ShipService
{
    private readonly PlayerContext _playerContext;
    private readonly ShipRepository _shipRepository;
    public ShipService(PlayerContext playerContext, ShipRepository shipRepository)
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

        await _playerContext.ShipStates.AddAsync(shipStatus);
        await _playerContext.SaveChangesAsync();
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

        await _playerContext.SaveChangesAsync();
    }
}
