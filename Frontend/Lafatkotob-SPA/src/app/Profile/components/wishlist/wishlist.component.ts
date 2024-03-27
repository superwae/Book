import { WishListService } from './../../services/wishlist service/wish-list.service';
import { Component, OnInit } from '@angular/core';
import { NgModule } from '@angular/core';
import { MyTokenPayload } from '../../../shared/Models/MyTokenPayload';
import { jwtDecode } from 'jwt-decode';
import { BookInWishList } from '../../Models/BookInWishList';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';



@Component({
  
  selector: 'app-wishlist',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './wishlist.component.html',
  styleUrl: './wishlist.component.css'
})
export class WhisllistComponent implements OnInit{
  Books: BookInWishList[]|null=null;
  constructor( private WishListService : WishListService ) { }
  
 
ngOnInit(): void {
  
  const userId = this.getUserInfoFromToken()?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
  if (userId) {
   this.WishListService.getWishListByUserId(userId).subscribe({
     next: (data) => {
      this.Books = data;
      console.log("Books:"+this.Books);
      console.log("Books:"+data);

       
     },
     error: (err) => { console.error('Error fetching books:', err)},
   });
  } 
}
getUserInfoFromToken(): MyTokenPayload | undefined {
  const token = localStorage.getItem('token');
  if (token) {
    return jwtDecode<MyTokenPayload>(token);
  }
  return undefined;
}

}
