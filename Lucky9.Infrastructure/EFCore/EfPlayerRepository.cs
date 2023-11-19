
using Domain.Models;
using Lucky9.Application.Interfaces;
using Lucky9.Domain.Entities;
using Lucky9.Infrastructure.Persistence;

public class EfCorePlayerRepository : EfCoreRepository<User, AppDbContext>, IPlayerRepository
{
        public EfCorePlayerRepository(AppDbContext context) : base(context)
        {

        }
        // We can add new methods specific to the movie repository here in the future
    }

