import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map, pipe } from 'rxjs';
import { login } from '../../Models/LoginModel';
import { ResetPasswordModel } from '../../Models/ResetPasswordModel';
import { AppUserModel } from '../../Models/AppUserModel';
import { LoginResponse } from '../../Models/Loginresponse';
import { SetUserHistoryModel } from '../../Models/SetUserHistoryModel';
import { Route, Router } from '@angular/router';
import { MyTokenPayload } from '../../../shared/Models/MyTokenPayload';

@Injectable({
  providedIn: 'root'
})

export class AppUsereService {
  private baseUrl = 'https://localhost:7139/api/AppUser';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.isLoggedIn());
  public isAuthenticated = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient,private router:Router) {

   }
   loginUser(loginData: login): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/Login`, loginData)
      .pipe(map(response => {
        console.log("respose bbb "+response);
        localStorage.setItem('token', response.token);
        localStorage.setItem('userId', response.userId);
        localStorage.setItem('userName', response.userName);
        localStorage.setItem('profilePicture', response.profilePicture);


        this.isAuthenticatedSubject.next(true);
        window.location.href = '/home';
        return response;
      }));
  }

  signup(formData: FormData, role: string): Observable<any> {
    const params = new HttpParams().set('role', role);
    return this.http.post(`${this.baseUrl}/Register`, formData, { params });
  }
  

  forgotPassword(email: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/forgot-password`, { email });
  }

  resetPassword(data: ResetPasswordModel): Observable<any> {
    return this.http.put(`${this.baseUrl}/reset-password`, data);
  }

  getUserById(id: string): Observable<AppUserModel> {
    return this.http.get<AppUserModel>(`${this.baseUrl}/getbyid?userId=${id}`);
  }

  getALlUser(): Observable<AppUserModel[]> {
    return this.http.get<AppUserModel[]>(`${this.baseUrl}/getall`);
  }

  updateUserHistoryId(data: SetUserHistoryModel): Observable<any> {
    return this.http.put(`${this.baseUrl}/set-historyId`, data);
  }
  isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    if (!token) {
      return false;
    }

    const payload = this.decodeToken(token);
    return !this.isTokenExpired(payload);
  }

  logout() {
    localStorage.removeItem('token');
    this.isAuthenticatedSubject.next(false);
    window.location.href = '/home';
    }

  private decodeToken(token: string): any {
    try {
      const base64Payload = token.split('.')[1];
      const jsonPayload = atob(base64Payload);
      return JSON.parse(jsonPayload);
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }

  private isTokenExpired(payload: any): boolean {
    if (!payload || !payload.exp) {
      return true
    }
    const currentTime = Math.floor(Date.now() / 1000); 
    return payload.exp < currentTime;
  }



}

