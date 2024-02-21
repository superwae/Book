import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AppUserServiceService } from '../Services/app-user-service.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule ,CommonModule,FormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private AppUserServiceService: AppUserServiceService
  ) { 
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

 
ngOnInit() {
  this.route.queryParams.subscribe(params => {
    let token = params['token'];
    let email = params['email'];
  });
}

  submitForgotPassword(): void {
    if (this.forgotPasswordForm.valid) {
    
       this.AppUserServiceService.forgotPassword(this.forgotPasswordForm.value.email).subscribe(
         (response) => {

         },
         (error) => {
         }
       );

      console.log(this.forgotPasswordForm.value);
      this.forgotPasswordForm.reset();
      
       //this.router.navigate(['/login']);
    }
  }
}