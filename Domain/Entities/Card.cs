using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky9.Domain.Enums;

namespace Lucky9.Domain.Entities;
public class Card
{

    public string Suit { get; set; }
    public int Rank { get; set; }

    public Card()
    {
      
    }


    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = Convert.ToInt32(rank);
        Id = ToString();
    }

    public override string ToString()
    {
        return $"{Suit}-{Rank}";
    }

    public string imageName()
    {
        return $"{Suit}{Rank}.png";
    }

    [Key]
    public string Id { get; set; }

}
