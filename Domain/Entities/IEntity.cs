using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Lucky9.Domain.Entities;
public interface IEntity
{
    public int Id { get; set; }

    public DateTime? Created { get; set; }

    [NotMapped]
   public List<ValidationDto> ValidationResult { get; set; }
}
