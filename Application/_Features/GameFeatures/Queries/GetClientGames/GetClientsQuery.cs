using System.Collections.Generic;
using Lucky9.Application._BusinessRules;
using Lucky9.Application.Common.Models;
using Lucky9.Application.Interfaces;
using Lucky9.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lucky9.Application.Features.Queries
{
    public record GetGameClientsQuery : IRequest<List<Domain.Entities.Game>>
    {

        public string?  email { get; set; }
    }

    public class GetGameClientsQueryHandler : IRequestHandler<GetClientsQuery, List<Domain.Entities.Game>>
    {
        private readonly IGameRepository _gameRepository;

        public GetGameClientsQueryHandler(IGameRepository clientRepository)
        {
            _gameRepository = clientRepository;
        }

        public async Task<List<Domain.Entities.Game>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {

            IEnumerable<Player> list= new List<Player>();

           list = await  ClientFindPolicy.AssertClientSearchPolicy(request.search, _clientRepository);

            return list.ToList();
  
           
        }

    }
}
