using AutoFixture;
using FluentValidation.TestHelper;
using Lucky9.Application._Features.ClientFeatures.Commands;
using Lucky9.Application.Interfaces;
using Lucky9.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using static Lucky9.Application._BusinessRules.ClientPolicy;

namespace Application.UnitTest;


public class UnitTestClientPolicy
{
    AddClientCommandValidator _addValidator { get; set; }

    private readonly Fixture _fixture;
    readonly IPlayerRepository _clientRepository;

    public UnitTestClientPolicy()
    {
        
        _addValidator = new AddClientCommandValidator();
        _fixture = new Fixture();

        // Build in-memory DbContextOptions.
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("valocityDb").Options;
        var context = new AppDbContext(options);
        _clientRepository = new EfCorePlayerRepository(context);
    }

    [Fact]
    public async void When_Adding_All_Field_Should_Be_Required()
    {

        //arrange
        AddClientCommand clientCommand = new AddClientCommand();       

        //act
        var response = await _addValidator.TestValidateAsync(clientCommand);

        //assert
        response.ShouldHaveValidationErrorFor(x => x.Email);
        response.ShouldHaveValidationErrorFor(x => x.FirstName);
        response.ShouldHaveValidationErrorFor(x => x.LastName);
        response.ShouldHaveValidationErrorFor(x => x.Password);

    }
   
   
}