import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Book } from '../../Models/bookModel';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BookService } from '../../Service/BookService';

@Component({
  selector: 'app-book',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './book.component.html',
  styleUrl: './book.component.css'
})
export class BookComponent implements OnInit{
  @Input() book!: Book;
  
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


  onLikeBook(bookId: number, event: MouseEvent): void {
    event.stopPropagation();
    console.log('Liking book with ID:', bookId);
  }
  
  onOpenChat(userId: string, event: MouseEvent): void {
    event.stopPropagation();
    console.log('Opening chat with user ID:', userId);
  }
}