import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AppUserServiceService } from '../Services/app-user-service.service';

@Component({
  selector: 'app-login-register',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,FormsModule,RouterModule],
  templateUrl: './login-register.component.html',
  styleUrl: './login-register.component.css'
})


export class LoginRegisterComponent implements OnInit {
  loginForm: FormGroup;
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private AppUserServiceService: AppUserServiceService,
    private router: Router
  ) { 
    this.router.events.subscribe(event => {
      console.log(event);
    });
    this.loginForm = this.fb.group({
      UserName: ['', [Validators.required]],
      Password: ['', [Validators.required]]
    });

    this.registerForm = this.fb.group({
      Name: [''], 
      UserName: ['', [Validators.required]],
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d).{8,}$')]],
      ConfirmNewPassword: [''],
      ConfirmNewEmail: [''],
      DTHDate: ['', [Validators.required]],
      City: ['', [Validators.required]],
      ProfilePictureUrl: ['test'],
      About: ['test']
    }, { validators: this.checkPasswords });
  }

  ngOnInit(): void {}

  login(): void {
    if (this.loginForm.valid) {
      this.AppUserServiceService.loginUser(this.loginForm.value).subscribe({
        next: (data) => console.log(data),
        error: (error) => console.log(error)
      });
    }
  }

  Register(): void {
    this.registerForm.value.ConfirmNewEmail=this.registerForm.value.Email;
    this.registerForm.value.Name=this.registerForm.value.UserName;
    if (this.registerForm.valid) {
      let registerData = this.registerForm.value;
      console.log('Register form data:', registerData);

      this.AppUserServiceService.signup(registerData, 'User').subscribe({
        next: (data) => console.log(data),
        error: (error) => console.log(error)
      });
    }
  }

  checkPasswords(group: FormGroup) { 
    let pass = group.get('Password')?.value;
    let confirmPass = group.get('ConfirmNewPassword')?.value;
    return pass === confirmPass ? null : { notSame: true };
  }
  forgotPassword(event: Event): void {
    event.preventDefault();
    this.router.navigate(['/forgot-password']);
  }
  
}