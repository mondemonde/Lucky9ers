using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lucky9.Application.Utilities.Swagger;
public class ReApplyOptionalRouteParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
       if( operation.Parameters.Count>0 )
        {
            var parameter = operation.Parameters[0];
            if (parameter.Name.StartsWith("search"))
            {
                parameter.AllowEmptyValue = true;
                parameter.Description = "Must check \"Send empty value\" or Swagger passes a comma for empty values otherwise";
                parameter.Required = false;
                parameter.Schema.Default = new OpenApiString(string.Empty);
                parameter.Schema.Nullable = true;

            }
        }


       
    }
}