import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Book } from '../../Models/bookModel';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BookService } from '../../Service/BookService';
import { jwtDecode } from 'jwt-decode';
import { MyTokenPayload } from '../../../shared/Models/MyTokenPayload';
import { AddBookPostLike } from '../../Models/addBookPostLike';

@Component({
  selector: 'app-book',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './book.component.html',
  styleUrl: './book.component.css'
})
export class BookComponent implements OnInit{
  @Input() book!: Book;
  BookPostLike:AddBookPostLike | undefined;
  BookPostLikeId?: number;
  
  constructor(
    private route: ActivatedRoute,
    private bookService: BookService
    )
    {
    
   }

  ngOnInit(): void {
    const bookId = this.route.snapshot.params['id'];
    const userId = this.getUserInfoFromToken()?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];    
    if (bookId) {
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
  

    
    
  }


  onLikeBook(bookId: number, event: MouseEvent): void {
    event.stopPropagation();
    const userId = this.getUserInfoFromToken()?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
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
    
  
    // Toggle the like status
    this.book.isLikedByCurrentUser = !this.book.isLikedByCurrentUser;
  }
  
  onOpenChat(userId: string, event: MouseEvent): void {
    event.stopPropagation();
    console.log('Opening chat with user ID:', userId);
  }

  getUserInfoFromToken(): MyTokenPayload | undefined {
    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken: MyTokenPayload = jwtDecode<MyTokenPayload>(token);
      return decodedToken;
    }
    return undefined;
  }
}