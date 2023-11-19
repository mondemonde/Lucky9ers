using Lucky9.Domain.Entities;

namespace Lucky9.Infrastructure.Services;
public interface IGameService
{
    Bet GenerateBettingCardsForTwo(Bet game);
    Bet GenerateExtraCardForPlayer(Bet game);
    Bet GenerateExtraCardForServer(Bet game);
    Card PullRandomCard();
}