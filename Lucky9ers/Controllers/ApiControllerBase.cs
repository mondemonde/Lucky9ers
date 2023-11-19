using MediatR;
using Microsoft.AspNetCore.Mvc;
using Lucky9.Application.Common.Security;
using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog;

namespace Lucky9.WebUI.Controllers;
[ApiController]
//[ApiExceptionFilter]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected void LogBadRequest(Exception e)
    {
        Log.Error($"{e.Message} /n {e.StackTrace}");
        //return new BadRequestObjectResult(new BadRequestException("Something went wrong"));

    }
}
