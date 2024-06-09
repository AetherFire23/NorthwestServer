


using Microsoft.Extensions.DependencyInjection;
using Northwest.Domain.GameTasks.Initialization;
using Northwest.Domain.Initialization.GameOptions;
using Northwest.Domain.Models;
using Northwest.Domain.Strategies;
using Northwest.Domain.UniversalSkills;

namespace Northwest.Domain.Initialization;

public static class DomainServicesInitialization
{
    public static void InitializeDomainServices(this IServiceCollection serviceCollection)
    {
        RoomsTemplate.InitializeDefaultReflectedRooms(); // could abstract those 2 if I wanted to waste my fucking time
        StationsTemplate.InitializeDefaultReflectedStations();
        RoleStrategyMapper.RegisterRoleStrategies(serviceCollection);
        SkillStrategyMapper.RegisterSkillStrategies(serviceCollection);
        DomainServicesConfigurator.AddServices(serviceCollection);
        DomainRepositoryConfigurator.AddServices(serviceCollection);
        serviceCollection.RegisterGameTaskTypes();

        var s = new GameOptionsValidation();
        s.ValidateGameOptionsAtGameLaunch();


        
    }
}