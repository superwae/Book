import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { EventService } from '../../Service/event.service';
import { EventModel } from '../../Models/EventModels';
import { EventComponent } from '../event/event.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [CommonModule,EventComponent],
  templateUrl: './events.component.html',
  styleUrl: './events.component.css'
})
export class EventsComponent implements OnInit{
  @Input() events: EventModel[] = [];
  constructor(private eventService:EventService,private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.route.parent!.paramMap.subscribe(params => {
      const username = params.get('username');
      console.log("username: ", username);
  
      if (username) {
        this.eventService.getEventsByUserName(username).subscribe({
          next: (events: EventModel[]) => {
            this.events = events;
          },
          error: (err) => console.error('Error fetching events:', err),
        });
      } else {
        this.eventService.getAllEvents().subscribe({
          next: (events: EventModel[]) => {
            this.events = events;
          },
          error: (err) => console.error('Error fetching events:', err)
        });
      }
    });
  }
}
