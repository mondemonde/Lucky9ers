using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky9.Domain.Entities;
public class txLog
{

    public txLog()
    {
        Created = DateTime.Now; 
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public double Amount { get; set; }



    public DateTime Created {  get; set; }

    public string Remarks { get; set; }

}
