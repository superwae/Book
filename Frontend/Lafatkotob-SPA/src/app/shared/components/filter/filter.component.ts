import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-filter',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.css'
})
export class FilterComponent {
  genres: { id: number, name: string }[] = [
    { id: 1, name: 'History' },
    { id: 2, name: 'Law' },
    { id: 3, name: 'Romance' },
    { id: 4, name: 'Science Fiction' },
    { id: 5, name: 'Fantasy' },
    { id: 6, name: 'Biography' },
    { id: 7, name: 'Mystery' },
    { id: 8, name: 'Thriller' },
    { id: 9, name: 'Young Adult' },
    { id: 10, name: 'Children' },
    { id: 11, name: 'Science' },
    { id: 12, name: 'Horror' },
    { id: 13, name: 'Nonfiction' },
    { id: 14, name: 'Self Help' },
    { id: 15, name: 'Health' },
    { id: 16, name: 'Travel' },
    { id: 17, name: 'Cooking' },
    { id: 18, name: 'Art' },
    { id: 19, name: 'Comics' },
    { id: 20, name: 'Religion' },
    { id: 21, name: 'Philosophy' },
    { id: 22, name: 'Education' },
    { id: 23, name: 'Politics' },
    { id: 24, name: 'Business' },
    { id: 25, name: 'Technology' },
    { id: 26, name: 'Sports' },
    { id: 27, name: 'Music' },
    { id: 28, name: 'True Crime' },
    { id: 29, name: 'Poetry' },
    { id: 30, name: 'Drama' },
    { id: 31, name: 'Classics' },
    { id: 32, name: 'Adventure' },
    { id: 33, name: 'Nature' },
    { id: 34, name: 'Humor' },
    { id: 35, name: 'Lifestyle' },
    { id: 36, name: 'Crafts' },
    { id: 37, name: 'Espionage' },
    { id: 38, name: 'Westerns' },
    { id: 39, name: 'Military' },
    { id: 40, name: 'Economics' },
    { id: 41, name: 'Anthropology' },
    { id: 42, name: 'Archaeology' },
    { id: 43, name: 'Astronomy' },
    { id: 44, name: 'Media Studies' },
    { id: 45, name: 'Linguistics' },
    { id: 46, name: 'English' },
    { id: 47, name: 'Spanish' },
    { id: 48, name: 'German' },
    { id: 49, name: 'Japanese' },
    { id: 50, name: 'Chinese' },
    { id: 51, name: 'Hebrew' },
    { id: 52, name: 'Arabic' },
    { id: 53, name: 'French' },
    { id: 54, name: 'Italian' },
    { id: 55, name: 'Russian' },
    { id: 56, name: 'Adult' },
    { id: 57, name: 'Style' },
    { id: 58, name: 'Literature' },
    { id: 59, name: 'Contemporary' },
    { id: 60, name: 'Short Story' },
    { id: 61, name: 'Novel' },
    { id: 62, name: 'Marriage' },
    { id: 63, name: 'Sex' },
    { id: 64, name: 'Bible' },
    { id: 65, name: 'Islam' },
    { id: 66, name: 'Judaism' },
    { id: 67, name: 'Family' },
    { id: 68, name: 'Programming' },
    { id: 69, name: 'Computer' },
    { id: 70, name: 'Mathematics' },
    { id: 71, name: 'Physics' },
    { id: 72, name: 'Chemistry' },
    { id: 73, name: 'Biology' },
    { id: 74, name: 'Engineering' },
    { id: 75, name: 'Medicine' },
    { id: 76, name: 'Psychology' },
    { id: 77, name: 'Anime' },
    { id: 78, name: 'Manga' },
    { id: 79, name: 'Superheroes' }
  ];
  
  selectedGenreId: number | null = null;
  @Output() genreSelect = new EventEmitter<number | null>();

  selectGenre(genreId: number): void {
    this.selectedGenreId = this.selectedGenreId === genreId ? null : genreId;
    this.genreSelect.emit(this.selectedGenreId);
  }

  isActive(genreId: number): boolean {
    return this.selectedGenreId === genreId;
  }
}
