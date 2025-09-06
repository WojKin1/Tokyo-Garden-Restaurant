import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { PozycjaService } from '../../services/pozycja.service';
import { KategoriaService } from '../../services/kategoria.service';
import { AuthService } from '../../services/auth.service';

export interface KategoriaDto {
    id: number;
    nazwa_kategorii: string;
}

export interface AlergenDto {
    id: number;
    nazwaAlergenu: string;
}

export interface PozycjaDto {
    [x: string]: any;
    id?: number;
    nazwa_pozycji: string;
    opis: string;
    cena: number;
    skladniki: string;
    kategoria_id: number;
    alergeny: number[];
}

@Component({
    selector: 'app-pozycja-form',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './pozycja-form.component.html',
    styleUrls: ['./pozycja-form.component.css']
})
export class PozycjaFormComponent implements OnInit {
    pozycja: PozycjaDto = {
        nazwa_pozycji: '',
        opis: '',
        cena: 0,
        skladniki: '',
        kategoria_id: 0,
        alergeny: []
    };

    isEditMode = false;
    kategorie: KategoriaDto[] = [];
    alergeny: AlergenDto[] = [];

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private pozycjaService: PozycjaService,
        private kategoriaService: KategoriaService,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        // Dostęp tylko dla administratora
        if (!this.authService.isAdmin()) {
            this.router.navigate(['/']);
            return;
        }

        // Pobranie listy kategorii przed załadowaniem danych pozycji
        this.loadKategorie().then(() => {
            const idParam = this.route.snapshot.paramMap.get('id');
            if (idParam) {
                this.isEditMode = true;
                const id = Number(idParam);

                this.pozycjaService.getPozycjaById(id).subscribe({
                    next: (data) => {
                        this.pozycja = {
                            id: data.id,
                            nazwa_pozycji: data.nazwa || '',
                            opis: data.opis || '',
                            cena: data.cena || 0,
                            skladniki: data.skladniki || '',
                            kategoria_id: data.kategoria?.id || this.kategorie[0]?.id || 0,
                            alergeny: data.alergeny?.map((a: any) => a.id) || []
                        };
                    },
                    error: (err) => console.error('Błąd ładowania pozycji', err)
                });
            } else {
                // Domyślna kategoria dla nowej pozycji
                if (this.kategorie.length > 0) {
                    this.pozycja.kategoria_id = this.kategorie[0].id;
                }
            }
        });
    }

    loadKategorie(): Promise<void> {
        return new Promise((resolve) => {
            if (!this.authService.isAdmin()) return;

            this.kategoriaService.getAllKategorie().subscribe({
                next: res => {
                    this.kategorie = res.map(k => ({
                        id: k.id,
                        nazwa_kategorii: k.nazwaKategorii || `Kategoria ${k.id}`
                    }));

                    // Ustawienie domyślnej kategorii przy dodawaniu nowej pozycji
                    if (!this.isEditMode && this.kategorie.length > 0) {
                        this.pozycja.kategoria_id = this.kategorie[0].id;
                    }

                    resolve();
                },
                error: err => console.error('Błąd pobierania kategorii', err)
            });
        });
    }

    savePozycja(form: NgForm) {
        if (form.invalid) {
            console.log('Formularz jest nieprawidłowy');
            return;
        }

        if (this.pozycja.kategoria_id === 0 && this.kategorie.length > 0) {
            this.pozycja.kategoria_id = this.kategorie[0].id;
        }

        const pozycjaPayload = {
            id: this.pozycja.id,
            nazwa_pozycji: this.pozycja.nazwa_pozycji,
            opis: this.pozycja.opis,
            skladniki: this.pozycja.skladniki || null,
            cena: this.pozycja.cena,
            kategoria_menu: { id: this.pozycja.kategoria_id },
            alergeny: this.pozycja.alergeny || []
        };

        if (this.isEditMode && this.pozycja.id != null) {
            this.pozycjaService.updatePozycja(this.pozycja.id, pozycjaPayload).subscribe({
                next: () => this.router.navigate(['/admin/pozycje']),
                error: err => console.error('Błąd edycji pozycji:', err)
            });
        } else {
            this.pozycjaService.createPozycja(pozycjaPayload).subscribe({
                next: () => this.router.navigate(['/admin/pozycje']),
                error: err => console.error('Błąd tworzenia pozycji', err)
            });
        }
    }

    cancel(): void {
        this.router.navigate(['/admin/pozycje']);
    }

    goToAdmin(): void {
        this.router.navigate(['/admin']);
    }
}
