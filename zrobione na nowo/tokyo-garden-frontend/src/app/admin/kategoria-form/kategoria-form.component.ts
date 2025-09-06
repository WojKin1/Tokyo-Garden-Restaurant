import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { KategoriaService } from '../../services/kategoria.service';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-kategoria-form',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './kategoria-form.component.html',
    styleUrls: ['./kategoria-form.component.css']
})
export class KategoriaFormComponent implements OnInit {
    kategoria: any = {};
    isEditMode = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private kategoriaService: KategoriaService,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        // Dostêp do formularza tylko dla administratora
        if (!this.authService.isAdmin()) {
            this.router.navigate(['/']);
            return;
        }

        // Jeœli w URL znajduje siê ID, przechodzimy w tryb edycji
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.isEditMode = true;

            // Pobieramy dane kategorii z serwisu
            this.kategoriaService.getKategoriaById(+id).subscribe({
                next: (data) => (this.kategoria = data),
                error: (err) => console.error('B³¹d pobierania kategorii', err)
            });
        }
    }

    saveKategoria(): void {
        if (!this.authService.isAdmin()) return;

        if (this.isEditMode) {
            // Aktualizacja istniej¹cej kategorii
            this.kategoriaService.updateKategoria(this.kategoria.id, this.kategoria).subscribe({
                next: () => this.router.navigate(['/admin/kategorie']),
                error: (err) => console.error('B³¹d aktualizacji kategorii', err)
            });
        } else {
            // Dodanie nowej kategorii
            this.kategoriaService.createKategoria(this.kategoria).subscribe({
                next: () => this.router.navigate(['/admin/kategorie']),
                error: (err) => console.error('B³¹d tworzenia kategorii', err)
            });
        }
    }

    cancel(): void {
        this.router.navigate(['/admin/kategorie']);
    }

    goToAdmin(): void {
        this.router.navigate(['/admin']);
    }
}
