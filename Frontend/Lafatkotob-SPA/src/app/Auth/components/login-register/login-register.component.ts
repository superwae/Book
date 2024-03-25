import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import {  AppUsereService } from '../../services/appUserService/app-user.service';
import { LoginResponse } from '../../Models/Loginresponse';
import { GenreService } from '../../services/GenreService/genre.service';
import { registerModel } from '../../Models/registerModel';


@Component({
  selector: 'app-login-register',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,FormsModule,RouterModule,RouterLink],
  templateUrl: './login-register.component.html',
  styleUrl: './login-register.component.css'
})


export class LoginRegisterComponent implements OnInit {
  loginForm: FormGroup;
  registerForm: FormGroup;
  loginErrorMessage: string | null = null;
  userDetails: registerModel | null = null;

  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private AppUserService: AppUsereService,
    private router: Router,
    private userPreferences: GenreService
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
      About: ['test']
    }, { validators: this.checkPasswords });
  }

  ngOnInit(): void {}

  login(): void {
  this.loginErrorMessage = null; 
  if (this.loginForm.valid) {
    this.AppUserService.loginUser(this.loginForm.value).subscribe({
      next: (response: LoginResponse) => {
        console.log(response);
      },
      error: (error) => {
        console.error(error);
        this.loginErrorMessage = "Invalid username or password.";
      }
    });
  }
}


onFileSelected(event: Event): void {
  const file = (event.target as HTMLInputElement).files?.[0];
  if (file) {
    this.selectedFile = file;
    // Optionally: Display file name or preview the image
  }
}

Register(): void {
  if (this.registerForm.valid) {

    const userDetails: registerModel = {
      Name: this.registerForm.value.UserName, 
      UserName: this.registerForm.value.UserName,
      Email: this.registerForm.value.Email,
      Password: this.registerForm.value.Password,
      ConfirmNewPassword: this.registerForm.value.ConfirmNewPassword,
      DthDate: this.registerForm.value.DTHDate, 
      City: this.registerForm.value.City,
      About: this.registerForm.value.About,
      ConfirmNewEmail: this.registerForm.value.Email,
      ProfilePictureUrl: "test", 
    };


    // Now pass the userDetails object to the userPreferences service
    this.userPreferences.setUserDetails(userDetails);

    // Navigate to the userPreferences component
    this.router.navigate(['/userPreferences']);
 
  }
}
  checkPasswords(group: FormGroup) { 
    let pass = group.get('Password')?.value;
    let confirmPass = group.get('ConfirmNewPassword')?.value;
    return pass === confirmPass ? null : { notSame: true };
  }
  
}