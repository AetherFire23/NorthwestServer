using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using System.Security.Cryptography.X509Certificates;
using WebAPI;
using WebAPI.Dummies;

namespace TestProject1
{
    public class UnitTest1
    {
        DbContextOptions<PlayerContext> _options;
        private static IServiceProvider _serviceProvider;

        public UnitTest1()
        {
            SetupOptions();
        }

        [Fact]
        public void Test1()
        {

        }

        public int Calculate(int x, int y)
        {
            return x + y;
        }

        private void SetupOptions()
        {
            _options = new DbContextOptionsBuilder<PlayerContext>()
            .UseInMemoryDatabase(databaseName: "PlayerTest")
            .Options;



            var services = new ServiceCollection();
            _serviceProvider = services.BuildServiceProvider();

        }

        private void BuildDefaults()
        {
            using (var context = new PlayerContext(_options))
            {
                Player player = new Player()
                {
                    Id = DummyValues.defaultPlayer1Guid,
                    ActionPoints = 99,
                    CurrentChatRoomId = Guid.NewGuid(),
                    CurrentGameRoomId = Guid.NewGuid(),
                    GameId = Guid.NewGuid(),
                    HealthPoints = 0,
                    Name = "UTest",
                    Profession = RoleType.Commander,
                    X = 2f,
                    Y = 0,
                    Z = 0,
                };
                context.Players.Add(player);
                context.SaveChanges();
            }
        }

        [Fact]
        public void TestGetPlayer()
        {
            //arr
            BuildDefaults();
            using (var context = new PlayerContext(_options))
            {
                // act
                var rep = new PlayerRepository(context);
                var players = rep.GetPlayer(DummyValues.defaultPlayer1Guid);


                // assert
                Assert.NotNull(players);
                Assert.Equal(players.Id, DummyValues.defaultPlayer1Guid);
            }
        }


        // plusieurs tests avec des var. differentes
        [Theory]
        [InlineData(2, 2, 4)]
        [InlineData(3, 2, 5)]
        [InlineData(4, 2, 6)]
        [InlineData(5, 2, 7)]
        [InlineData(6, 2, 8)]
        public void CalculateTest(int x, int y, int actual)
        {
            int result = Calculate2(x, y);
            Assert.Equal(result, actual);
        }

        public int Calculate2(int x, int y)
        {
            return x + y;
        }
    }
}