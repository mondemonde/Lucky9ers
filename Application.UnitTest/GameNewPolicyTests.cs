using System.Numerics;
using System.Threading.Tasks;
using Domain.Models;
using FluentValidation;
using Lucky9.Application._BusinessRules;
using Lucky9.Application._Features.GameFeatures.Commands;
using Lucky9.Application.Interfaces;
using Lucky9.Domain.Entities;
using Lucky9.Infrastructure.Services;
using Moq;
using Xunit;

namespace Application.UnitTest
{
    public class GameNewPolicyTests
    {
        [Fact]
        public async Task AsserCreatePolicy_ValidBet_ReturnsBetObject()
        {
            // Arrange
            var addGameClientCommand = new AddGameClientCommand
            {
                Email = "test@example.com",
                BetMoney = 100
            };

            var playerRepositoryMock = new Mock<IPlayerRepository>();
            playerRepositoryMock.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new User[] { new User { Email = "test@example.com" } });

            var betRepositoryMock = new Mock<IBetRepository>();

            var gameSvc = new GameService();
            var gameRepositoryMock = new Mock<IGameRepository>();
            
            gameRepositoryMock.Setup(repo => repo.Add(It.IsAny<Game>()))
            .ReturnsAsync((Game entity) => entity);


            // Act
            var result = await GameNewPolicy.AsserCreatePolicy(
                addGameClientCommand,
                playerRepositoryMock.Object,
                betRepositoryMock.Object,
                gameRepositoryMock.Object,
                gameSvc
            );

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Game);
            Assert.Equal(100, result.BetValue);

            Assert.True(!string.IsNullOrEmpty(result.PlayerCards));
            Assert.True(!string.IsNullOrEmpty(result.Game.ServerCards));
        }

        
    }
}
