using Domain.Models;
using Lucky9.Domain.Entities;


namespace Lucky9.Application.Interfaces
{
    public interface IPlayerRepository:IRepository<User>
    {
        //_step #3 CRUD operatoins is coverd by generic IRepository
        //though any special method can be added here later 
       
    }

    public interface IGameRepository : IRepository<Game>
    {
        //_step #3 CRUD operatoins is coverd by generic IRepository
        //though any special method can be added here later 

    }

    public interface IBetRepository : IRepository<Bet>
    {
        //_step #3 CRUD operatoins is coverd by generic IRepository
        //though any special method can be added here later 

    }


}