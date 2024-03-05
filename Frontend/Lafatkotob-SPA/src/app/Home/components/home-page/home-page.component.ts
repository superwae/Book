import { Component, OnInit } from '@angular/core';
import { RecommendationComponent } from '../../../shared/components/recommendation/recommendation.component';
import { SearchBarComponent } from '../../../shared/components/search-bar/search-bar.component';
import { BooksComponent } from '../../../Book/components/books/books.component';
import { BookService } from '../../../Book/Service/BookService';
import { Book } from '../../../Book/Models/bookModel';
import { MagicTextComponent } from '../magic-text/magic-text.component';
import { AppUsereService } from '../../../Auth/services/app-user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [RecommendationComponent,SearchBarComponent,BooksComponent,MagicTextComponent,CommonModule],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent implements OnInit {
  searchResults: Book[] = [];
  isLoggedIn:boolean = false;

  constructor(
    private bookService: BookService,
    private AppUsereService:AppUsereService)
   {}

  ngOnInit(): void {
    this.isLoggedIn=this.AppUsereService.isLoggedIn();
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