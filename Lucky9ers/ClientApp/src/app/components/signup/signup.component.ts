import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../../helpers/validationform';
import { Router } from '@angular/router';
//import { BaseComponent } from 'src/app/shared/base-component';
import { takeUntil } from 'rxjs';
import { BaseComponent } from '../../shared/base-component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent extends  BaseComponent implements OnInit {

  public signUpForm!: FormGroup;
  type: string = 'password';
  isText: boolean = false;
  eyeIcon:string = "fa-eye-slash"
  constructor(private fb : FormBuilder,private toast:ToastrService,
     private auth: AuthService, private router: Router) {super() }

  ngOnInit() {
    this.signUpForm = this.fb.group({
      firstName:['', Validators.required],
      lastName:['', Validators.required],
      //userName:['', Validators.required],
      email:['', Validators.required],
      password:['', Validators.required]
    })
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = 'fa-eye' : this.eyeIcon = 'fa-eye-slash'
    this.isText ? this.type = 'text' : this.type = 'password'
  }

  onSubmit() {
    console.log(this.signUpForm.value);
    if (this.signUpForm.valid) {
      console.log(this.signUpForm.value);
      let signUpObj = {
        ...this.signUpForm.value,
        role:'',
        token:''
      }
      this.auth.signUp(signUpObj)
      .pipe(takeUntil(this.stop$))
      .subscribe({
        next:(res=>{
          console.log(res.message);


          if(res.ValidationResult!==null) {
            this.toast.warning("Oops", res.ValidationResults[0]);
            return res;
          }

          this.signUpForm.reset();
          this.router.navigate(['login']);
          alert(res.message)
          AuthService.currentUser = res.email;
        }),
        error:(err=>{
          //alert(err?.error.message)
          let error= err.error.message.errorMessage;
            this.toast.warning(error,"Oops!");}
        )
      })
    } else {
      ValidateForm.validateAllFormFields(this.signUpForm); //{7}
    }
  }

}
