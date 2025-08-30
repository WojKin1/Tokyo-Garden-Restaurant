import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { CartService } from '../../services/cart.service';

interface Alergen {
    id: number;
    nazwa_Alergenu: string;
    opis_Alergenu: string;
}

interface PozycjaMenu {
    id: number;
    nazwa_pozycji: string;
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

    // konstruktor z HttpClient i CartService
    constructor(private http: HttpClient, private cartService: CartService) { }

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
                // logowanie błędu w konsoli jeśli nie uda się pobrać danych
                console.error('Błąd ładowania menu:', err);
            }
        });
    }

    // dodawanie pozycji do koszyka
    addToCart(item: PozycjaMenu): void {
        // wywołanie serwisu koszyka i dodanie elementu
        this.cartService.addToCart({
            id: item.id,
            nazwa_pozycji: item.nazwa_pozycji,
            cena: item.cena,
            ilosc: 1,
            image_data: item.image_data
        });
        // logowanie informacji o dodaniu do koszyka
        console.log('Dodano do koszyka:', item);
    }
}
