import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AppUserServiceService } from '../Services/app-user-service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  token: string='';
  email: string='';

  constructor(
    private fb: FormBuilder,
    private AppUserServiceService: AppUserServiceService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.resetPasswordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d).{8,}$')]],
      confirmPassword: ['', [Validators.required]]
    }, { validator: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
      this.email = params['email'];

      if (!this.token || !this.email) {
        // Handle the absence of token or email, perhaps redirecting the user or showing an error
        console.error('Token or email not provided in URL');
        // this.router.navigate(['/']); // Redirect to home or error page
      }
    });
  }

  passwordMatchValidator(fg: FormGroup): { [key: string]: boolean } | null {
    const password = fg.get('password')?.value;
    const confirmPassword = fg.get('confirmPassword')?.value;
    if (password && confirmPassword && password !== confirmPassword) {
      return { 'passwordMismatch': true };
    }
    return null;
  }

  submitResetPassword(): void {
    if (this.resetPasswordForm.valid && this.email && this.token) {
      const resetData = {
        email: this.email,
        token: this.token,
        newPassword: this.resetPasswordForm.value.newPassword,
        confirmPassword: this.resetPasswordForm.value.confirmPassword
      };
      console.log('Reset password data', resetData);

      this.AppUserServiceService.resetPassword(resetData).subscribe({
        next: (response) => {
          // Handle successful response
          console.log('Password reset successful', response);
          // Optionally navigate to the login page or show success message
          this.router.navigate(['/login']);
        },
        error: (error) => {
          // Handle error
          console.error('Password reset error', error);
        }
      });
    }
  }
}