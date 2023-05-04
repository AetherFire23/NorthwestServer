namespace WebAPI
{
    public interface ILandmassService2
    {
        Task AdvanceToNextLandmass(Guid gameId);
    }
}