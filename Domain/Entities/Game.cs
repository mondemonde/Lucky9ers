using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky9.Domain.Entities;
public class Game:IEntity
{
    [Key] // This indicates that the property is the primary key.
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Game()
    {
        var date = DateTime.Now.Ticks;      

        Created = DateTime.Now;
    }

    

    public  string ServerCards { get; set; }

    public DateTime? Created { get; set; }

    [NotMapped]
    public List<ValidationDto> ValidationResult { get; set; }


}
