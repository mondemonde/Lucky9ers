using Microsoft.EntityFrameworkCore;
using Lucky9.Domain.Entities;
using System.Numerics;
using Domain.Models;

namespace Lucky9.Application.Common.Interfaces;
public interface IDataContext
{
    DbSet<User> Users { get; }
    DbSet<Game> Games { get; }
    DbSet<Card> Cards { get; }


    Task<int> SaveChangesAsync();
}
