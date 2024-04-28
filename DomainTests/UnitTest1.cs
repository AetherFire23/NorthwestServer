using Microsoft.Extensions.DependencyInjection;
using Northwest.Domain.Authentication;
using Northwest.Domain.Initialization;
using Northwest.Domain.Models.Requests;
using Northwest.Domain.Services;
using Northwest.WebApi.ApiConfiguration;

namespace DomainTests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        // Configure Domain

        var serviceCollection = new ServiceCollection();




        serviceCollection.InitializeDomainServices();
        serviceCollection.Configure<JwtConfig>(jc =>
        {

            jc.SecretKey = "HYTp1U0gRCf1zYFwVDND2N/tDlgcuJDSE+2nMcpdB8GlO6a7AHKV4K3S64r+zje/yE6NHOuJo4q3dg1tH254BS+tDO+4puPlad+PhJ0DKuRAP2Tx4Lo/UMFF7qN9oC1D2SiONQ8Pw7qGIqVmVV1dRGfR6vm+ePGvw0JlQ6AkLLZnw/M8PL2rMszJBrbakflAzg/k+HtTOCPVuxAN05gWBjPMygH6X2JJrgBtMjUBX/9vOtlougdNoPhnXvKMp+zBQw5kX8T6C4M6ceyNVAG65QEQcXAv9uSzNajgLD57wMs57aPxo1D+Ea2/SYQvnCyUGgtlGLruaycyV9jsx+rWiw==";
            jc.Issuer = "issuer";
            jc.Audience = "audience";
            jc.ExpirationDays = 999;
        });

        var serviceProvider = serviceCollection.BuildServiceProvider();
        await serviceProvider.SeedAndMigrate2();
        var s = serviceProvider.GetRequiredService<AuthenticationService>();
        

        var t = await s.TryRegister(new()
        {
            Email = "sd@hotmail.com",
            Password = "password",
            UserName = "username",
        });
        // register

        var s2 = await s.TryLogin(new(){
            PasswordAttempt = "password",
            UserName = "username",
        });

    }
}