import { Component, OnInit } from '@angular/core';
import { CartService, CartItem } from '../../services/cart.service';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-cart',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './cart.component.html',
    styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
    // Tablica przechowująca wszystkie pozycje w koszyku
    cart: CartItem[] = [];
    // Zmienna przechowująca łączną cenę koszyka
    totalPrice = 0;
    // Flaga informująca o ładowaniu koszyka
    loading = false;
    // Przechowuje komunikat błędu przy ładowaniu koszyka
    errorMessage = '';

    // Konstruktor z wstrzyknięciem serwisu koszyka
    constructor(private cartService: CartService) { }

    // Wywoływane przy inicjalizacji komponentu
    ngOnInit(): void {
        // Ładowanie koszyka przy starcie komponentu
        this.loadCart();
    }

    // Funkcja ładuje koszyk z serwisu
    loadCart(): void {
        this.loading = true; // Ustawienie stanu ładowania
        this.cartService.getCart().subscribe({
            next: (data: CartItem[]) => {
                // Przypisanie danych koszyka, domyślna ilość 1
                this.cart = data.map(item => ({ ...item, ilosc: item.ilosc ?? 1 }));
                // Obliczenie łącznej ceny po załadowaniu
                this.calculateTotal();
                this.loading = false; // Koniec ładowania
            },
            error: () => {
                // Obsługa błędu przy ładowaniu koszyka
                this.errorMessage = 'Nie udało się załadować koszyka.';
                this.loading = false;
            }
        });
    }

    // Funkcja oblicza łączną cenę koszyka
    calculateTotal(): void {
        this.totalPrice = this.cart.reduce((sum, item) => sum + item.cena * item.ilosc, 0);
    }

    // Zwiększa ilość danej pozycji w koszyku
    increase(item: CartItem): void {
        item.ilosc++;
        // Aktualizacja ilości w serwisie koszyka
        this.cartService.updateQuantity(item.id, item.ilosc);
        this.calculateTotal(); // Aktualizacja łącznej ceny
    }

    // Zmniejsza ilość danej pozycji w koszyku
    decrease(item: CartItem): void {
        if (item.ilosc > 1) {
            item.ilosc--;
            // Aktualizacja ilości w serwisie koszyka
            this.cartService.updateQuantity(item.id, item.ilosc);
            this.calculateTotal(); // Aktualizacja łącznej ceny
        }
    }

    // Usuwa pozycję z koszyka
    remove(item: CartItem): void {
        this.cartService.removeFromCart(item.id);
        // Odświeżenie koszyka po usunięciu pozycji
        this.loadCart();
    }

    // Funkcja kończy składanie zamówienia
    finalizeOrder(): void {
        // Tutaj wyślij dane zamówienia do backendu
        alert('Tutaj wyślij zamówienie do backendu');
    }
}
