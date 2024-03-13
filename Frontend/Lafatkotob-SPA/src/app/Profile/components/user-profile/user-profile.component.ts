import { Component } from '@angular/core';
import { BooksComponent } from '../../../Book/components/books/books.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [BooksComponent,SidebarComponent],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {

}
