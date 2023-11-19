using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky9.Application.IServices;

public interface IEmailService
{
    public Task Send(string email, string message);
}
