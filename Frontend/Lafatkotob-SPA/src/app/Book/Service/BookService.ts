import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from '../Models/bookModel';
import { RegisterBook } from '../Models/RegisterbookModel';
import { AddBookPostLike } from '../Models/addBookPostLike';

@Injectable({
  providedIn: 'root'
})
export class BookService  {
  private baseUrl = 'https://localhost:7139/api/Book'; 

  constructor(private http: HttpClient) { }

  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.baseUrl+'/getall');
  }

  getBookById(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.baseUrl}/${id}`);
  }
  registerBook(formData: FormData): Observable<any> {
    return this.http.post(`${this.baseUrl}/post`, formData);
  }
  searchBooks(query: string): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.baseUrl}/search`, { params: { query } });
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