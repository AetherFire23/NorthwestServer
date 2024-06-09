using DomainTests.AdditionalServices;
using DomainTests.GameStart;
using Microsoft.Extensions.DependencyInjection;
using Northwest.Domain.Authentication;
using Northwest.Domain.Initialization;
using Northwest.WebApi.ApiConfiguration;
namespace DomainTests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var names = NamesListReader.Read;

        var serviceCollection = new ServiceCollection();

        serviceCollection.InitializeDomainServices();

        // could move this into infra honestly 
        serviceCollection.Configure<JwtConfig>(jc =>
        {
            jc.SecretKey = "HYTp1U0gRCf1zYFwVDND2N/tDlgcuJDSE+2nMcpdB8GlO6a7AHKV4K3S64r+zje/yE6NHOuJo4q3dg1tH254BS+tDO+4puPlad+PhJ0DKuRAP2Tx4Lo/UMFF7qN9oC1D2SiONQ8Pw7qGIqVmVV1dRGfR6vm+ePGvw0JlQ6AkLLZnw/M8PL2rMszJBrbakflAzg/k+HtTOCPVuxAN05gWBjPMygH6X2JJrgBtMjUBX/9vOtlougdNoPhnXvKMp+zBQw5kX8T6C4M6ceyNVAG65QEQcXAv9uSzNajgLD57wMs57aPxo1D+Ea2/SYQvnCyUGgtlGLruaycyV9jsx+rWiw==";
            jc.Issuer = "issuer";
            jc.Audience = "audience";
            jc.ExpirationDays = 999;
        });


        serviceCollection.RegisterTestServices();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        await serviceProvider.SeedAndMigrate2();



        // ------ TEST ---------

        await TestStuff(serviceProvider);

        // Just start a game I guess
    }

    private static async Task TestStuff(ServiceProvider serviceProvider)
    {
        var gss = serviceProvider.GetRequiredService<GameStartService>();

        await gss.CreateUsersAndStartGame();
    }
}