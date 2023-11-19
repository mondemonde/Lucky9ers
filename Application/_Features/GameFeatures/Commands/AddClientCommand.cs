using Lucky9.Application.Interfaces;
using Lucky9.Application.Common.Models;
using Lucky9.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Lucky9.Application.IServices;
using Lucky9.Application._BusinessRules;
using Lucky9.Infrastructure.Services;

namespace Lucky9.Application._Features.GameFeatures.Commands;

public record AddGameClientCommand : IRequest<Bet>
{

    public string Email { get; set; }
    public double BetMoney { get; set; }


}

public class AddGameClientCommandHandler : IRequestHandler<AddGameClientCommand, Bet>
{
    private readonly IGameRepository _gameclientRepository;
    private readonly IBetRepository _betRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IGameService _gameService;


    public AddGameClientCommandHandler(IGameRepository GameclientRepository, IPlayerRepository playerRepository
        , IBetRepository betRepository
        , IGameService gameService
       )
    {
        _gameclientRepository = GameclientRepository;
        _betRepository = betRepository;
        _playerRepository = playerRepository;
        _gameService = gameService;
    }

    public async Task<Bet> Handle(AddGameClientCommand request, CancellationToken cancellationToken)
    {

        return await GameNewPolicy.AsserCreatePolicy(request
          , _playerRepository
          , _betRepository
          , _gameclientRepository, _gameService);
    }


}
