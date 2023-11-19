using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky9.Domain.Entities;

namespace Lucky9.Infrastructure.Services;
public class GameService : IGameService
{
    private Random random;
    private List<Card> deck;

    public GameService()
    {
        random = new Random();
        this.InitializeDeck();
    }

    public Bet GenerateBettingCardsForTwo(Bet betgame)
    {
        List<string> serverCards = new List<string>();
        List<string> playerCards = new List<string>();
        


        for (int i = 1; i <= 4; i++)
        {
            Card randomCard = PullRandomCard();
            if (randomCard != null)
            {
                if (i % 2 == 0)
                {
                    serverCards.Add(randomCard.ToString());
                }
                else
                    playerCards.Add(randomCard.ToString());

                //cards.Add(randomCard);

                Console.WriteLine($"You pulled: {randomCard}");
            }
        }
       
        betgame.Game.ServerCards = string.Join(",", serverCards.Select(n => n.ToString()).ToArray()); 
        betgame.PlayerCards = string.Join(",", playerCards.Select(n => n.ToString()).ToArray()); 



        return betgame;
    }


    public Bet GenerateExtraCardForPlayer(Bet betgame)
    {
        List<Card> cards = new List<Card>();
       
        Card randomCard = PullRandomCard();
        betgame.PlayerCards = betgame.PlayerCards + ","+   randomCard.ToString();


        return betgame;
    }



    public Bet GenerateExtraCardForServer(Bet betgame)
    {
        List<Card> cards = new List<Card>();
        

        Card randomCard = PullRandomCard();
        betgame.Game.ServerCards = betgame.Game.ServerCards +"," + randomCard.ToString();


        return betgame;
    }

    void InitializeDeck()
    {
        string[] suits = { "h", "d", "f", "s" };
        string[] ranks = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };//, "Jack", "Queen", "King", "Ace" };

        deck = new List<Card>();

        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                deck.Add(new Card(suit, rank));
            }
        }
    }

    public Card PullRandomCard()
    {
        if (deck.Count == 0)
        {
            Console.WriteLine("No more cards in the deck!");
            return null;
        }

        int randomIndex = random.Next(deck.Count);
        Card randomCard = deck[randomIndex];
        deck.RemoveAt(randomIndex);

        return randomCard;
    }

}

