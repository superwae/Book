import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { MyTokenPayload } from '../../../shared/Models/MyTokenPayload';
import { EventService } from '../../Service/event.service';
import { EventModel } from '../../Models/EventModels';

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {
  @Input() event: EventModel|null = null;
 
  constructor(
    private route: ActivatedRoute,
    private eventService: EventService 
  ) {}

  ngOnInit(): void {
    const eventId = this.route.snapshot.params['id'];
    const userId = this.getUserInfoFromToken()?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    
    if (eventId && userId) {
      this.eventService.getEventById(eventId).subscribe({
        next: (data) => {
          this.event = data;
        },
        error: (err) => {
          console.error(err);
        }
      });
    }
  }


  getUserInfoFromToken(): MyTokenPayload | undefined {
    const token = localStorage.getItem('token');
    if (token) {
      return jwtDecode<MyTokenPayload>(token);
    }
    return undefined;
  }

}
