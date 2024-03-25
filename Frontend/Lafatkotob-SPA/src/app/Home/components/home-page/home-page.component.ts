import { Component, OnInit } from '@angular/core';
import { RecommendationComponent } from '../../../shared/components/recommendation/recommendation.component';
import { SearchBarComponent } from '../../../shared/components/search-bar/search-bar.component';
import { BooksComponent } from '../../../Book/components/books/books.component';
import { BookService } from '../../../Book/Service/BookService';
import { Book } from '../../../Book/Models/bookModel';
import { MagicTextComponent } from '../magic-text/magic-text.component';
import { AppUsereService } from '../../../Auth/services/appUserService/app-user.service';
import { CommonModule } from '@angular/common';
import { FilterComponent } from '../../../shared/components/filter/filter.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [RecommendationComponent,SearchBarComponent,BooksComponent,MagicTextComponent,CommonModule,FilterComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent implements OnInit {
  searchResults: Book[] = [];
  filteredBooks: Book[] = [];
  isLoggedIn:boolean = false;

  constructor(
    private bookService: BookService,
    private AppUsereService:AppUsereService)
   {}

  ngOnInit(): void {
    this.isLoggedIn=this.AppUsereService.isLoggedIn();
  }

  handleSearch(query: string): void {
    if(query) {
      // Handle search with a query
      this.bookService.searchBooks(query).subscribe({
        next: (books) => this.searchResults = books,
        error: (err) => console.error('Error fetching search results:', err)
      });
    } else {
      // Load all books or reset to initial state if query is empty
      this.loadAllBooks();
    }
  }

  handleGenreSelect(genreId: number | null): void {
    if (genreId) {
      this.bookService.getBooksFilteredByGenres([genreId]).subscribe({
        next: (books) => {
          console.log(books);
          this.searchResults = books;
        },
        error: (err) => console.error('Error fetching filtered books:', err)
      });
    } else {
      this.loadAllBooks();
    }
  }

  private loadAllBooks(): void {
    this.bookService.getAllBooks().subscribe({
      next: (books) => this.searchResults = books,
      error: (err) => console.error('Error loading books:', err)
    });
  }
}