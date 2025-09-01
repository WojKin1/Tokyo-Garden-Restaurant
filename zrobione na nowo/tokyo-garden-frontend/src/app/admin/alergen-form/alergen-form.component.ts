import { Component, OnInit } from '@angular/core';
// importujemy ActivatedRoute aby pobrać parametry z URL
import { ActivatedRoute, Router } from '@angular/router';
// importujemy CommonModule dla *ngIf i innych dyrektyw strukturalnych
import { CommonModule } from '@angular/common';
// import FormsModule aby korzystać z ngModel w formularzach
import { FormsModule } from '@angular/forms';
// importujemy serwis odpowiedzialny za operacje na alergenach
import { AlergenService } from '../../services/alergen.service';
// importujemy AuthService aby sprawdzać uprawnienia użytkownika
import { AuthService } from '../../services/auth.service';

export interface AlergenDto {
    // unikalny identyfikator alergenu w bazie danych
    id: number;
    // nazwa alergenu widoczna w interfejsie użytkownika
    nazwa_alergenu: string;
    // krótki opis alergenu dla użytkowników i administratorów
    opis_alergenu: string;
    // lista powiązanych pozycji menu, można rozszerzyć typ na PozycjaMenuDto
    pozycje_menu: any[]; // można później wyodrębnić interfejs PozycjaMenuDto
}

@Component({
    selector: 'app-alergen-form',
    standalone: true,
    // importujemy moduły wymagane do działania komponentu
    imports: [CommonModule, FormsModule],
    templateUrl: './alergen-form.component.html',
    styleUrls: ['./alergen-form.component.css']
})
export class AlergenFormComponent implements OnInit {
    // inicjalizacja obiektu alergenu z pustymi polami
    alergen: AlergenDto = {
        id: 0,
        nazwa_alergenu: '',
        opis_alergenu: '',
        pozycje_menu: []
    };
    // flaga określająca czy komponent jest w trybie edycji
    isEditMode = false;

    // konstruktor z wstrzyknięciem potrzebnych serwisów i routera
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private alergenService: AlergenService,
        private authService: AuthService // wstrzykujemy AuthService do sprawdzania uprawnień
    ) { }

    ngOnInit(): void {
        // sprawdzamy czy użytkownik ma uprawnienia administratora
        if (!this.authService.isAdmin()) {
            // jeśli nie jest adminem, przekierowujemy na stronę główną
            this.router.navigate(['/']);
            return;
        }

        // pobieramy parametr id z URL jeśli istnieje, aby wczytać alergenu
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.isEditMode = true; // ustawiamy tryb edycji
            // pobieramy dane alergenu z serwisu po id
            this.alergenService.getAlergen(+id).subscribe({
                next: (data) => (this.alergen = data),
                // logujemy błędy w konsoli jeśli pobieranie się nie powiedzie
                error: (err) => console.error('Błąd pobierania alergenu', err)
            });
        }
    }

    // metoda do zapisywania alergenu po kliknięciu "Zapisz"
    saveAlergen(): void {
        // dodatkowe zabezpieczenie w przypadku braku uprawnień admina
        if (!this.authService.isAdmin()) return;

        if (this.isEditMode) {
            // jeśli jest tryb edycji, aktualizujemy istniejący alergen
            this.alergenService.updateAlergen(this.alergen).subscribe({
                // po udanej aktualizacji przekierowujemy do listy alergenów
                next: () => this.router.navigate(['/admin/alergeny']),
                // logujemy ewentualny błąd przy aktualizacji
                error: (err) => console.error('Błąd aktualizacji alergenu', err)
            });
        } else {
            // jeśli nie jest tryb edycji, tworzymy nowy alergen
            this.alergenService.createAlergen(this.alergen).subscribe({
                // po udanym dodaniu przekierowujemy do listy alergenów
                next: () => this.router.navigate(['/admin/alergeny']),
                // logujemy ewentualny błąd przy tworzeniu alergenu
                error: (err) => console.error('Błąd tworzenia alergenu', err)
            });
        }
    }

    // metoda obsługująca anulowanie operacji i powrót do listy alergennów
    cancel(): void {
        this.router.navigate(['/admin/alergeny']);
    }

    // metoda do szybkiego powrotu do panelu administratora
    goToAdmin(): void {
        this.router.navigate(['/admin']);
    }

}
