namespace WebAPI.Interfaces
{
    public interface ILandmassService
    {
        Task AdvanceToNextLandmass(Guid gameId);
    }
}