import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Book } from '../../Models/bookModel';
import { ActivatedRoute } from '@angular/router';
import { BookService } from '../../Service/BookService';

@Component({
  selector: 'app-book',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './book.component.html',
  styleUrl: './book.component.css'
})
export class BookComponent implements OnInit{

  book: Book | null = null;
  constructor(
    private route: ActivatedRoute,
    private bookService: BookService
    )
    {
    
   }

  ngOnInit(): void {
    const bookId = this.route.snapshot.params['id'];
    if (bookId) {
      this.bookService.getBookById(bookId).subscribe({
        next: (data) => {
          this.book = data;
        },
        error: (err) => {
          console.error(err);
        }
      });
    }
  }
}