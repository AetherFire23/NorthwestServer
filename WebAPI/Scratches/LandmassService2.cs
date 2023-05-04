using Microsoft.AspNetCore.Components;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPI.Interfaces;
using WebAPI.Repository;
using WebAPI.Scratches;
using WebAPI.TestFolder;

namespace WebAPI
{
    public class LandmassService2 : ILandmassService2
    {
        private readonly PlayerContext _playerContext;
        private readonly IRoomRepository _roomRepository;
        private readonly ILandmassCardsService _landmassCardsService;

        public LandmassService2(PlayerContext playerContext, IRoomRepository roomRepository, ILandmassCardsService landmassCardsService)
        {
            _roomRepository = roomRepository;
            _playerContext = playerContext;
            _landmassCardsService = landmassCardsService;
        }

        public async Task AdvanceToNextLandmass(Guid gameId) // entry point for switching landmasses
        {
            var roomNames = await _landmassCardsService.DrawNextLandmassRoomNames(gameId);
            LandmassLayout layout = LandmassGetter.CreateNewLandmassLayout(roomNames); // attention : 8 rooms ici ! 
            Tuple<List<Room>, List<AdjacentRoom>> s = LandmassEntitiesInitializer.CreateNewDefaultLandmassRoomsAndConnections(layout, gameId);

            await WipeLandmassRoomsAndConnections(gameId);
            _playerContext.Rooms.AddRange(s.Item1);
            _playerContext.AdjacentRooms.AddRange(s.Item2);
            _playerContext.SaveChanges();
        }

        private async Task WipeLandmassRoomsAndConnections(Guid gameId) // faudra y penser
        {
            // risque de probleme quand je vais deleter si ya un gamestate qui se fait. en tout cas.
            var landmassRooms = await _roomRepository.GetAllLandmassRoomsInGame(gameId);
            var connections = await _roomRepository.GetLandmassAdjacentRoomsAsync(gameId);

            _playerContext.Rooms.RemoveRange(landmassRooms);
            _playerContext.AdjacentRooms.RemoveRange(connections);
            // do not saveChanges since conflict is unlikely, probably solves the gameState problem also.
        }
    }
}