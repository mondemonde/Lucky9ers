import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyGameHelperService {

constructor() { }

parseFirstDigit(cardValue:number):number {
  let i:number =cardValue;
  return i%10;
}

}
