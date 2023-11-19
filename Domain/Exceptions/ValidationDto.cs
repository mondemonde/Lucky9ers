using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky9.Domain.Exceptions;

public class ValidationDto
{
    public string Property { get; set; }
    public string ErrorMessage { get; set; }
}
