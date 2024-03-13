import { Component, Input, OnInit, input } from '@angular/core';
import { BookService } from '../../Service/BookService';
import { Book } from '../../Models/bookModel';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BookComponent } from '../book/book.component';
import { MyTokenPayload } from '../../../shared/Models/MyTokenPayload';
import { jwtDecode } from 'jwt-decode';
import { timeout } from 'rxjs';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [CommonModule,BookComponent],
  templateUrl: './books.component.html',
  styleUrl: './books.component.css'
})
export class BooksComponent implements OnInit{
  @Input() books: Book[] | null = null;

  constructor(private bookService: BookService ,private route:ActivatedRoute) {}


  ngOnInit(): void {

    const username = this.route.snapshot.paramMap.get('username');
    if (username) {
      this.bookService.getBooksByUserName(username).subscribe({
        next: (books: Book[]) => {
          this.books = books;
        },
        error: (err) => console.error('Error fetching books:', err),
      });
    } else {
      // Fallback if not on a user's profile page, e.g., load all books
      this.bookService.getAllBooks().subscribe({
        next: (books: Book[]) => {
          console.log('Fetched all books:', books);
          this.books = books;
          this.checkBooksLikeStatus();
        },
        error: (err) => console.error('Error fetching books:', err)
      });    }
  }

    
  
  checkBooksLikeStatus(): void {
    const userId = this.getUserInfoFromToken()?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    if (userId && this.books!.length > 0) {
      const bookIds = this.books!.map(book => book.id);
      this.bookService.checkBulkLikes(userId, bookIds).subscribe(isLikedMap => {
        this.books!.forEach(book => {
          book.isLikedByCurrentUser = !!isLikedMap[book.id];
        });
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


