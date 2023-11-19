using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using Lucky9.Domain.Entities;

namespace Lucky9.Application.Interfaces;

    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
       Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    }

