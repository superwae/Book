import { Component, OnInit } from '@angular/core';
import { BooksComponent } from '../../../Book/components/books/books.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { Book } from '../../../Book/Models/bookModel';


@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [BooksComponent,SidebarComponent],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit {
  userBooks: Book[] = [];
  
  constructor() {}

  ngOnInit(): void {
    
}
}

