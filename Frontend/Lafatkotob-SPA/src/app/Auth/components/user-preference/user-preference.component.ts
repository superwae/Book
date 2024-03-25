import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Genre } from '../../Models/Genre';
import { UserPreference } from '../../Models/UserPreference';
import { GenreService } from '../../services/GenreService/genre.service';
import { AppUsereService } from '../../services/appUserService/app-user.service';

@Component({
  selector: 'app-user-preference',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-preference.component.html',
  styleUrl: './user-preference.component.css'
})
export class UserPreferenceComponent {

  constructor(
    private userPreferenceService: GenreService,
    private appUserService: AppUsereService
    ) { }

  genres: Genre[] = [
    { id: 1, name: 'History', selected: false },
    { id: 2, name: 'Romance', selected: false },
    { id: 3, name: 'Science Fiction', selected: false },
    { id: 4, name: 'Fantasy', selected: false },
    { id: 5, name: 'Thriller', selected: false },
    { id: 6, name: 'Young Adult', selected: false },
    { id: 7, name: 'Children', selected: false },
    { id: 8, name: 'Science', selected: false },
    { id: 9, name: 'Horror', selected: false },
    { id: 10, name: 'Nonfiction', selected: false },
    { id: 11, name: 'Health', selected: false },
    { id: 12, name: 'Travel', selected: false },
    { id: 13, name: 'Cooking', selected: false },
    { id: 14, name: 'Art', selected: false },
    { id: 15, name: 'Religion', selected: false },
    { id: 16, name: 'Philosophy', selected: false },
    { id: 17, name: 'Education', selected: false },
    { id: 18, name: 'Politics', selected: false },
    { id: 19, name: 'Business', selected: false },
    { id: 20, name: 'Technology', selected: false },
    { id: 21, name: 'True Crime', selected: false },
    { id: 22, name: 'Drama', selected: false },
    { id: 23, name: 'Adventure', selected: false },
    { id: 24, name: 'Nature', selected: false },
    { id: 25, name: 'Humor', selected: false },
    { id: 26, name: 'Lifestyle', selected: false },
    { id: 27, name: 'Economics', selected: false },
    { id: 28, name: 'Astronomy', selected: false },
    { id: 29, name: 'Linguistics', selected: false },
    { id: 30, name: 'Literature', selected: false },
    { id: 31, name: 'Short Story', selected: false },
    { id: 32, name: 'Novel', selected: false },
    { id: 33, name: 'Medicine', selected: false },
    { id: 34, name: 'Psychology', selected: false },
    { id: 35, name: 'Anime', selected: false },
    { id: 36, name: 'Poetry', selected: false },
    { id: 37, name: 'Sports', selected: false },
    { id: 38, name: 'Comics', selected: false },


  ];

  toggleGenreSelection(genre: Genre): void {
    genre.selected = !genre.selected;
  }

  get selectedGenres(): Genre[] {
    return this.genres.filter(genre => genre.selected);
  }

  confirmSelection(): void {
    const userInfo = this.appUserService.getUserInfoFromToken();
    if (!userInfo) {
      console.error('User not logged in or token expired');
      return;
    }

    const preferences = this.selectedGenres.map(genre => ({
      userId: userInfo.sub, // 'sub' usually contains the user ID, adjust according to your token's payload
      genreId: genre.id,
      preferredAuthor: '' // Adjust this as needed
    }));

    this.userPreferenceService.postUserPreferences(preferences).subscribe({
      next: () => console.log('Preferences saved successfully'),
      error: err => console.error('Failed to save preferences', err)
    });
  }
}

