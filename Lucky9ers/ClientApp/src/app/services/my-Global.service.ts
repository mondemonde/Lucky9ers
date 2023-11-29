import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyGlobalService {

  readonly baseUrl = 'https://localhost:7235';
  //readonly baseUrl = 'https://lucky9ers.azurewebsites.net';

  readonly loginUrl = this.baseUrl + '/api/user/authenticate';
  readonly registerUrl = this.baseUrl + '/api/user/register';
  readonly NewGameUrl = this.baseUrl + '/api/Game/creategame';
  readonly swaggerUrl = this.baseUrl + '/swagger/index.html';
  readonly getAllUsers = this.baseUrl + '/api/User/find';


  readonly applicationName = 'Lucky9ers';

  constructor(private toast: ToastrService) { }


  success(title: string, message: string): void {

    this.toast.success(message, title, { "positionClass": "toast-top-left" });

  }


  error(title: string, message: string): void {

    this.toast.error(message, title, { "positionClass": "toast-top-left" });

  }


  warn(title: string, message: string): void {

    this.toast.warning(message, title, { "positionClass": "toast-top-left" });

  }

  info(title: string, message: string): void {

    this.toast.info(message, title, { "positionClass": "toast-top-left" });

  }

}
