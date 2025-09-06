import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AlergenService } from '../../services/alergen.service';
import { AuthService } from '../../services/auth.service';

interface Alergen {
    id: number;
    nazwa_alergenu: string;
}

@Component({
    selector: 'app-alergen-list',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './alergen-list.component.html',
    styleUrls: ['./alergen-list.component.css'],
})
export class AlergenListComponent implements OnInit {
    alergeny: Alergen[] = [];

    constructor(
        private alergenService: AlergenService,
        private authService: AuthService,
        private router: Router
    ) { }

    ngOnInit(): void {
        // Tylko administrator ma dostęp do listy alergenów
        if (!this.authService.isAdmin()) {
            this.router.navigate(['/']);
            return;
        }

        this.loadAlergeny();
    }

    loadAlergeny(): void {
        this.alergenService.getAllAlergeny().subscribe({
            next: (data) => {
                // Mapowanie danych z backendu na lokalny format
                this.alergeny = data.map(a => ({
                    id: a.id,
                    nazwa_alergenu: a.nazwaAlergenu
                }));
            },
            error: (err) => console.error('Błąd ładowania alergenów', err),
        });
    }

    deleteAlergen(id: number): void {
        // Dodatkowe zabezpieczenie przed nieautoryzowanym usunięciem
        if (!this.authService.isAdmin()) return;

        if (confirm('Czy na pewno chcesz usunąć ten alergen?')) {
            this.alergenService.deleteAlergen(id).subscribe({
                next: () => this.loadAlergeny(),
                error: (err) => console.error('Błąd usuwania alergenu', err),
            });
        }
    }

    goToAdmin(): void {
        this.router.navigate(['/admin']);
    }
}
