
using Lucky9.Application.Interfaces;
using Lucky9.Domain.Entities;
using Lucky9.Infrastructure.Persistence;

public class EfCoreBetRepository : EfCoreRepository<Bet, AppDbContext>, IBetRepository
{
        public EfCoreBetRepository(AppDbContext context) : base(context)
        {

        }
        // We can add new methods specific to the movie repository here in the future
    }

