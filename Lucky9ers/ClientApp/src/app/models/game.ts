export interface Bet {
  id: string
  playerId: string
  player: Player
  betValue: number
  gameId: string
  game: Game
  playerCards: string
  created: any
}

export interface Player {
  firstName: string
  lastName: string
  money: number
  email: string
  id: string
  created: any
}

export interface Game {
  id: string
  serverCards: string
  created: string
}


export interface AddGameClientCommand{
  email: string
  betMoney: number
}

export enum GameCommand {
  Idle,
  NewGame,

  DrawCards,
  DrawExtraCard,
  GameOver
}

export enum GameCards {
  S1,
  S2,
  S3,
  P1,
  P2,
  P3

}
