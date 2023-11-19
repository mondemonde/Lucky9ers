using Lucky9.Application.Interfaces;
using Lucky9.Application.Common.Models;
using Lucky9.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Lucky9.Application.IServices;
using Lucky9.Application._BusinessRules;
using Domain.Models;
using FluentValidation;
using Lucky9.Application.Commands;

namespace Lucky9.Application._Features.ClientFeatures.Commands
{
    public record AddClientCommand : IRequest<User>
    {      

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class AddClientCommandHandler : IRequestHandler<AddClientCommand, User>
    {
        private readonly IPlayerRepository _clientRepository;
        IValidator<AddClientCommand> _validator;

        public AddClientCommandHandler(IPlayerRepository clientRepository,
            IValidator<AddClientCommand> validator
            )
        {
            _clientRepository = clientRepository;
            _validator = validator;
        }

        public async Task<User> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {

            return await ClientPolicy.AsserCreateUserPolicy(request,_validator, _clientRepository);

        }

      
    }
}
