
using WebAPI.Interfaces;
using WebAPI.Repository;
using WebAPI.TestFolder;
using WebAPI.Constants;
using Newtonsoft.Json;
using LandmassTests;
using Shared_Resources.Entities;
using Shared_Resources.DTOs;

namespace WebAPI.Services
{
    public class LandmassService : ILandmassService
    {
        private readonly PlayerContext _playerContext;
        private readonly IRoomRepository _roomRepository;
        private readonly IStationRepository _stationRepository;
        private readonly ILandmassRepository _landmassRepository;
        public LandmassService(PlayerContext playerContext, IRoomRepository roomRepository,
            IStationRepository stationRepository,
            ILandmassRepository landmassRepository)
        {
            _playerContext = playerContext;
            _roomRepository = roomRepository;
            _stationRepository = stationRepository;
            _landmassRepository = landmassRepository;
        }

        public void NextLandmass(Guid gameId)
        {
            // Method to save all stations/properties would be here. 
            // Clean the previous Landmass information (other method maybe
            DisableCurrentLandmassRoomsAndStations(gameId);
            DeleteExistingLandmassConnections(gameId);

            // Get Cards and activate rooms and stations
            LandmassCards deck = _landmassRepository.GetCurrentLandmassDeckSetup(gameId);
            List<Card> drawnRoomcards = DrawNextLandmassCards(deck);
            ActivateAndInitializeDrawnRoomsAndStations(gameId, drawnRoomcards);

            // Now initialize the layout and connections between landmassRooms
            LandmassLayout layout = _landmassRepository.GetRandomLandmassLayout();
            ChangeLandmassLayoutRoomNames(layout, drawnRoomcards);
            CreateConnectionsBetweenLandmassRooms(gameId, layout);

            _landmassRepository.SaveLandmassLayout(gameId, layout);
            _landmassRepository.SaveDecksSetup(deck);
        }

        public void ChangeLandmassLayoutRoomNames(LandmassLayout layout, List<Card> drawnRoomCards)
        {
            // if (drawnRoomCards.Count != layout.AllRooms.Count) throw new Exception("When cards are drawn, an equal number of rooms must be picked.");
            for (int i = 0; i < drawnRoomCards.Count; i++)
            {
                var cell = layout.AllRooms[i];
                var card = drawnRoomCards[i];
                cell.Name = card.Name;
            }
        }

        public void DisableCurrentLandmassRoomsAndStations(Guid gameId)
        {
            var activeRooms = _roomRepository.GetAllActiveLandmassRooms(gameId);
            var activeStations = _playerContext.Stations.Where(x => x.IsActive && x.IsLandmass).ToList();
            foreach (var room in activeRooms)
            {
                room.IsActive = false;
            }

            foreach (var station in activeStations)
            {
                station.SerializedProperties = string.Empty;
                station.IsActive = false;
            }
            _playerContext.SaveChanges();
        }

        public void DeleteExistingLandmassConnections(Guid gameId)
        {
            var landmassRooms = _roomRepository.GetAllLandmassRooms(gameId);
            foreach (var room in landmassRooms)
            {
                _roomRepository.RemoveFromAllConnectedRooms(room.Id);
            }
        }

        public List<Card> DrawNextLandmassCards(LandmassCards setup)
        {

            int roomAmount = 3; // harcoded the amount of rooms
            Random random = new Random();
            var cards = new List<Card>();

            for (int i = 0; i < roomAmount; i++)
            {
                int randomIndex = random.Next(0, setup.AllCards.Count);
                var randomCard = setup.AllCards[randomIndex];

                if (cards.Contains(randomCard))
                {
                    i--;

                }
                cards.Add(randomCard);
            }
            return cards;
        }

        public void ActivateAndInitializeDrawnRoomsAndStations(Guid gameId, List<Card> drawnRoomcards)
        {
            // 1. Get rooms and their stations
            var gameRooms = _roomRepository.GetRoomsInGame(gameId);

            List<string> cardNames = drawnRoomcards.Select(c => c.Name).ToList();
            List<Room> drawnRooms = gameRooms.Where(r => cardNames.Contains(r.Name)).ToList();

            List<Station> drawnStations = new();

            foreach (var room in gameRooms)
            { // should be repository stuff here
                string stationName = string.Empty;
                bool hasStation = Constants.Constants.StationsFromRoomName.TryGetValue(room.Name, out stationName); // wtf 
                if (!hasStation) continue;

                // Station drawnStation = _playerContext.Stations.First(s => s.Name == stationName);
                Station drawnStation = _stationRepository.GetStationByName(gameId, stationName);

                drawnStations.Add(drawnStation);
            }

            // 2 Initialize with defaults
            foreach (Room room in drawnRooms)
            {
                room.IsActive = true;
            }

            // will need to map the stations and their properties also...
            foreach (Station station in drawnStations)
            {
                station.IsActive = true;
                station.SerializedProperties = StationRepository.SerializedStation1DefaultProperty;
            }
            _playerContext.SaveChanges();
        }

        public void CreateConnectionsBetweenLandmassRooms(Guid gameId, LandmassLayout layout)
        {
            foreach (RoomCell roomCell in layout.AllRooms)
            {
                bool isDefaultRoom = roomCell.Name.Contains("room");
                if (isDefaultRoom) continue;

                List<RoomCell> neighborsInNonRedundantDirections = roomCell.RoomNeighbors
                    .Where(x => DirectionHelper.NonRedundantNeighborDirection.Contains(x.Key))
                    .Select(x => x.Value).ToList();

                if (!neighborsInNonRedundantDirections.Any()) continue;

                Room room = _roomRepository.GetRoomFromName(gameId, roomCell.Name);

                foreach (var neighbor in neighborsInNonRedundantDirections)
                {
                    bool isDefaultNeighbor = neighbor.Name.Contains("room");
                    if (isDefaultNeighbor) continue;

                    Room neighborRoom = _playerContext.Rooms.FirstOrDefault(r => r.Name == neighbor.Name && r.Id == gameId);
                    var connection = new AdjacentRoom()
                    {
                        Id = Guid.NewGuid(),
                        RoomId = room.Id,
                        AdjacentId = neighborRoom.Id,
                    };
                    _playerContext.AdjacentRooms.Add(connection);
                }
            }
            _playerContext.SaveChanges();
        }
    }
}