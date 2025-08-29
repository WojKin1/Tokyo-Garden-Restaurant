import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = '/api/cart'; // zmień na swój backend

  constructor(private http: HttpClient) {}

  getCart(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  removeFromCart(index: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${index}`);
  }

  addToCart(item: any): Observable<void> {
    return this.http.post<void>(this.apiUrl, item);
  }
}
