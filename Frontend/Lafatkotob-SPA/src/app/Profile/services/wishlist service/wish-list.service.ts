import { WhisllistComponent } from './../../components/wishlist/wishlist.component';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { BookInWishList } from '../../Models/BookInWishList';


@Injectable({
  providedIn: 'root'
})


export class WishListService {

  private baseUrl = 'https://localhost:7139/api/WishList'; 

  constructor(private http: HttpClient) { }

  getWishListByUserId(userId: string): Observable<BookInWishList[]> {
    return this.http.get<BookInWishList[]>(`${this.baseUrl}/getbyidUser`, { params: new HttpParams().set('userId', userId) });
  }


  

  
  
}

