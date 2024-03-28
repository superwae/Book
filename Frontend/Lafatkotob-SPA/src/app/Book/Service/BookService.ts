import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map, tap } from 'rxjs';
import { Book } from '../Models/bookModel';
import { AddBookPostLike } from '../Models/addBookPostLike';

@Injectable({
  providedIn: 'root'
})
export class BookService  {
  private booksSubject = new BehaviorSubject<Book[]>([]);
  books$ = this.booksSubject.asObservable();

  private baseUrl = 'https://localhost:7139/api/Book'; 

  constructor(private http: HttpClient) { }

  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.baseUrl+'/getall');
  }

  getBookById(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.baseUrl}/${id}`);
  }

  getBooksByUserName(username: string): Observable<Book[]> {
    const params = new HttpParams().set('username', username);
    return this.http.get<Book[]>(`${this.baseUrl}/GetBooksByUserName`, { params });
  }
  
  registerBook(formData: FormData): Observable<any> {
    return this.http.post<Book>(`${this.baseUrl}/post`, formData).pipe(
      tap(book => {
        const currentBooks = this.booksSubject.getValue();
        this.booksSubject.next([...currentBooks, book]);
      })
    );
  }


  registerBookWithGenres(formData: FormData): Observable<any> {
    return this.http.post<Book>(`${this.baseUrl}/PostBookWithGenres`, formData).pipe(
      tap(book => {
        const currentBooks = this.booksSubject.getValue();
        this.booksSubject.next([...currentBooks, book]);
        
      })
    );
  }
  

  searchBooks(query: string): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.baseUrl}/search`, { params: { query } }).pipe(
      tap(books => {
        this.booksSubject.next(books);
      })
    );
  }

  getBooksFilteredByGenres(genreIds: number[]): Observable<Book[]> {
    return this.http.get<{data: Book[]}>(this.baseUrl + '/filter', { params: { genreIds } })
      .pipe(
        map(response => response.data),
        tap(books => {
          this.booksSubject.next(books);
        })
      );
  }

  refreshBooks(): void {
    this.getAllBooks().subscribe(books => {
      this.booksSubject.next(books);
    });
  }
  refreshBooksByUserName(username: string): void {
    this.getBooksByUserName(username).subscribe(books => {
      this.booksSubject.next(books);
    });
  }
  


checkBookLike(data: AddBookPostLike): Observable<any> {
  const params = new HttpParams()
    .set('userId', data.userId)
    .set('bookId', data.bookId.toString());
  return this.http.get<any>(`https://localhost:7139/api/BookPostLike/getbyid`, { params });
}
AddBookLike(data: AddBookPostLike): Observable<any> {
  return this.http.post(`https://localhost:7139/api/BookPostLike/post`, data);

}
removeBookLike(data: AddBookPostLike): Observable<any> {
  const params = new HttpParams()
    .set('userId', data.userId)
    .set('bookId', data.bookId.toString());
  return this.http.delete<any>(`https://localhost:7139/api/BookPostLike/delete`, { params });
}
checkBulkLikes(userId: string, bookIds: number[]): Observable<{[key: number]: boolean}> {
  let params = new HttpParams().set('userId', userId);
  bookIds.forEach((id) => {
    params = params.append('bookIds', id.toString());
  });
  
  return this.http.get<{[key: number]: boolean}>(`https://localhost:7139/api/BookPostLike/checkBulkLikes`, { params });
}

}