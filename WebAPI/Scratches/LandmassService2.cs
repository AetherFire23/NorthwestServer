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
            var layout2 = LandmassGetter.CreateNewLandmass();
            List<string> roomNames2 = await _landmassCardsService.DrawNextLandmassRoomNames2(gameId, layout2);
            LandmassGetter.InsertLandmassNamesInLayout(layout2, roomNames2);
           // var roomNames = await _landmassCardsService.DrawNextLandmassRoomNames(gameId);
            // attention : 8 rooms ici ! 
            //LandmassLayout layout = LandmassGetter.CreateNewLandmassLayoutAndInsertNames(roomNames);
            Tuple<List<Room>, List<AdjacentRoom>> s = LandmassEntitiesInitializer.CreateNewDefaultLandmassRoomsAndConnections(layout2, gameId);
            // ligne ici pour initializer les stations des landmasses I guess

            await WipeLandmassRoomsAndConnections(gameId);
            await _playerContext.Rooms.AddRangeAsync(s.Item1);
            await _playerContext.AdjacentRooms.AddRangeAsync(s.Item2);
            await _playerContext.SaveChangesAsync();
        }

        private async Task WipeLandmassRoomsAndConnections(Guid gameId) // faudra y penser
        {
            // risque de probleme quand je vais deleter si ya un gamestate qui est recupere fait. en tout cas.
            var landmassRooms = await _roomRepository.GetAllLandmassRoomsInGame(gameId);
            var connections = await _roomRepository.GetLandmassAdjacentRoomsAsync(gameId);
            
            _playerContext.Rooms.RemoveRange(landmassRooms);
            _playerContext.AdjacentRooms.RemoveRange(connections);
            // do not saveChanges since conflict is unlikely, probably solves the gameState problem also.
        }
    }
}