import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CartService } from '../../services/cart.service';
import { OrderService } from '../../services/order.service'; 
import { CartItem } from '../../models/cart-item';
import { AuthService } from '../../services/auth.service';

interface OrderData {
    orderOptions: string;
    city: string;
    street: string;
    houseNumber: string;
    apartmentNumber: string;
    postalCode: string;
    orderComments: string;
    paymentMethod: string;
}

@Component({
    selector: 'app-orders',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './orders.component.html',
    styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
    cart: CartItem[] = [];
    totalPrice = 0;
    additionalFee = 0;
    showAddressFields = false;
    orderData: OrderData = {
        orderOptions: 'W Restauracji',
        city: '',
        street: '',
        houseNumber: '',
        apartmentNumber: '',
        postalCode: '',
        orderComments: '',
        paymentMethod: 'Karta Płatnicza'
    };

    constructor(
        private cartService: CartService,
        private http: HttpClient,
        private router: Router,
        private orderService: OrderService,
        private authService: AuthService // Wstrzyknij AuthService
    ) { }

    ngOnInit() {
        this.loadCart();
    }

    loadCart() {
        this.cartService.getCart().subscribe((data: CartItem[]) => {
            this.cart = data;
            console.log('Cart contents:', data); // Debugowanie koszyka
            this.totalPrice = this.cart.reduce((sum, item) => sum + item.cena * item.ilosc, 0);
            this.updateDeliveryOption();
        });
    }

    updateDeliveryOption() {
        this.additionalFee = 0;
        this.showAddressFields = false;
        if (this.orderData.orderOptions === 'Na Dowóz') {
            this.additionalFee = 20;
            this.showAddressFields = true;
        } else if (this.orderData.orderOptions === 'Na Wynos') {
            this.additionalFee = 5;
        }
    }

    submitOrder() {
        if (
            this.orderData.orderOptions === 'Na Dowóz' &&
            (!this.orderData.city || !this.orderData.street)
        ) {
            alert('Proszę uzupełnić adres dostawy.');
            return;
        }

        // Pobierz aktualną datę i czas w lokalnej strefie (CEST)
        const now = new Date();
        const currentDate = now.toISOString().replace('Z', '+02:00'); // np. "2025-09-02T21:28:00+02:00"

        // Pobierz userId z AuthService
        const user = this.authService.currentUser;
        if (!user) { // Sprawdzenie tylko logowania, bez ograniczeń roli
            alert('Nie jesteś zalogowany. Zaloguj się, aby złożyć zamówienie.');
            this.router.navigate(['/login']); // Przekierowanie do logowania
            return;
        }
        console.log('Current User:', user); // Debugowanie użytkownika
        if (!user.id) { // Poprawiono na user.id zamiast user.Id
            console.error('User ID is undefined in currentUser:', user);
            alert('Błąd: Brak identyfikatora użytkownika. Skontaktuj się z administratorem.');
            return;
        }
        const userId = user.id; // Pobierz id użytkownika

        // Przygotowanie payloadu zgodnego z ZamowienieDTO
        const payload = {
            opcje_zamowienia: this.orderData.orderOptions,
            metoda_platnosci: this.orderData.paymentMethod,
            laczna_cena: this.totalPrice + this.additionalFee,
            dodatkowe_informacje: this.orderData.orderComments,
            data_zamowienia: currentDate, // Dodaj aktualną datę w formacie ISO z +02:00
            uzytkownikid: userId, // Dodaj identyfikator użytkownika
            // Adres tylko dla "Na Dowóz"
            ...(this.orderData.orderOptions === 'Na Dowóz' && {
                miasto: this.orderData.city,
                ulica: this.orderData.street,
                numer_domu: this.orderData.houseNumber,
                numer_mieszkania: this.orderData.apartmentNumber,
                kod_pocztowy: this.orderData.postalCode
            }),
            // Mapowanie cart na Pozycje zgodne z PozycjaZamowieniaDTO
            pozycje: this.cart.map(item => ({
                ilosc: item.ilosc,
                cena: item.cena,
                pozycja_menu: { id: item.id } // Tylko id, bez nazwy
            }))
        };

        // Debugowanie payloadu
        console.log('Wysyłany payload:', payload);

        // Użycie OrderService
        this.orderService.createOrder(payload).subscribe({
            next: (response) => {
                console.log('Odpowiedź API:', response); // Debugowanie odpowiedzi
                alert('Zamówienie złożone pomyślnie!');
                this.cartService.clearCart(); // Wyczyść koszyk
                this.router.navigate(['/']);
            },
            error: (err) => {
                console.error('Błąd składania zamówienia:', err);
                if (err.error && err.error.errors) {
                    console.log('Szczegóły błędów walidacji:', err.error.errors);
                } else {
                    console.log('Brak szczegółowych błędów w odpowiedzi:', err.error);
                }
                alert('Błąd podczas składania zamówienia. Sprawdź konsolę.');
            }
        });
    }

    // metoda do przejścia na stronę główną
    goToHome(): void {
        // Nawigacja do strony głównej
        this.router.navigate(['/']);
    }
}