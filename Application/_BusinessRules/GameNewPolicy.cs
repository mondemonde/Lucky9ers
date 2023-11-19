using System;
using Lucky9.Application.Interfaces;
using Lucky9.Application.IServices;
using Lucky9.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Lucky9.Infrastructure.Services;
using Lucky9.Application._Features.GameFeatures.Commands;

namespace Lucky9.Application._BusinessRules;

public static  class GameNewPolicy
{
    /// <summary>
    ///rule #1.1 create - All fields are required
    /// </summary>
    public class AddGameClientCommandValidator : AbstractValidator<AddGameClientCommand>
    {
        public AddGameClientCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("email is required.");         
         
            RuleFor(x => x.BetMoney).NotEmpty().WithMessage("Bet is required.");
            //RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.");
        }

    }


    /// <summary>
    ///rule 1.2 Send an email and sync documents after a client is created (using the mock repositories provided)
    /// </summary>
    public static async Task<Bet> AsserCreatePolicy(AddGameClientCommand request
        , IPlayerRepository playerRepository
        , IBetRepository betRepo
        , IGameRepository repoGame,IGameService gameService)
    {


        //get player
        var p = await playerRepository.FindAsync(p => p.Email == request.Email);
        var player = p.FirstOrDefault();     


        var bet = new Bet
        {
            Player = player,
            BetValue = request.BetMoney
        };


        var game = new Game();
        var newGame =  repoGame.Add(game);
        //generate cards..
        bet.GameId = newGame.Result.Id;
        bet.Game = newGame.Result;
       var newBet =  gameService.GenerateBettingCardsForTwo(bet);

        await betRepo.Add(bet);
       // await betRepo.SaveChangesAsync(); 
       
        return bet;

    }
}



