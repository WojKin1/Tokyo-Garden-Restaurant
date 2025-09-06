import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AlergenService } from '../../services/alergen.service';
import { AuthService } from '../../services/auth.service';

export interface AlergenDto {
    id: number;
    nazwa_alergenu: string;
    opis_alergenu: string;
    pozycje_menu: any[];
}

@Component({
    selector: 'app-alergen-form',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './alergen-form.component.html',
    styleUrls: ['./alergen-form.component.css']
})
export class AlergenFormComponent implements OnInit {
    // Model danych używany w formularzu
    alergen: AlergenDto = {
        id: 0,
        nazwa_alergenu: '',
        opis_alergenu: '',
        pozycje_menu: []
    };

    // Określa czy komponent działa w trybie edycji
    isEditMode = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private alergenService: AlergenService,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        // Dostęp tylko dla administratora
        if (!this.authService.isAdmin()) {
            this.router.navigate(['/']);
            return;
        }

        // Jeśli w URL jest ID, przechodzimy w tryb edycji
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.isEditMode = true;

            // Pobieramy dane alergenu z serwisu
            this.alergenService.getAlergen(+id).subscribe({
                next: (data) => (this.alergen = data),
                error: (err) => console.error('Błąd pobierania alergenu', err)
            });
        }
    }

    saveAlergen(): void {
        // Dodatkowe zabezpieczenie przed nieautoryzowanym zapisem
        if (!this.authService.isAdmin()) return;

        if (this.isEditMode) {
            // Aktualizacja istniejącego alergenu
            this.alergenService.updateAlergen(this.alergen).subscribe({
                next: () => this.router.navigate(['/admin/alergeny']),
                error: (err) => console.error('Błąd aktualizacji alergenu', err)
            });
        } else {
            // Dodanie nowego alergenu
            this.alergenService.createAlergen(this.alergen).subscribe({
                next: () => this.router.navigate(['/admin/alergeny']),
                error: (err) => console.error('Błąd tworzenia alergenu', err)
            });
        }
    }

    // Anulowanie operacji i powrót do listy
    cancel(): void {
        this.router.navigate(['/admin/alergeny']);
    }

    // Przejście do panelu administratora
    goToAdmin(): void {
        this.router.navigate(['/admin']);
    }
}
