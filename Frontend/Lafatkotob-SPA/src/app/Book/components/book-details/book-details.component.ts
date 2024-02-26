import { Component, OnInit } from '@angular/core';
import { Book } from '../../Models/bookModel';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BookService } from '../../Service/BookService';
import { CommonModule } from '@angular/common';
import {  AppUsereService } from '../../../Auth/services/app-user.service';
import { AppUserModel } from '../../../Auth/Models/AppUserModel';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.css'
})
export class BookDetailsComponent implements OnInit {
  
  book: Book | null = null;
  user: AppUserModel | null = null;
  
  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private userService: AppUsereService
    )
    {
    
   }

   ngOnInit(): void {
    const bookId = this.route.snapshot.params['id'];
    if (bookId) {
      this.bookService.getBookById(bookId).subscribe({
        next: (bookData: Book) => {
          this.book = bookData;
        
          if (this.book && this.book.userId) {
            this.userService.getUserById(this.book.userId).subscribe({
              next: (userData: AppUserModel) => {
                this.user = userData;
              },
              error: (err) => console.error(err)
            });
          }
        },
        error: (err) => console.error(err)
      });
    }
  }

  onLikeBook(bookId: number | undefined): void {
  if (bookId === undefined) {
    console.error('Book ID is undefined');
    return;
  }
}

onOpenChat(userId: string | undefined): void {
  if (userId === undefined) {
    console.error('User ID is undefined');
    return;
  }
}

}
