using Lucky9.Application.Interfaces;
using Lucky9.Application.Common.Models;
using Lucky9.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Lucky9.Application.IServices;
using Lucky9.Application._BusinessRules;
using Lucky9.Application.Common.Interfaces;
using Lucky9.Application.Helpers.Dto;
using FluentValidation;
using Lucky9.Application._Features.ClientFeatures.Commands;

namespace Lucky9.Application.Commands
{
    public record AuthenticateCommand : IRequest<TokenApiDto>
    {

       
        public string Email { get; set; }

        public string Password { get; set; }
    }


    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, TokenApiDto>
    {
        private readonly IPlayerRepository _clientRepository;
        IValidator<AuthenticateCommand> _validator;

        public AuthenticateCommandHandler(IPlayerRepository clientRepository
            ,IValidator<AuthenticateCommand> validator
            )
        {
            _validator = validator;
            _clientRepository = clientRepository;
            
        }

        public async Task<TokenApiDto> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {

           return  await AuthenticateClientPolicy.AssertAuthenticatePolicy(request,_validator, _clientRepository);

        }
    }
}
