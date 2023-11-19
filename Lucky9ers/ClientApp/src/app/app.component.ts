;
import { Component, Renderer2, ElementRef, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Lucky9';

  //script_file = 'https://deck-of-cards.js.org/dist/deck.min.js';
  constructor(private renderer: Renderer2, private el: ElementRef) { }

  ngOnInit() {

   IsStarted = null
       DeckInit();
  }

  loadScript(src: string) {
    const script = this.renderer.createElement('script');
    script.type = 'text/javascript';
    script.src = src;
    script.async = true;

    // Append the script element to the body or any other element you want
    this.renderer.appendChild(this.el.nativeElement, script);
  }
}


declare function DeckShuffle():void ;
declare var IsStarted:any;
declare function DeckInit():void ;
//declare function ShuffleDealer(): void
declare var deck: any
