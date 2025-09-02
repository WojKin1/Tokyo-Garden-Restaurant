import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { PozycjaService } from '../../services/pozycja.service';
import { AuthService } from '../../services/auth.service';

export interface AlergenDto {
    id: number;
    nazwa_alergenu: string;
}

export interface KategoriaDto {
    id: number;
    nazwa_kategorii: string;
}

export interface PozycjaDto {
    id: number;
    nazwa_pozycji: string;
    opis: string;
    cena: number;
    alergeny?: AlergenDto[];
    kategoria_menu?: KategoriaDto;
}

@Component({
    selector: 'app-pozycja-list',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './pozycja-list.component.html',
    styleUrls: ['./pozycja-list.component.css']
})
export class PozycjaListComponent implements OnInit {
    pozycje: PozycjaDto[] = [];
    loading = false;
    errorMessage = '';

    constructor(
        private pozycjaService: PozycjaService,
        private authService: AuthService,
        private router: Router
    ) { }

    ngOnInit(): void {
        if (!this.authService.isAdmin()) {
            this.router.navigate(['/']);
            return;
        }
        this.loadPozycje();
    }

    loadPozycje(): void {
        if (!this.authService.isAdmin()) return;

        this.loading = true;
        this.errorMessage = '';

        this.pozycjaService.getAllPozycje().subscribe({
            next: (data) => {
                this.pozycje = (data || []).map(p => ({
                    id: p.id,
                    nazwa_pozycji: p.nazwa, // mapowanie z backendu
                    opis: p.opis,
                    cena: p.cena,
                    alergeny: p.alergeny?.map((a: any) => ({
                        id: a.id,
                        nazwa_alergenu: a.nazwaAlergenu
                    })),
                    kategoria_menu: p.kategoria ? {
                        id: p.kategoria.id,
                        nazwa_kategorii: p.kategoria.nazwaKategorii
                    } : undefined
                }));
                this.loading = false;
            },
            error: (err) => {
                console.error('Błąd ładowania pozycji:', err);
                this.errorMessage = 'Nie udało się załadować pozycji.';
                this.loading = false;
            }
        });
    }

    deletePozycja(id: number): void {
        if (!this.authService.isAdmin()) return;

        if (!confirm('Czy na pewno chcesz usunąć tę pozycję?')) return;

        this.pozycjaService.deletePozycja(id).subscribe({
            next: () => {
                this.pozycje = this.pozycje.filter(p => p.id !== id);
            },
            error: (err) => {
                console.error('Błąd usuwania pozycji:', err);
                alert('Wystąpił błąd podczas usuwania pozycji.');
            }
        });
    }

    goToAdmin(): void {
        this.router.navigate(['/admin']);
    }
}
