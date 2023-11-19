import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from '@angular/router';
//import {JwtHelperService} from '@auth0/angular-jwt'
import { TokenApiModel } from '../models/token-api.model';
import { Observable, catchError, map, switchMap, tap } from 'rxjs';
import { MyGlobalService } from './my-Global.service';
//import { JwtHelperService } from '@auth0/angular-jwt';
import * as jwt_decode from "jwt-decode";
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string =this.MyGlobal.baseUrl; //'https://localhost:7058/api';
  private userPayload:any;
  public static  currentUser: string ;
  public static  currentFirstName: string ;

  constructor(private http: HttpClient, private router: Router,private MyGlobal:MyGlobalService) {




   }

  signUp(userObj: any) : Observable<any> {
    console.log(this.MyGlobal.registerUrl);
    return this.http.post<any>(this.MyGlobal.registerUrl, userObj)
    .pipe(
      //map((response: any) => {JSON.parse(response);}),
      tap((response: any) => {
        console.log(response);
        AuthService.currentUser = response.Email;
      })
     );

  }

  signIn(loginObj : any): Observable<any> {
    return this.http.post<any>(this.MyGlobal.loginUrl,loginObj)
     .pipe(
      //map((response: any) => {JSON.parse(response);}),
      tap((response: any) => {

        const decodedToken = jwtDecode(response.accessToken);

        console.log(decodedToken);

        AuthService.currentFirstName = response.firstName;
        AuthService.currentUser = response.email;
        this.userPayload =   decodedToken;//this.decodedToken();
        console.log(response);
        //AuthService.currentUser = response;
      })
     );
  }

  signOut(){
    localStorage.clear();
    this.router.navigate(['login'])
  }

  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue)
  }
  storeRefreshToken(tokenValue: string){
    localStorage.setItem('refreshToken', tokenValue)
  }

  getToken(){
    return localStorage.getItem('token')
  }
  getRefreshToken(){
    return localStorage.getItem('refreshToken')
  }

  isLoggedIn(): boolean{
    let result = !!localStorage.getItem('token')
    return result;
  }

  decodedToken():any{
    //const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;


    if (!token||token == undefined ) {
      // Redirect to login page
     // this.router.navigate(['/login']);
     return null;
    }

  else
  {
    const decodedToken = jwtDecode(token);
    console.log(decodedToken);
    return decodedToken;// jwtHelper.decodeToken(token)

  }

  }

  getfullNameFromToken(){
    if(this.userPayload)
    return this.userPayload.name;
  }

  getRoleFromToken(){
    if(this.userPayload)
    return this.userPayload.role;
  }

  renewToken(tokenApi : TokenApiModel){
    return this.http.post<any>(`${this.baseUrl}/refresh`, tokenApi)
  }
}
