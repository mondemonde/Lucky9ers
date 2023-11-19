using System.Threading.Tasks;
using Domain.Models;
using FluentValidation;
using Lucky9.Application._BusinessRules;
using Lucky9.Application._Features.ClientFeatures.Commands;
using Lucky9.Application.Commands;
using Lucky9.Application.Interfaces;
using Moq;
using Xunit;

namespace Application.UnitTest
{
    public class ClientPolicyTests
    {
        [Fact]
        public async Task AsserCreateUserPolicy_ValidUser_ReturnsUserObject()
        {
            // Arrange
            var addClientCommand = new AddClientCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "SecurePassword123"
            };

            var validatorMock = new Mock<IValidator<AddClientCommand>>();
            validatorMock.Setup(x => x.Validate(It.IsAny<AddClientCommand>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            var userRepoMock = new Mock<IPlayerRepository>();
            userRepoMock.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new User[] { });

            // Act
            var result = await ClientPolicy.AsserCreateUserPolicy(addClientCommand, validatorMock.Object, userRepoMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("john.doe@example.com", result.Email);
            Assert.Equal("User", result.Role);
            Assert.NotNull(result.Password);
            Assert.Equal("", result.Token);
            Assert.Equal("john.doe@example.com", result.Username);
            Assert.Null(result.ValidationResult);
        }

        [Fact]
        public async Task AsserCreateUserPolicy_DuplicateEmail_ReturnsUserObjectWithValidationResult()
        {
            // Arrange
            var addClientCommand = new AddClientCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "SecurePassword123"
            };

            var validatorMock = new Mock<IValidator<AddClientCommand>>();
            validatorMock.Setup(x => x.Validate(It.IsAny<AddClientCommand>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            var userRepoMock = new Mock<IPlayerRepository>();
            userRepoMock.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new User[] { new User { Email = "john.doe@example.com" } });

            // Act
            var result = await ClientPolicy.AsserCreateUserPolicy(addClientCommand, validatorMock.Object, userRepoMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Password); // Password should not be set in case of a duplicate email
            Assert.NotNull(result.ValidationResult);
            Assert.Single(result.ValidationResult);
            Assert.Equal("Email Already Exist", result.ValidationResult[0].ErrorMessage);
        }
    }
}
