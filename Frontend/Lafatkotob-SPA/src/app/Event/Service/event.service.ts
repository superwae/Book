import { HttpClient } from '@angular/common/http';
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
    return this.http.get<EventModel>(`${this.baseUrl}/getbyid?id=${id}`);
  }

  getEventsByUserId(userId: string): Observable<EventModel[]> {
    return this.http.get<EventModel[]>(`${this.baseUrl}/user/${userId}/events`);
  }
  
  postEvent(eventData: EventModel): Observable<any> {
    return this.http.post(`${this.baseUrl}/post`, eventData);
  }

  updateEvent(eventData: EventModel): Observable<any> {
    return this.http.put(`${this.baseUrl}/update`, eventData);
  }

  deleteEvent(eventId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/delete?eventId=${eventId}`);
  }

}
