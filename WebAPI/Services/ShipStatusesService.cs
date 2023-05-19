using Shared_Resources.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class ShipStatusesService : IShipStasusesService
    {
        private readonly PlayerContext _playerContext;
        public ShipStatusesService(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public async Task InitializeShipStatusesAndResources(Guid gameId)
        {
            var shipStatus = new ShipState()
            {
                Id = Guid.NewGuid(),
                GameId = gameId,
                HullInPercentage = 100,
                DeviationInDegrees = 0,
                AdvancementInKilometers = 0,

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
    }
}
