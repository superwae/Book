import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventModel } from '../Models/EventModels'; 

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private baseUrl = 'https://localhost:7139/api/Event';

  constructor(private http: HttpClient) { }

  getAllEvents(): Observable<EventModel[]> {
    return this.http.get<EventModel[]>(`${this.baseUrl}/getall`);
  }

  getEventById(id: number): Observable<EventModel> {
    return this.http.get<EventModel>(`${this.baseUrl}/getbyid?EventId=${id}`);
  }  

  getEventsByUserName(username: string): Observable<EventModel[]> {
    const params = new HttpParams().set('username', username);
    return this.http.get<EventModel[]>(`${this.baseUrl}/getbyusername`, { params });
  }
  
  
  postEvent(eventData: FormData): Observable<any> {
    return this.http.post(`${this.baseUrl}/post`, eventData);
  }

  updateEvent(eventId: number, eventData: FormData): Observable<any> {
    return this.http.put(`${this.baseUrl}/update/${eventId}`, eventData);
  }
  

  deleteEvent(eventId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/delete?eventId=${eventId}`);
  }

}
