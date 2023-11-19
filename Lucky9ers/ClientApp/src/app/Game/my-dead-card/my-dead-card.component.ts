import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-my-dead-card',
  templateUrl: './my-dead-card.component.html',
  styleUrls: ['./my-dead-card.component.css']
})
export class MyDeadCardComponent implements OnInit {

  imageDefaults = "assets/images/card.png";


  constructor() { }

  ngOnInit() {
    this.imageDefaults = "assets/images/card.png";
  }

shuffle() {
  
}

}
