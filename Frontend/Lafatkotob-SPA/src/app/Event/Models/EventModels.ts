export interface EventModel {
    id: number;
    eventName: string;
    description: string;
    dateScheduled: Date | string; 
    location: string;
    hostUserId: string;
    attendances: number;
    imagePath: string;
  }
  