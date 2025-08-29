import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { RouterLink } from '@angular/router';

export interface CartItem {
  id: number;
  nazwa_pozycji: string;
  cena: number;
  image_data?: string;
}

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cart: CartItem[] = [];
  totalPrice = 0;
  loading = false;
  errorMessage = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.loading = true;
    this.http.get<CartItem[]>('/api/cart').subscribe({
      next: (data) => {
        this.cart = data || [];
        this.calculateTotal();
        this.loading = false;
      },
      error: (err) => {
        console.error('Błąd ładowania koszyka', err);
        this.errorMessage = 'Nie udało się pobrać koszyka.';
        this.loading = false;
      }
    });
  }

  calculateTotal(): void {
    this.totalPrice = this.cart.reduce((sum, item) => sum + (item.cena || 0), 0);
  }

  removeFromCart(itemId: number): void {
    if (!confirm('Czy na pewno chcesz usunąć ten produkt z koszyka?')) return;

    this.http.delete(`/api/cart/${itemId}`).subscribe({
      next: () => {
        this.cart = this.cart.filter(item => item.id !== itemId);
        this.calculateTotal();
      },
      error: (err) => {
        console.error('Błąd usuwania z koszyka', err);
        alert('Nie udało się usunąć pozycji z koszyka.');
      }
    });
  }
}
