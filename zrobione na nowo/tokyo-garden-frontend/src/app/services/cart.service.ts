import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface CartItem {
    id: number;
    nazwa_pozycji: string;
    cena: number;
    ilosc: number;
    image_data?: string;
}

@Injectable({
    providedIn: 'root'
})
export class CartService {
    private cart: CartItem[] = [];
    private cartSubject = new BehaviorSubject<CartItem[]>([]);

    constructor() {
        const stored = localStorage.getItem('cart');
        if (stored) {
            try {
                this.cart = JSON.parse(stored);
                this.cartSubject.next([...this.cart]); // Emitujemy kopię
                console.log('CartService inicjalizuje koszyk:', this.cart);
            } catch (e) {
                console.error('Błąd parsowania koszyka z localStorage:', e);
                this.cart = [];
                this.cartSubject.next([]);
            }
        } else {
            console.log('Brak danych koszyka w localStorage, inicjalizacja pustego koszyka');
            this.cartSubject.next([]);
        }
    }

    getCart(): Observable<CartItem[]> {
        return this.cartSubject.asObservable();
    }

    private updateCart() {
        this.cartSubject.next([...this.cart]);
        localStorage.setItem('cart', JSON.stringify(this.cart));
        console.log('CartService aktualizuje koszyk:', this.cart);
    }

    addToCart(item: CartItem): void {
        console.log('Otrzymana pozycja w CartService:', item);
        const existing = this.cart.find(i => i.id === item.id);
        if (existing) existing.ilosc += item.ilosc;
        else this.cart.push({ ...item });
        this.updateCart();
    }

    removeFromCart(id: number): void {
        this.cart = this.cart.filter(item => item.id !== id);
        this.updateCart();
    }

    updateQuantity(id: number, ilosc: number): void {
        const item = this.cart.find(i => i.id === id);
        if (item) item.ilosc = ilosc > 0 ? ilosc : 1;
        this.updateCart();
    }

    clearCart(): void {
        this.cart = [];
        this.updateCart();
    }
}