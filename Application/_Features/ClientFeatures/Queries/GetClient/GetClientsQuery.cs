using System.Collections.Generic;
using Domain.Models;
using Lucky9.Application._BusinessRules;
using Lucky9.Application.Common.Models;
using Lucky9.Application.Interfaces;
using Lucky9.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lucky9.Application.Features.Queries
{
    public record GetClientsQuery : IRequest<List<User>>
    {

        public string?  search { get; set; }
    }

    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, List<User>>
    {
        private readonly IPlayerRepository _clientRepository;

        public GetClientsQueryHandler(IPlayerRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<List<User>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {

            IEnumerable<User> list= new List<User>();

           list = await  ClientFindPolicy.AssertClientSearchPolicy(request.search, _clientRepository);

            return list.ToList();
  
           
        }
    }
}
