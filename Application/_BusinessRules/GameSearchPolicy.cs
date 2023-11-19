using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky9.Application.Interfaces;
using Lucky9.Domain.Entities;
using MediatR;

namespace Lucky9.Application._BusinessRules;
public static class GameFindPolicy
{
    public static async Task<List<Game>> AssertClientSearchPolicy(string? search,IGameRepository repo)
    {
        //rule #3: Searching in the "search field" should filter the list of Game by their Email
        //if none supplied in the parameter  i assume to show all

        IEnumerable<Game> list = new List<Game>();

        if (string.IsNullOrEmpty(search))
        {
            list = await repo.GetAll();
            return list.ToList();
        }

        var src = search.ToLower().Trim();


        return list.ToList();
    }


}
