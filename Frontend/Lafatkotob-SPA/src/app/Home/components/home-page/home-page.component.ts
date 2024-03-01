import { Component, OnInit } from '@angular/core';
import { RecommendationComponent } from '../../../shared/components/recommendation/recommendation.component';
import { SearchBarComponent } from '../../../shared/components/search-bar/search-bar.component';
import { BooksComponent } from '../../../Book/components/books/books.component';
import { BookService } from '../../../Book/Service/BookService';
import { Book } from '../../../Book/Models/bookModel';
import { MagicTextComponent } from '../magic-text/magic-text.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [RecommendationComponent,SearchBarComponent,BooksComponent,MagicTextComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent implements OnInit {
  searchResults: Book[] = [];

  constructor(private bookService: BookService) {}

  ngOnInit(): void {
  }

  handleSearch(query: string): void {
    this.bookService.searchBooks(query).subscribe({
      next: (books) => {
        this.searchResults = books;
      },
      error: (err) => console.error('Error fetching search results:', err)
    });
  }
}