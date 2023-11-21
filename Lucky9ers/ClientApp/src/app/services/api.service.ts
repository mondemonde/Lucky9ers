import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MyGlobalService } from './my-Global.service';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  //private baseUrl: string = 'https://localhost:7058/api/User/';
  constructor(private http: HttpClient,private globalSvc:MyGlobalService) {}

  getUsers() {
    return this.http.get<any>(this.globalSvc.getAllUsers);
  }
}
