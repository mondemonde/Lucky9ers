using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky9.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lucky9.Infrastructure.Data;
public class ApplicationUser : IdentityUser,IPlayer
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public double Money { get; set; }

   // public string Email {  get; set; }
}
