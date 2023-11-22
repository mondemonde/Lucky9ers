using Lucky9.Application.Interfaces;
using Lucky9.Application.Common.Models;
using Lucky9.Application.IServices;
using Lucky9.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Helpers;

namespace Lucky9.Application._Features.Utilities.SecurityToken.Queries
{
    public record GetTestTokenQuery : IRequest<string>
    {

    }

    public class GetTestTokenQueryHandler : IRequestHandler<GetTestTokenQuery, string>
    {
        //private readonly ISecurityService _security;

        public GetTestTokenQueryHandler()
        {          

        }

        public async Task<string> Handle(GetTestTokenQuery request, CancellationToken cancellationToken)
        {
            string result = await Task.Run(() => { 
               
                return IdentityHelper.CreateTestToken();
            });

            return result;
        }
    }
}
