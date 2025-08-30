import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

// Interfejs reprezentujący element koszyka
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
    // Tablica przechowująca aktualne elementy koszyka
    private cart: CartItem[] = [];
    // BehaviorSubject umożliwia subskrypcję zmian koszyka
    private cartSubject = new BehaviorSubject<CartItem[]>([]);

    // Zwraca obserwowalny strumień elementów koszyka
    getCart(): Observable<CartItem[]> {
        return this.cartSubject.asObservable();
    }

    // Dodaje element do koszyka lub zwiększa ilość jeśli istnieje
    addToCart(item: CartItem): void {
        const existing = this.cart.find(i => i.id === item.id);
        if (existing) {
            // Zwiększa ilość elementu jeśli już istnieje w koszyku
            existing.ilosc += item.ilosc;
        } else {
            // Dodaje nowy element do koszyka
            this.cart.push({ ...item });
        }
        // Powiadamia wszystkich subskrybentów o zmianie
        this.cartSubject.next(this.cart);
    }

    // Usuwa element z koszyka po jego id
    removeFromCart(id: number): void {
        this.cart = this.cart.filter(item => item.id !== id);
        // Aktualizuje subskrybentów po usunięciu elementu
        this.cartSubject.next(this.cart);
    }

    // Aktualizuje ilość danego elementu w koszyku
    updateQuantity(id: number, ilosc: number): void {
        const item = this.cart.find(i => i.id === id);
        if (item) {
            // Ustawia nową ilość dla istniejącego elementu
            item.ilosc = ilosc;
            this.cartSubject.next(this.cart);
        }
    }

    // Czyści cały koszyk i powiadamia subskrybentów
    clearCart(): void {
        this.cart = [];
        this.cartSubject.next(this.cart);
    }
}
