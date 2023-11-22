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
using Lucky9.Application._Features.Utilities.SecurityToken.Queries;

namespace Lucky9.WebUI.Controllers;
public class UtilityController : ApiControllerBase
{



    //[Authorize]
    [EnableCors]    
    [HttpPost("GetTestToken")]
    public async Task<IActionResult> GetTestToken([FromBody] AddGameClientCommand bet)
    {
        try
        {

            var param = new GetTestTokenQuery();

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





