

import { Bet, GameCommand, AddGameClientCommand } from '../models/game';
import { Injectable, OnDestroy, OnInit } from '@angular/core';

import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { mergeMap as _observableMergeMap, catchError as _observableCatch, tap, switchMap } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf, BehaviorSubject, Subscription } from 'rxjs';
import { throwError as observableThrowError } from 'rxjs';

// import { map, catchError } from 'rxjs/operators';

import { catchError, map } from "rxjs/operators";
import { AuthService } from './auth.service';
import { MyGlobalService } from './my-Global.service';
import { MyFlipCardComponent } from '../Game/my-flipCard/my-flipCard.component';
import { MyGameHelperService } from './my-game-helper.service';

@Injectable({
  providedIn: 'root'
})
export class MyGameService implements OnInit, OnDestroy {
  // Implement the ngOnDestroy method to unsubscribe
  ngOnDestroy() {
    this.userSubscription.unsubscribe();
    this.messageSource.subscribe();
    this.betMessageSource.unsubscribe();
  }

  CurrentBet!: Bet;
  public userName!: Observable<string>;
  currentUser: string | undefined;

  private userSubscription!: Subscription;
  private messageSource = new BehaviorSubject(GameCommand.Idle);
  currentMessage = this.messageSource.asObservable();

  betMessageSource = new BehaviorSubject<Bet | null>(null);
  currentBet = this.betMessageSource.asObservable();




  constructor(private http: HttpClient, private helper: MyGameHelperService,
    private myGlobal: MyGlobalService,
    private auth: AuthService,) {


  }
  ngOnInit(): void {
    this.userSubscription = this.userName/* your observable source */
      .subscribe((value: string) => {
        this.currentUser = value;
      });

  }
  changeBet(bet: Bet) {
    this.CurrentBet = bet;
    this.betMessageSource.next(bet)
  }

  P1: number = 0;
  P2: number = 0;
  S1: number = 0;
  S2: number = 0;

  Ptotal: number = 0;
  Stotal: number = 0;
  IsGameOver: boolean = false;
  CardCount: number = 0;

  resetTotals(): void {
    this.P1 = 0;
    this.P2 = 0;
    this.S1 = 0;
    this.S2 = 0;

    this.Ptotal = 0;
    this.Stotal = 0;
    this.IsGameOver = false;
    this.CardCount = 0;
  }

  updateGameResult(card: MyFlipCardComponent) {
    this.CardCount += 1;
    if (card.CardId == "P1" || card.CardId == "P2") {
      this.Ptotal += parseInt(card.CardValue);

    } else if (card.CardId == "S1" || card.CardId == "S2") {
      this.Stotal += parseInt(card.CardValue);
    }

    if (this.CardCount == 4) {
      this.IsGameOver = true;

      this.changeMessage(GameCommand.GameOver);

      this.Ptotal = this.helper.parseFirstDigit(this.Ptotal);
      this.Stotal = this.helper.parseFirstDigit(this.Stotal);

      if (this.Ptotal > this.Stotal) {
        this.myGlobal.success("Greate!", "You won!");

      }
      else if (this.Stotal > this.Ptotal) {
        this.myGlobal.error("Oops!", "Sorry mate,you lost");
      } else {

        this.myGlobal.warn("Whew!", "It's a draw!");

      }

    }

  }

  changeMessage(message: GameCommand) {
    switch (message) {
      case GameCommand.NewGame: {

        this.resetTotals();
        this.userName = this.auth.getfullNameFromToken();

        const obj = {} as AddGameClientCommand;
        obj.email = AuthService.currentUser;
        obj.betMoney = 10000;

        console.log(this.myGlobal.NewGameUrl);

        this.makeHttpPostRequest2(this.myGlobal.NewGameUrl, obj);

        break;

      }

      case GameCommand.DrawCards: {
        //statements;
        break;
      }
      default: {
        //statements;
        break;
      }
    }
    this.messageSource.next(message)
  }


  makeHttpPostRequest(url: string, body: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    const content_ = JSON.stringify(body);

    // Use an options object to include headers
    const options = {
      headers: headers,
    };

    return this.http.post(url, content_, options).pipe(
      map((response: any) => {
        // Process and transform the response here
        let bet = response as Bet; // Ensure response structure matches the Bet type
        this.changeBet(bet);
        return response;
      }),
      catchError((error: any) => {
        // Handle and log errors here
        console.error('Error:', error);
        throw error; // You can throw or return an error here if needed
      })
    );
  }



  makeHttpGetRequest(url: string, body: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    //const content_ = body;//JSON.stringify(body);

    const options = { headers };

    return this.http.get(url, options).pipe(
      map((response: any) => {
        // Process and transform the response here
        return response;
      }),
      catchError((error: any) => {
        // Handle and log errors here
        console.error('Error:', error);
        throw error; // You can throw or return an error here if needed
      })
    );
  }

  //using this for some unknown reson maybe cors issue
  makeHttpPostRequest2(url: string, body: any) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    //const content_ = JSON.stringify(body);

    const options = { headers };


    const requestOptions = {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json', // Set the appropriate content type
      },
      body: JSON.stringify(body), // Convert the data object to a JSON string
    };

    fetch(url, requestOptions)
      .then((response) => {
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json(); // Parse the response JSON
      })
      .then((responseData) => {
        // Handle the successful response here
        console.log('POST request successful', responseData);
        this.changeBet(responseData);

      })
      .catch((error) => {
        // Handle errors here
        console.error('POST request error', error);
      });

  }

}


