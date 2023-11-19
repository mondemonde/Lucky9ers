using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Lucky9.Application.Interfaces;
using Lucky9.Domain.Entities;
using MediatR;

namespace Lucky9.Application._BusinessRules;
public static class ClientFindPolicy
{
    public static async Task<List<User>> AssertClientSearchPolicy(string? search,IPlayerRepository repo)
    {
        //rule #3: Searching in the "search field" should filter the list of clients by their first or last name
        //if none supplied in the parameter  i assume to show all

        IEnumerable<User> list = new List<User>();

        if (string.IsNullOrEmpty(search))
        {
            list = await repo.GetAll();
            return list.ToList();
        }

        var src = search.ToLower().Trim();

        list = await repo.FindAsync(
            c => c.FirstName.ToLower().Contains(src)
            || c.LastName.ToLower().Contains(src));

        return list.ToList();
    }


}
