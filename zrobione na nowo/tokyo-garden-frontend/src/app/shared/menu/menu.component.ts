import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CartService } from '../../services/cart.service';

interface Alergen {
    id: number;
    nazwa_Alergenu: string;
    opis_Alergenu: string;
}

interface PozycjaMenu {
    id: number;
    nazwa: string;
    opis: string;
    skladniki: string;
    cena: number;
    image_data?: string;
    alergeny: Alergen[];
    kategoria_menu?: { id: number; nazwa_kategorii: string };
}

@Component({
    selector: 'app-menu',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
    // obiekt przechowujący pozycje menu pogrupowane po kategorii
    groupedMenu: { [category: string]: PozycjaMenu[] } = {};
    // flaga do kontrolowania widoczności przycisku koszyka
    showCartButton = false;

    constructor(
        private http: HttpClient,
        private cartService: CartService,
        private router: Router // Dodanie Router do konstruktora
    ) { }

    // metoda wywoływana przy inicjalizacji komponentu
    ngOnInit(): void {
        // pobieranie danych menu z API
        this.http.get<PozycjaMenu[]>('/api/PozycjeMenu').subscribe({
            next: (data) => {
                // grupowanie pozycji menu po kategorii
                this.groupedMenu = data.reduce((acc, item) => {
                    const category = item.kategoria_menu?.nazwa_kategorii || 'Inne';
                    if (!acc[category]) acc[category] = [];
                    acc[category].push(item);
                    return acc;
                }, {} as { [category: string]: PozycjaMenu[] });
            },
            error: (err) => {
                // logowanie błędu w konsoli jeśli nie uda się pobierać danych
                console.error('Błąd ładowania menu:', err);
            }
        });
        // subskrypcja zmian w koszyku, aby aktualizować widoczność przycisku
        this.cartService.getCart().subscribe((cart) => {
            this.showCartButton = cart.length > 0;
        });
    }

    // dodawanie pozycji do koszyka
    addToCart(item: PozycjaMenu): void {
        console.log('Przekazywana pozycja:', item);

        this.cartService.addToCart({
            id: item.id,
            nazwa_pozycji: item.nazwa,
            cena: item.cena,
            ilosc: 1,
            image_data: item.image_data
        });

        console.log('Dodano do koszyka:', item);

        alert(`Dodano do koszyka: ${item.nazwa} - ${item.cena} zł. Składniki: ${item.skladniki}`);
    }


    // metoda do przejścia na stronę główną
    goToHome(): void {
        // Nawigacja do strony głównej
        this.router.navigate(['/']);
    }

    // metoda do przejścia do koszyka
    goToCart(): void {
        this.router.navigate(['/cart']);
    }
}