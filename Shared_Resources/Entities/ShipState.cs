using System;
namespace Shared_Resources.Entities;

public class ShipState
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public int HullInPercentage { get; set; }

    public int AdvancementInKilometersReal { get; set; } // secret
    public int AdvancementInKilometersExpected { get; set; }
    public int AdvancementInKilometersConfirmed { get; set; }
    public int DeviationInDegrees { get; set; }

    public int SpeedInKilometers { get; set; }

    // resources
    public int Cans { get; set; }
    public int Flour { get; set; }
    public int Coal { get; set; }
    public int Gunpowder { get; set; }
    public int Wood { get; set; }
    public int Iron { get; set; }
}
