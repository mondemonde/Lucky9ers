using Application.Helpers;
using Domain.Models;
using FluentValidation;
using Lucky9.Application._BusinessRules;
using Lucky9.Application.Commands;
using Lucky9.Application.Common.Models;
using Lucky9.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
namespace Application.UnitTest;

public class AuthenticateClientPolicyTests
{
    [Fact]
    public async Task AssertAuthenticatePolicy_ValidCredentials_ReturnsTokenApiDto()
    {
        var validPassword = "P@ssword123.com";
        // Arrange
        var authenticateCommand = new AuthenticateCommand
        {
            Email = "test@example.com",
            Password = validPassword
        };

        var validatorMock = new Mock<IValidator<AuthenticateCommand>>();
        validatorMock.Setup(x => x.Validate(It.IsAny<AuthenticateCommand>()))
            .Returns(new FluentValidation.Results.ValidationResult());

        var userRepoMock = new Mock<IPlayerRepository>();
        userRepoMock.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
            .ReturnsAsync(new User[] { new User { Email = "test@example.com"
                ,Username = "test@example.com"
                ,FirstName ="John"
                ,LastName ="Doe"
                , Password = PasswordHasher.HashPassword(validPassword) } });

        AppSettings settings = new AppSettings();
        settings.JWT.Issuer = "https://medium.com/@monde.justflowers/why-dont-just-encapsulate-the-business-rule-developer-driven-design-8d7b31670f00";
        settings.JWT.Audience = "https://medium.com/@monde.justflowers/why-dont-just-encapsulate-the-business-rule-developer-driven-design-8d7b31670f00";
        settings.JWT.Secret = "your-private-key";

        //var configurationMock = new Mock<IConfiguration>();
        //configurationMock.SetupGet(c => c["JWT:Issuer"]).Returns("https://medium.com/@monde.justflowers/why-dont-just-encapsulate-the-business-rule-developer-driven-design-8d7b31670f00");
        //configurationMock.SetupGet(c => c["JWT:Audience"]).Returns("https://medium.com/@monde.justflowers/why-dont-just-encapsulate-the-business-rule-developer-driven-design-8d7b31670f00");
        //configurationMock.SetupGet(c => c["JWT:Secret"]).Returns("your-private-key");


        // Act
        var result = await AuthenticateClientPolicy
        .AssertAuthenticatePolicy(authenticateCommand, validatorMock.Object
        ,userRepoMock.Object
        , settings);

        // Assert
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.Equal("test@example.com", result.Email);
        Assert.Null(result.ValidationResult);
    }

    [Fact]
    public async Task AssertAuthenticatePolicy_InvalidCredentials_ReturnsTokenApiDtoWithError()
    {
        // Arrange
        var authenticateCommand = new AuthenticateCommand
        {
            Email = "test@example.com",
            Password = "wrongPassword"
        };

        var validatorMock = new Mock<IValidator<AuthenticateCommand>>();
        validatorMock.Setup(x => x.Validate(It.IsAny<AuthenticateCommand>()))
            .Returns(new FluentValidation.Results.ValidationResult());

        var appSettingsMock = new Mock<AppSettings>();
        appSettingsMock.Setup(x => x.JWT).Returns(new JWT
        {
            Secret = "secret",
            Audience = "https://lucky9ers.azurewebsites.ne",
            Issuer = "https://lucky9ers.azurewebsites.ne"

        });

        var userRepoMock = new Mock<IPlayerRepository>();
        userRepoMock.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
            .ReturnsAsync(new User[] { new User { Email = "test@example.com"
               ,Username = "test@example.com"
                ,FirstName ="John"
                ,LastName ="Doe"
                , Password = PasswordHasher.HashPassword("correctPassword") } });

        // Act
        var result = await AuthenticateClientPolicy
        .AssertAuthenticatePolicy(authenticateCommand, validatorMock.Object
        , userRepoMock.Object, appSettingsMock.Object);

        // Assert
        Assert.True(string.IsNullOrEmpty(result.AccessToken));
        Assert.True(string.IsNullOrEmpty(result.RefreshToken));
      
        Assert.NotNull(result.ValidationResult);
        Assert.Equal(1, result.ValidationResult.Count);
        Assert.Equal("Password is Incorrect", result.ValidationResult[0].ErrorMessage);
    }
}

