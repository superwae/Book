import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { login } from '../Models/LoginModel';
import { ResetPasswordModel } from '../Models/ResetPasswordModel';
import { AppUserModel } from '../Models/AppUserModel';
import { registerModel } from '../Models/registerModel';
import { LoginResponse } from '../Models/Loginresponse';
import { SetUserHistoryModel } from '../Models/SetUserHistoryModel';

@Injectable({
  providedIn: 'root'
})

export class AppUsereService {

  constructor(private http :HttpClient) { }
  loginUser(loginData: login): Observable<LoginResponse> {
        return this.http.post<LoginResponse>('https://localhost:7139/api/AppUser/Login',loginData);

  }
  signup(userdata: registerModel, role: string): Observable<registerModel> {
    // Prepare HttpParams
    const params = new HttpParams().set('role', role);
    
    return this.http.post<registerModel>('https://localhost:7139/api/AppUser/Register', userdata, { params });
  }
  forgotPassword(email: string): Observable<any> {
    return this.http.post('https://localhost:7139/api/AppUser/forgot-password', { email });
  }
  resetPassword(data: ResetPasswordModel): Observable<any> {
    return this.http.put('https://localhost:7139/api/AppUser/reset-password', data);
  }
  getUserById(id: string): Observable<AppUserModel> {
    return this.http.get<AppUserModel>(`https://localhost:7139/api/AppUser/getbyid?userId=${id}`);
  }
  getALlUser(): Observable<AppUserModel[]> {
    return this.http.get<AppUserModel[]>('https://localhost:7139/api/AppUser/getall');
  }
  updateUserHistoryId(data:SetUserHistoryModel): Observable<any> {
    console.log(data.HistoryId, data.UserId);
    return this.http.put('https://localhost:7139/api/AppUser/set-historyId', data );
           
  }
}
