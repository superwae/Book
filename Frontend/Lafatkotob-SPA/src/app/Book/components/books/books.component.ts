import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BookService } from '../../Service/BookService';
import { Book } from '../../Models/bookModel';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { BookComponent } from '../book/book.component';
import { MyTokenPayload } from '../../../shared/Models/MyTokenPayload';
import { jwtDecode } from 'jwt-decode';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [CommonModule, BookComponent],
  templateUrl: './books.component.html',
  styleUrl: './books.component.css'
})
export class BooksComponent implements OnInit,OnDestroy {
  @Input() books: Book[] | null = null;
  private booksSubscription: Subscription = new Subscription();
  constructor(private bookService: BookService, private route: ActivatedRoute) { }


  
  ngOnInit(): void {
    this.route.parent!.paramMap.subscribe(params => {
      const username = params.get('username');

      // Directly refreshing the book list based on the username
      if (username) {
        this.bookService.refreshBooksByUserName(username);
      } else {
        this.bookService.refreshBooks();
      }
    });

    this.bookService.books$.subscribe(books => {
      this.checkBooksLikeStatus(books);
    });
  }


  checkBooksLikeStatus(books: Book[]): void {
    const userId = localStorage.getItem('userId');
    if (userId && books.length > 0) {
      const bookIds = books.map(book => book.id);
      this.bookService.checkBulkLikes(userId, bookIds).subscribe(isLikedMap => {
        books.forEach(book => {
          book.isLikedByCurrentUser = !!isLikedMap[book.id];
        });
      });
    }
  }

  get books$(): Observable<Book[]> {
    return this.bookService.books$;
  }


  trackByBook(index: number, book: Book): number {
    return book.id; 
  }

  ngOnDestroy(): void {
    if (this.booksSubscription) {
      this.booksSubscription.unsubscribe(); 
    }
  }
}


