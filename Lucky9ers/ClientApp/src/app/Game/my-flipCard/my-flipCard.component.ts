import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
//import { MyGameService } from 'src/app/services/my-game.service';
//import { Bet, GameCommand } from 'src/app/models/game';
import { CardData } from '../game-card/game-card.component';
import { Subscription } from 'rxjs';
import { Bet, GameCommand } from '../../models/game';
import { MyGameService } from '../../services/my-game.service';
import { ToastrService } from 'ngx-toastr';
import { MyGlobalService } from '../../services/my-Global.service';


@Component({
  selector: 'app-my-flipCard',
  templateUrl: './my-flipCard.component.html',
  styleUrls: ['./my-flipCard.component.css'],
  animations: [
    trigger('flipState', [
      state('active', style({
        transform: 'rotateY(179deg)'
      })),
      state('inactive', style({
        transform: 'rotateY(0)'
      })),
      transition('active => inactive', animate('500ms ease-out')),
      transition('inactive => active', animate('500ms ease-in'))
    ])
  ]
})



export class MyFlipCardComponent implements OnInit, OnDestroy {

  @Input() CardId!: string;
  CardValue!: string;
  conf: any;
  imageUrl!: string;
  imageDefaults!: string
  data: CardData = {
    imageId: "pDGNBK9A0sk",
    state: "default"
  };

  message!: GameCommand;
  subscription!: Subscription;


  messageBet: Bet | null = null;
  subscriptionCommand!: Subscription;



  constructor(private svc: MyGameService,private toast:MyGlobalService) { }

  ngOnInit() {

    this.imageUrl = "assets/images/d1.png";
    this.imageDefaults = "assets/images/card.png";


    this.subscriptionCommand = this.svc.currentBet.subscribe((bet: any) => {
      if(bet ==null) return

      this.messageBet = bet as Bet;
      console.log(this.CardId);
      switch (this.CardId) {
        case "P1": {
          let split = this.messageBet.playerCards.split(',')
          //playerCards": "f-2,s-3",
          let str = split[0];
          let split2 =str.split('-');
          this.CardValue =split2[1];

          this.imageUrl = "assets/images/" + str.replace('-', '') + ".png";
          break;
        }
        case "P2": {
          //statements;
          let split = this.messageBet.playerCards.split(',')
          //playerCards": "f-2,s-3",
          let str = split[1];
          let split2 =str.split('-');
          this.CardValue =split2[1];

          this.imageUrl = "assets/images/" + str.replace('-', '') + ".png";
          break;
        }
        case "S1": {
          let split = this.messageBet.game.serverCards.split(',')
          //playerCards": "f-2,s-3",
          let str = split[0];
          let split2 =str.split('-');
          this.CardValue =split2[1];

          this.imageUrl = "assets/images/" + str.replace('-', '') + ".png";
          break;
        }
        case "S2":{
          let split = this.messageBet.game.serverCards.split(',')
          //playerCards": "f-2,s-3",
          let str = split[1];
          let split2 =str.split('-');
          this.CardValue =split2[1];

          this.imageUrl = "assets/images/" + str.replace('-', '') + ".png";


          break;
        }
        default: {
          //statements;
          break;
        }
      }

    });


    this.subscription = this.svc.currentMessage.subscribe(message => {
      this.message = message

      if (message == GameCommand.NewGame) {
        //this.cardClicked();
      }

    });


  }

  cardClicked() {
    if (this.data.state === "default") {
      this.data.state = "flipped";
    } else {
      this.data.state = "default";
    }


  }


  ngOnDestroy() {
    this.subscription.unsubscribe();

  }

  newMessageBet(bet: number) {

    this.svc.CurrentBet.betValue = bet;

    this.svc.changeBet(this.svc.CurrentBet);
  }

  flip: string = 'inactive';

  toggleFlip() {
    this.flip = (this.flip == 'inactive') ? 'active' : 'inactive';

    if( this.flip == "active")
    {
       if(this.CardValue)
       {
        if(this.CardId.startsWith("P"))
           this.toast.info('Note','You opened you card with number ' + this.CardValue);
        else
           this.toast.warn('Note','The server card is opened with number ' + this.CardValue);

        if(!this.svc.IsGameOver)
        {
          this.svc.updateGameResult(this);
        }
       }
    }
  }

}

