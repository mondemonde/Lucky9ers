
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lucky9.Domain.Entities;

[Table("PlayerView")]
[Keyless]
public class Player : IPlayer,IEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public double Money { get; set; }

    public string Email { get; set; }
    public string Id {  get; set; }

    [NotMapped]
    public DateTime? Created {  get; set; }
}
