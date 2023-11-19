using Microsoft.AspNetCore.Mvc;
using Lucky9.Domain.Entities;
using Lucky9.Application.Features.Queries;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Lucky9.Infrastructure.Data;
using Lucky9.Application.Commands;
using Lucky9.Application._Features.GameFeatures.Commands;
using Microsoft.AspNetCore.Cors;

namespace Lucky9.WebUI.Controllers;
public class GameController : ApiControllerBase
{




    [EnableCors("MyPolicy")]    
    [HttpPost("creategame")]

    public async Task<IActionResult> CreateGame([FromBody] AddGameClientCommand bet)
    {
        try
        {
            //relay message to applicaiton layer...
            var param = new AddGameClientCommand
            {
                Email = bet.Email,
                BetMoney = bet.BetMoney

            };

            var result = await Mediator.Send(param);

            return Ok(result);

        }
        catch (Exception e)
        {

            LogBadRequest(e);
            return BadRequest(e.Message);
        }



    }
}





