using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Lucky9.Domain.Entities;
public class Bet : IEntity
{
      public Bet()
    {
        Created = DateTime.Now;
    }

    [Key] // This indicates that the property is the primary key.
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(Player))]
    public string PlayerId { get; set; }


    [NotMapped]
    public virtual User Player { get; set; }

    public  double BetValue { get; set; }

    [ForeignKey(nameof(Game))]
    public  int GameId { get; set; }

    public virtual Game Game { get; set; }

    public string PlayerCards { get; set; }
    public DateTime? Created {  get; set; }

    [NotMapped]
    public List<ValidationDto> ValidationResult { get; set; }
}
