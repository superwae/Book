import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import {  AppUsereService } from '../../services/app-user.service';
import { LoginResponse } from '../../Models/Loginresponse';


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

  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private AppUserService: AppUsereService,
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
  this.loginErrorMessage = null; 
  if (this.loginForm.valid) {
    this.AppUserService.loginUser(this.loginForm.value).subscribe({
      next: (response: LoginResponse) => {
        console.log(response);
        this.router.navigate(['/home-page']); 
      },
      error: (error) => {
        console.error(error);
        // Update this part based on how your API response structure for errors
        // Assuming the API returns a simple string message for login failures
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
    const formData = new FormData();

    formData.append('Name', this.registerForm.value.Name);
    formData.append('UserName', this.registerForm.value.UserName);
    formData.append('Email', this.registerForm.value.Email);
    formData.append('Password', this.registerForm.value.Password);
    formData.append('ConfirmNewPassword', this.registerForm.value.ConfirmNewPassword);
    formData.append('DTHDate', this.registerForm.value.DTHDate);
    formData.append('City', this.registerForm.value.City);
    formData.append('ProfilePictureUrl', this.registerForm.value.ProfilePictureUrl); // Assuming this is the file
    formData.append('About', this.registerForm.value.About);

    if (this.selectedFile) {
      formData.append('imageFile', this.selectedFile, this.selectedFile.name);
    }

    this.AppUserService.signup(formData, 'User').subscribe({
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

  
 
  
}