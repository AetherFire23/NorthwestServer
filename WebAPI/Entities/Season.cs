using WebAPI.Enums;
namespace WebAPI.Entities;

public class Season
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Seasons Current { get; set; }
    public bool Frozen { get; set; }// Degres de gel ?
}
