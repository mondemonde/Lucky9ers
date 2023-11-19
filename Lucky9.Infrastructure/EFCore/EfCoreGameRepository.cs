
using Lucky9.Application.Common.Interfaces;
using Lucky9.Application.Interfaces;
using Lucky9.Domain.Entities;

namespace Lucky9.Infrastructure.Persistence;
    public class EfCoreGameRepository : EfCoreRepository<Game, AppDbContext>, IGameRepository
{
        public EfCoreGameRepository(AppDbContext context) : base(context)
        {

        }
        // We can add new methods specific to the movie repository here in the future
    }
