import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { register } from '../Models/registerModel';
import { login } from '../Models/LoginModel';
import { ResetPasswordComponent } from '../components/reset-password/reset-password.component';
import { ResetPasswordModel } from '../Models/ResetPasswordModel';

@Injectable({
  providedIn: 'root'
})

export class AppUserServiceService {

  constructor(private http :HttpClient) { }
  loginUser(userdata:login):Observable<login>{
    return this.http.post<login>('https://localhost:7139/api/AppUser/Login',userdata);

  }
  signup(userdata: register, role: string): Observable<register> {
    // Prepare HttpParams
    const params = new HttpParams().set('role', role);
    
    return this.http.post<register>('https://localhost:7139/api/AppUser/Register', userdata, { params });
  }
  forgotPassword(email: string): Observable<any> {
    return this.http.post('https://localhost:7139/api/AppUser/forgot-password', { email });
  }
  resetPassword(data: ResetPasswordModel): Observable<any> {
    return this.http.put('https://localhost:7139/api/AppUser/reset-password', data);
  }
}
