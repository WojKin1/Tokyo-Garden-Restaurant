import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { KategoriaService } from '../../services/kategoria.service';
import { AuthService } from '../../services/auth.service';

export interface Kategoria {
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
    kategorie: Kategoria[] = [];

    constructor(
        private kategoriaService: KategoriaService,
        private authService: AuthService,
        private router: Router
    ) { }

    ngOnInit(): void {
        // Dostęp do listy tylko dla administratora
        if (!this.authService.isAdmin()) {
            this.router.navigate(['/']);
            return;
        }

        this.loadKategorie();
    }

    loadKategorie(): void {
        if (!this.authService.isAdmin()) return;

        this.kategoriaService.getAllKategorie().subscribe({
            next: (data) => {
                // Mapowanie danych z backendu na lokalny format
                this.kategorie = data.map(k => ({
                    id: k.id,
                    nazwa_kategorii: k.nazwaKategorii
                }));
            },
            error: (err) => console.error('Błąd ładowania kategorii', err),
        });
    }

    deleteKategoria(id: number): void {
        if (!this.authService.isAdmin()) return;

        if (confirm('Czy na pewno chcesz usunąć tę kategorię?')) {
            this.kategoriaService.deleteKategoria(id).subscribe({
                next: () => this.loadKategorie(),
                error: (err) => console.error('Błąd usuwania kategorii', err),
            });
        }
    }

    goToAdmin(): void {
        this.router.navigate(['/admin']);
    }
}
