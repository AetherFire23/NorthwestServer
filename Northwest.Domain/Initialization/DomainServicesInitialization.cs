

using Microsoft.Extensions.DependencyInjection;
using Northwest.Domain.GameTasks.Initialization;
using Northwest.Domain.Strategies;
using Northwest.Domain.UniversalSkills;

namespace Northwest.Domain.Initialization;

public static class DomainServicesInitialization
{
    public static void InitializeDomainServices(this IServiceCollection serviceCollection)
    {
        RoleStrategyMapper.RegisterRoleStrategies(serviceCollection);
        SkillStrategyMapper.RegisterSkillStrategies(serviceCollection);
        DomainServicesConfigurator.AddServices(serviceCollection);
        DomainRepositoryConfigurator.AddServices(serviceCollection);
        serviceCollection.RegisterGameTaskTypes();
    }
}