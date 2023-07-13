namespace WebAPI.SSE.Senders
{
    internal interface IMainMenuSSESender
    {
        Task RefreshLobbiesAndGamesInfo(Guid userId);
    }
}