import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { KategoriaService } from '../../services/kategoria.service';
import { AuthService } from '../../services/auth.service';

// interfejs reprezentujący kategorię w systemie
export interface KategoriaDto {
    id: number;
    nazwa_kategorii: string;
}

@Component({
    selector: 'app-kategoria-list',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './kategoria-list.component.html',
    styleUrls: ['./kategoria-list.component.css'],
})
export class KategoriaListComponent implements OnInit {

    // tablica przechowująca wszystkie kategorie pobrane z serwisu
    kategorie: KategoriaDto[] = [];

    constructor(
        // serwis odpowiedzialny za operacje na kategoriach
        private kategoriaService: KategoriaService,

        // serwis obsługujący sprawdzanie uprawnień użytkownika
        private authService: AuthService,

        // router pozwalający na przekierowania między stronami
        private router: Router
    ) { }

    // funkcja wywoływana przy inicjalizacji komponentu
    ngOnInit(): void {

        // blokada dostępu jeśli użytkownik nie jest administratorem
        if (!this.authService.isAdmin()) {

            // przekierowanie na stronę główną w przypadku braku uprawnień
            this.router.navigate(['/']);
            return;
        }

        // wywołanie metody ładującej wszystkie dostępne kategorie
        this.loadKategorie();
    }

    // metoda pobierająca wszystkie kategorie z backendu
    loadKategorie(): void {

        // dodatkowa ochrona przed dostępem dla nie-adminów
        if (!this.authService.isAdmin()) return;

        // subskrypcja danych z serwisu i przypisanie ich do tablicy
        this.kategoriaService.getAllKategorie().subscribe({
            next: (data) => (this.kategorie = data),

            // logowanie błędów w przypadku problemów z serwerem
            error: (err) => console.error('Błąd ładowania kategorii', err),
        });
    }

    // metoda umożliwiająca usunięcie wybranej kategorii
    deleteKategoria(id: number): void {

        // ochrona przed usuwaniem dla użytkowników bez uprawnień
        if (!this.authService.isAdmin()) return;

        // potwierdzenie usunięcia kategorii przez użytkownika
        if (confirm('Czy na pewno chcesz usunąć tę kategorię?')) {

            // wywołanie serwisu usuwającego kategorię w backendzie
            this.kategoriaService.deleteKategoria(id).subscribe({

                // odświeżenie listy kategorii po pomyślnym usunięciu
                next: () => this.loadKategorie(),

                // logowanie błędu w przypadku problemów podczas usuwania
                error: (err) => console.error('Błąd usuwania kategorii', err),
            });
        }
    }

    // metoda umożliwiająca powrót do panelu administratora
    goToAdmin(): void {

        // przekierowanie użytkownika do głównego panelu admina
        this.router.navigate(['/admin']);
    }
}
