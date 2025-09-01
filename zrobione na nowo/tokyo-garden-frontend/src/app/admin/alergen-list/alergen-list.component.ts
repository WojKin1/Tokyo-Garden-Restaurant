import { Component, OnInit } from '@angular/core';
// importujemy CommonModule aby użyć dyrektyw strukturalnych Angulara
import { CommonModule } from '@angular/common';
// importujemy Router i RouterLink do nawigacji między stronami
import { Router, RouterLink } from '@angular/router';
// importujemy serwis do operacji na alergenach
import { AlergenService } from '../../services/alergen.service';
// importujemy interfejs alergenu z formularza
import { AlergenDto } from '../alergen-form/alergen-form.component';
// importujemy AuthService aby sprawdzać uprawnienia użytkownika
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-alergen-list',
    standalone: true,
    // importujemy moduły wymagane do działania komponentu
    imports: [CommonModule, RouterLink],
    templateUrl: './alergen-list.component.html',
    styleUrls: ['./alergen-list.component.css'],
})
export class AlergenListComponent implements OnInit {
    // tablica przechowująca wszystkie alergeny pobrane z API
    alergeny: AlergenDto[] = [];

    // konstruktor z wstrzykniętymi serwisami i routerem
    constructor(
        private alergenService: AlergenService,
        private authService: AuthService,   // wstrzyknięty AuthService do sprawdzania rangi admina
        private router: Router              // router potrzebny do przekierowania użytkownika
    ) { }

    ngOnInit(): void {
        // zabezpieczenie dostępu - tylko admin może przeglądać listę
        if (!this.authService.isAdmin()) {
            // jeśli użytkownik nie jest adminem, przekierowujemy na stronę główną
            this.router.navigate(['/']);
            return;
        }

        // jeśli użytkownik jest adminem, ładujemy listę alergenów z serwisu
        this.loadAlergeny();
    }

    // metoda pobierająca wszystkie alergeny z API
    loadAlergeny(): void {
        this.alergenService.getAllAlergeny().subscribe({
            // jeśli dane zostaną pobrane pomyślnie, zapisujemy je w lokalnej tablicy
            next: (data) => (this.alergeny = data),
            // jeśli wystąpi błąd podczas pobierania, logujemy go do konsoli
            error: (err) => console.error('Błąd ładowania alergenów', err),
        });
    }

    // metoda usuwania alergenu po jego ID
    deleteAlergen(id: number): void {
        // dodatkowe zabezpieczenie przed dostępem nie-admina
        if (!this.authService.isAdmin()) return;

        // pytamy użytkownika o potwierdzenie usunięcia
        if (confirm('Czy na pewno chcesz usunąć ten alergen?')) {
            this.alergenService.deleteAlergen(id).subscribe({
                // po pomyślnym usunięciu odświeżamy listę alergenów
                next: () => this.loadAlergeny(),
                // jeśli wystąpi błąd, logujemy go w konsoli
                error: (err) => console.error('Błąd usuwania alergenu', err),
            });
        }
    }

    // metoda umożliwiająca szybki powrót do panelu administratora
    goToAdmin(): void {
        this.router.navigate(['/admin']);
    }
}
