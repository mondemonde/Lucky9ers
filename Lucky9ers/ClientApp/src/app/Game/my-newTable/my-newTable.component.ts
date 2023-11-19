import { animate, group, query, state, style, transition, trigger } from '@angular/animations';
import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';

import { Subscription, timeout } from 'rxjs';
import { Bet, GameCommand } from '../../models/game';
import { MyGameService } from '../../services/my-game.service';
import { MyGlobalService } from '../../services/my-Global.service';
//import { Bet, GameCommand } from 'src/app/models/game';
//import { Bet, GameCards, GameCommand } from 'src/app/models/game';
//import { MyGameService } from 'src/app/services/my-game.service';

@Component({

  selector: 'app-my-newTable',
  templateUrl: './my-newTable.component.html',
  styleUrls: ['./my-newTable.component.css'],

  animations: [
    trigger('fade', [
      state('void', style({ opacity: 0 })),
      state('*', style({ opacity: 1 })),

      transition(':enter, :leave', [
        animate(2000)
      ])
    ]),
    trigger('fadeSlide', [
      transition(':enter', [
        group([
          query('.sd-1', [
            style({ opacity: 0, transform: 'translateX(-250px)' }),
            animate(
              500,
              style({ opacity: 1, transform: 'translateX(0)' })
            )
          ]),
          query('.sd-2', [
            style({ opacity: 0, transform: 'translateX(250px)' }),

            animate(
              500,
              style({ opacity: 1, transform: 'translateX(0)' })
            )
          ],{optional: true})
        ])
      ]),
      transition(':leave', [
        group([
          query('.sd-1', [
            animate(
              1000,
              style({ opacity: 0, transform: 'translateX(-250px)' })
            )
          ],{ optional: true }),
          query('.sd-2', [
            animate(
              1000,
              style({ opacity: 0, transform: 'translateX(250px)' })
            ),
          ],{optional: true})
        ])
      ])
    ])
  ]
})
export class MyNewTableComponent implements OnInit, OnDestroy {

  //backgrnd
  conf: any;
 // backgroundImageUrl: string = "https://images.unsplash.com/photo-1619043518800-7f14be467dca?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";
  //backgroundOpacity: number = .05;

//game service
  message: GameCommand = GameCommand.Idle;
  subscription!: Subscription;
  messageBet: Bet | null = null;
  subscriptionCommand!: Subscription;



  constructor(private svc: MyGameService,private MyGlobal:MyGlobalService) { }

  ngOnInit() {

    setTimeout(() => {
      IsStarted = null;
      this.MyGlobal.success("Ready!", "Click 'Draw Cards' to start");

      var element = document.getElementById("btnPlay") as HTMLButtonElement;
      element.disabled = false;

    }, 12000);


    //deck intro animation
    this.OnetimeIntro();

    this.subscriptionCommand = this.svc.currentBet.subscribe((bet: any) => {
      this.messageBet = bet as Bet;

    });


    this.subscription = this.svc.currentMessage.subscribe(message => {
      this.message = message

      if (message == GameCommand.NewGame) {
        //this toggle twice making disapper and reapper
        this.Shuffle();
        }else if (message == GameCommand.GameOver) {
          var element = document.getElementById("btnPlay") as HTMLButtonElement;
          element.disabled = false;
        }
    });


  }

  // animations-----------------------------------------
  showEvents1 = true;
  showEvents2 = true;
  toggleFlag = false;

  toggleEvents1() {
    if (!this.toggleFlag) {
      this.showEvents1 = !this.showEvents1
    }
  }

  toggleEvents2() {
    if (!this.toggleFlag) {
      this.showEvents2 = !this.showEvents2
    }
  }
  NewGameClicked() {
    var element = document.getElementById("btnPlay") as HTMLButtonElement;
    element.disabled = true;


    deck.fan();
    deck.shuffle();
    this.svc.changeMessage(GameCommand.NewGame);
    this.Shuffle();
    this.toggleEvents2();

    setTimeout(() => {

      this.MyGlobal.warn("Ready!","You can now open the cards including the server" );


    }, 3000); // Ani

  }

  Shuffle(): void {
    deck.shuffle();
    this.toggleEvents2();
    //setTimeout(this.toggleEvents2, 2000);
    // Trigger the animation after a delay
    setTimeout(() => {

      this.toggleEvents2();
      // this.toastr.success('you can open your cards now ' + card1.toString(), 'Cards are Ready!', this.conf);
    }, 2000); // Animation will start after 2 seconds


  }

  getAllCards(): void {

  }
  generateRandomNumber(): number {
    const min = 1,
      max = 9;
    return Math.floor(Math.random() * (max - min) + min);
  }
  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    if (this.subscriptionCommand) {
      this.subscriptionCommand.unsubscribe();
    }
  }

  //trigger bet event for system
  newMessageBet(bet: number) {

    this.svc.CurrentBet.betValue = bet;

    this.svc.changeBet(this.svc.CurrentBet);
  }

  OnetimeIntro() {

    IsShutdown = true;
    IsDemo = true;
    IsOneTimeIntro = true;

    console.log("IsOneTimeIntro")

    setTimeout(function () {
      IsStarted = null;
      DeckInit();

    }, 2000)

  }

}

declare function DeckShuffle(): void;
declare var IsStarted: any;
declare var IsDemo: any;
declare var IsShutdown: any;
declare var IsOneTimeIntro: any;
declare function DeckInit(): void;
//declare function ShuffleDealer(): void
declare var deck: any
