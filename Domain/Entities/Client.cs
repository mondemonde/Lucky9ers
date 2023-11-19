using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky9.Domain.Entities
{

    public class Player
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }    

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public double Money { get; set; }
    }


}


