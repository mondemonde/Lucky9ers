import { JwtPayload } from './../../../../node_modules/jwt-decode/build/cjs/index.d';
import { Router } from '@angular/router';
import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../../helpers/validationform';
import { NgToastService } from 'ng-angular-popup';
//import { UserStoreService } from 'src/app/services/user-store.service';
import { takeUntil, tap } from 'rxjs';
import { BaseComponent } from '../../shared/base-component';
import { UserStoreService } from '../../services/user-store.service';
import { MyGameService } from '../../services/my-game.service';
import { MyGlobalService } from '../../services/my-Global.service';
import { AddGameClientCommand } from '../../models/game';
import { ToastrService } from 'ngx-toastr';
//import { BaseComponent } from 'src/app/shared/base-component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent extends  BaseComponent implements OnInit {
  public loginForm!: FormGroup;
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';
  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private toast: ToastrService,
    private userStore: UserStoreService,
    private myGlobal:MyGlobalService,
    private gService:MyGameService ) {super();}

  ngOnInit() {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }
  onSubmit() {
    console.log(this.loginForm.value);
    if (this.loginForm.valid) {
      console.log(this.loginForm.value);
      this.auth.signIn(this.loginForm.value)
      .pipe(takeUntil(this.stop$)).subscribe({
        next: (res) => {
          console.log(res);
          this.loginForm.reset();


          this.auth.storeToken(res.accessToken);
          this.auth.storeRefreshToken(res.refreshToken);
          const tokenPayload = this.auth.decodedToken();
          this.userStore.setFullNameForStore(tokenPayload.name);
          this.userStore.setRoleForStore(tokenPayload.role);

          this.toast.success('Welcome ' + AuthService.currentFirstName ,"Hey!");

          this.router.navigate(['dashboard'])
          AuthService.currentUser = res.email;
        },
        error: (err) => {
          let error= err.error.message.errorMessage;
          this.toast.warning(error,"Oops!");
          console.log(error);
        },
      });
    } else {
      ValidateForm.validateAllFormFields(this.loginForm);
    }
  }

  onPostTest(){


    // const obj = {} as AddGameClientCommand;
    // obj.email = 'rgalvez@blastasia.com';//AuthService.currentUser;
    // obj.betMoney = 10000;

    this.toast.success("Hi", "hello world!");
    this.toast.info("Information","Welcome");

  }


}
