using Northwest.Persistence.Enums;
namespace Northwest.Domain.Initialization.GameOptions;

internal class GameOptionsValidation
{
    internal void ValidateGameOptionsAtGameLaunch()
    {
        ValidateEnumPlayerCount();
    }

    private void ValidateEnumPlayerCount()
    {
        var roleCount = Enum.GetValues<RoleType>().Length;
        bool hasEnoughRolesForAllPlayers = roleCount >= GameOptionsData.MaximumPlayerAmount;

        if (!hasEnoughRolesForAllPlayers) throw new Exception("Not enough roles for all players");
    }
}
