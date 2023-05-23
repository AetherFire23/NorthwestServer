using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LandmassTests
{
    public class RoomCell
    {
        public string Name { get; set; }

        public Dictionary<Direction, RoomCell> Neighbors { get; set; } = new();

        public Dictionary<Direction, RoomCell> DoorConnections { get; set; } = new();

        [JsonIgnore]
        public Dictionary<Direction, RoomCell> RoomNeighbors => Neighbors.Where(x => x.Value.Name != "").ToDictionary(x => x.Key, x => x.Value);

        [JsonIgnore]
        public Dictionary<Direction, RoomCell> EmptyNeighbors => Neighbors.Where(x => x.Value.Name == "").ToDictionary(x => x.Key, x => x.Value);

        public Rectangle FormsRectangle;

        public Point Position;

        public string DebugCreator = string.Empty;

        public bool IsEmptySpace => this.Name == "";
        public void ConnectNeighbors(RoomCell roomCell, Direction direction)
        {
            this.Neighbors.Add(direction, roomCell);

            Direction oppositeDirection = DirectionHelper.GetOppositeDirection(direction);
            roomCell.Neighbors.Add(oppositeDirection, this);
        }

        public void ConnectDoors(RoomCell roomCell)
        {
            bool alreadyConnected = this.DoorConnections.Values.Contains(roomCell)
                || roomCell.DoorConnections.Values.Contains(this);


            bool isSelf = roomCell == this;
            if (alreadyConnected || isSelf)
            {
                return;
            }

            var keyPair = this.Neighbors.FirstOrDefault(x => x.Value == roomCell);
            this.DoorConnections.Add(keyPair.Key, keyPair.Value);

            Direction opposite = DirectionHelper.GetOppositeDirection(keyPair.Key);
            roomCell.DoorConnections.Add(opposite, this);
        }

        public List<RoomCell> GetAllNeighboringRooms()
        {
            var neighorRooms = this.Neighbors.Select(x => x.Value)
                .Where(x => x.Name != "").ToList();

            return neighorRooms;
        }

        public void PrintNeighbor()
        {
            Console.WriteLine($"{this.Name}");

            foreach (var cell in this.Neighbors)
            {
                Console.WriteLine($"{cell.Key}");
            }
        }

        public override string ToString()
        {
            string name = this.Name == "" ? "Empty" : this.Name;
            return name;
        }
    }
}
