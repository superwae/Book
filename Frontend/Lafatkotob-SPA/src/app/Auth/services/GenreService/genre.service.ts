import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserPreference } from '../../Models/UserPreference';

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  private baseUrl = 'https://localhost:7139/api';

  constructor(private http: HttpClient,private router:Router) { }

  postUserPreferences(preferences: UserPreference[]): Observable<any> {
    return this.http.post(`${this.baseUrl}/postBatch`, preferences); 
  }
}
