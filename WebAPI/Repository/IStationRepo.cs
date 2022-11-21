﻿using WebAPI.GameTasks.Stations;

namespace WebAPI.Repository
{
    public interface IStationRepository
    {
        public StationDTO RetrieveStation<T>(Guid playerId, string stationName);
        public void SaveStation(StationDTO station);
    }
}