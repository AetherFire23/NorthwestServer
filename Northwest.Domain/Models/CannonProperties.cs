using Northwest.Domain.Enums;

namespace Northwest.Domain.Models;

public class CannonProperties
{
    public int Ammo { get; set; } // bool Charged
    public State State { get; set; }
}
