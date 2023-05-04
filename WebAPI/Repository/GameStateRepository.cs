﻿using WebAPI.Game_Actions;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using Shared_Resources.DTOs;
using WebAPI.Interfaces;
using Shared_Resources.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Repository
{
    public class GameStateRepository : IGameStateRepository
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly PlayerContext _playerContext;
        public GameStateRepository(PlayerContext playerContext, IPlayerRepository playerRepository, IRoomRepository roomRepository)
        {
            _playerContext = playerContext;
            _playerRepository = playerRepository;
            _roomRepository = roomRepository;
        }

        public async Task<GameState> GetPlayerGameStateAsync(Guid playerId, DateTime? lastTimeStamp)
        {
            //temp
            //Player player = _playerContext.Players.FirstOrDefault(player => player.Id == playerId);
            Player player = await _playerRepository.GetPlayerAsync(playerId);

            PlayerDTO playerDTO = await _playerRepository.MapPlayerDTOAsync(playerId);
            List<Message> newMessages = await GetNewMessagesAsync(lastTimeStamp, playerDTO.GameId);
            List<Player> players = _playerRepository.GetPlayersInGame(playerDTO.GameId);
            DateTime timeStamp = DateTime.UtcNow;
            List<PrivateChatRoomParticipant> chatRoomParticipants = await GetChatRoomsWithMainPlayerInItAsync(playerDTO.Id);

            //string serializedLayout = _playerContext.Landmass.First(l => l.GameId == playerDTO.GameId).SerializedLandmassLayout;
            RoomDTO roomDTO = await _roomRepository.GetRoomDTOAsync(player.CurrentGameRoomId);

            // FIELD INITIALIZER NOT RECOMMENDED WITH ASYNC/AWAIT
            List<RoomDTO> roomList = await GetAllRoomDTOSInGameAsync(playerDTO.GameId);

            var trigs = await _playerRepository.GetTriggerNotificationsAsync(playerId, lastTimeStamp);
            var logs = _playerRepository.GetAccessibleLogs(playerId, lastTimeStamp);
            var privs = await GetPrivateChatRoomsAsync(player.Id);

            var gameState = new GameState()
            {
                PlayerDTO = playerDTO,
                TriggerNotifications = trigs,
                NewMessages = newMessages,
                Players = players,
                TimeStamp = timeStamp,
                PrivateChatRoomParticipants = chatRoomParticipants,
                Room = roomDTO,
                Logs = logs,
                Rooms = roomList,
                SerializedLayout = string.Empty,
                PrivateChatRooms = privs,
            };

            return gameState;
        }

        public async Task<List<PrivateChatRoom>> GetPrivateChatRoomsAsync(Guid playerId)
        {
            var rooms = await _playerContext.PrivateChatRooms.Join(_playerContext.PrivateChatRoomParticipants,
                p => p.Id,
                participant => participant.RoomId,
                (p, p2) => p).ToListAsync();

            return rooms;
        }

        public async Task<List<RoomDTO>> GetAllRoomDTOSInGameAsync(Guid gameId)
        {
            var roomsInGame = await _playerContext.Rooms.Where(x => x.GameId == gameId).ToListAsync();

            var roomDtos = new List<RoomDTO>();

            foreach (var room in roomsInGame)
            {
                var roomDTO = await _roomRepository.GetRoomDTOAsync(room.Id);
                roomDtos.Add(roomDTO);
            }

            // var roomDTOs = await Task.WhenAll(roomsInGame.Select(async x => await _roomRepository.GetRoomDTOAsync(x.Id)));

            return roomDtos;
        }

        public async Task<List<PrivateChatRoomParticipant>> GetChatRoomsWithMainPlayerInItAsync(Guid playerID)
        {
            var roomsTheMainPlayerIsIn = await _playerContext.PrivateChatRoomParticipants.Where(pair => pair.ParticipantId == playerID).ToListAsync();
            var chatRoomIds = roomsTheMainPlayerIsIn.Select(chatRoom => chatRoom.RoomId).ToList();
            var participantsInSameRoom = await _playerContext.PrivateChatRoomParticipants.Where(room => chatRoomIds.Contains(room.RoomId)).ToListAsync(); // qqch marche pas
            var excludeMainPlayer = participantsInSameRoom.Where(room => room.ParticipantId != playerID);

            return participantsInSameRoom;
        }

        public async Task<List<Message>> GetNewMessagesAsync(DateTime? lastTimeStamp, Guid gameId)
        {
            bool hasReceivedMessages = lastTimeStamp != null;
            var messages = !hasReceivedMessages
                   ? GetMessagesFromGameId(gameId)
                   : GetMessagesAfterTimeStamp(lastTimeStamp, gameId);

            return messages;
        }

        public List<Message> GetMessagesFromGameId(Guid gameId)
        {
            var allMessages = _playerContext.Messages.Where(mess => mess.GameId == gameId).ToList();
            return allMessages;
        }

        public List<Message> GetMessagesAfterTimeStamp(DateTime? timeStamp, Guid gameId)
        {
            return _playerContext.Messages
                .Where(message => message.Created > timeStamp
                      && message.GameId == gameId).ToList();
        }
    }
}
