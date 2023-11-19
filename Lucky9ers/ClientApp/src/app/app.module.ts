
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './shared/app-routing.module';
import { AppComponent } from './app.component';
//import { LoginComponent } from './components/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgToastModule } from 'ng-angular-popup';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
//import { MyNewTableComponent } from './Game/my-newTable/my-newTable.component';
import { MyFlipCardComponent } from './Game/my-flipCard/my-flipCard.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GameCardComponent } from './Game/game-card/game-card.component';
import { SignupComponent } from './components/signup/signup.component';
import { LoginComponent } from './components/login/login.component';
import { MyNewTableComponent } from './Game/my-newTable/my-newTable.component';

import { MyDeadCardComponent } from './Game/my-dead-card/my-dead-card.component';
import { ToastrModule } from 'ngx-toastr';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    DashboardComponent,
    MyFlipCardComponent,
    MyNewTableComponent,
    //MyDealerComponent,
    GameCardComponent,
    //MyDeckCardsComponent,
    MyDeadCardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    NgToastModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
