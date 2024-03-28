import { Component, OnInit } from '@angular/core';
import { Book } from '../../Models/bookModel';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BookService } from '../../Service/BookService';
import { CommonModule } from '@angular/common';
import { AppUserModel } from '../../../Auth/Models/AppUserModel';
import { jwtDecode } from 'jwt-decode';
import { AddBookPostLike } from '../../Models/addBookPostLike';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.css'
})
export class BookDetailsComponent implements OnInit {
  
  book!: Book ;
  user: AppUserModel | null = null;
  BookPostLike:AddBookPostLike | undefined;
  BookPostLikeId?: number;
  
  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    )
    {
    
   }

   ngOnInit(): void {
    const bookId = this.route.snapshot.params['id'];
    const userId = localStorage.getItem('userId');
      this.bookService.getBookById(bookId).subscribe({
          next: (data) => {
              this.book = data; // Make sure this line successfully assigns data to this.book
              if (userId) {
                  this.BookPostLike = { bookId: +bookId, userId: userId , dateLiked: new Date()};
                  this.bookService.checkBookLike(this.BookPostLike).subscribe(isLiked => {
                      this.book.isLikedByCurrentUser = true;
                  });
              }
          },
          error: (err) => {
              console.error(err);
          }
      });
  }

  checkBookLikeStatus(): void {
    const userId = localStorage.getItem('userId');
    if (userId && this.book && this.book.id !== undefined) {
      this.bookService.checkBulkLikes(userId, [this.book.id]).subscribe(isLikedMap => {
        if (this.book) {
          this.book.isLikedByCurrentUser = !!isLikedMap[this.book.id];
        }
      });
    }
  }
  


  onLikeBook(bookId: number, event: MouseEvent): void {
    event.stopPropagation();
    const userId = localStorage.getItem('userId');
  
    if (!userId) {
      console.error('User must be logged in to like a book');
      return;
    }
  
    if (this.book.isLikedByCurrentUser) {
      const bookPostLikeData: AddBookPostLike = {
        bookId: bookId,
        userId: userId,
        dateLiked: new Date() 
      };
    
      this.bookService.removeBookLike(bookPostLikeData).subscribe({
        next: (data) => {
          console.log('Book like removed:', data);
          this.book.isLikedByCurrentUser = false;
        },
        error: (err) => {
          console.error(err);
        }
      });
    } else {
      const bookPostLikeData: AddBookPostLike = {
        bookId: bookId,
        userId: userId,
        dateLiked: new Date() 
      };
    
      this.bookService.AddBookLike(bookPostLikeData).subscribe({
        next: (data) => {
          console.log('Book liked:', data);
          this.book.isLikedByCurrentUser = true;
        },
        error: (err) => {
          console.error(err);
        }
      });
    }
    
  
    this.book.isLikedByCurrentUser = !this.book.isLikedByCurrentUser;
  }
    

  onOpenChat(userId: string, event: MouseEvent): void {
    event.stopPropagation();
    console.log('Opening chat with user ID:', userId);
  }

}
