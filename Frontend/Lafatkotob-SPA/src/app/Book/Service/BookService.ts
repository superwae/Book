import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from '../Models/bookModel';
import { RegisterBook } from '../Models/RegisterbookModel';

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
}
