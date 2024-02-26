import { Component, OnInit } from '@angular/core';
import { BookService } from '../../Service/BookService';
import { Book } from '../../Models/bookModel';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BookComponent } from '../book/book.component';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [CommonModule,BookComponent],
  templateUrl: './books.component.html',
  styleUrl: './books.component.css'
})
export class BooksComponent implements OnInit{
  books: Book[] = [];

  constructor(private bookService: BookService) {}

  ngOnInit(): void {
    this.bookService.getAllBooks().subscribe({
      next: (data: Book[]) => {
        this.books = data;
      },
      error: (err) => {
        console.error('Error fetching books:', err);
      }
    });
  }
}


