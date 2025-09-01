import { Component, OnInit } from '@angular/core';
//potrzebne dla *ngIf i *ngFor
import { CommonModule } from '@angular/common';
//potrzebne dla [(ngModel)]
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { KategoriaService } from '../../services/kategoria.service';
//serwis sprawdzaj�cy admina
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-kategoria-form',
    standalone: true,
    //importujemy modu�y potrzebne w komponencie
    imports: [CommonModule, FormsModule],
    templateUrl: './kategoria-form.component.html',
    styleUrls: ['./kategoria-form.component.css']
})
export class KategoriaFormComponent implements OnInit {

    // obiekt przechowuj�cy dane kategorii
    kategoria: any = {};

    // flaga sprawdzaj�ca czy edytujemy czy tworzymy nowy wpis
    isEditMode = false;

    constructor(
        //  do pobierania parametr�w z URL
        private route: ActivatedRoute, 
        //  do przekierowa� w komponencie
        private router: Router,  
        //  serwis do obs�ugi kategorii
        private kategoriaService: KategoriaService,
        //  serwis do sprawdzania uprawnie� admina
        private authService: AuthService
    ) { }

    // funkcja wywo�ywana przy inicjalizacji komponentu
    ngOnInit(): void {

        // blokada dost�pu dla nie-admin�w
        if (!this.authService.isAdmin()) {
            // przekierowanie na stron� g��wn� gdy brak uprawnie�
            this.router.navigate(['/']);
            return;
        }

        // pobranie parametru id z URL
        const id = this.route.snapshot.paramMap.get('id');
        // sprawdzenie czy jeste�my w trybie edycji
        if (id) {
            this.isEditMode = true; // ustawienie trybu edycji
            // pobranie danych kategorii z serwisu
            this.kategoriaService.getKategoriaById(+id).subscribe({
                next: (data) => (this.kategoria = data),
                error: (err) => console.error('B��d pobierania kategorii', err)
            });
        }
    }

    // funkcja zapisuj�ca kategori�
    saveKategoria(): void {

        // dodatkowa blokada dla nie-admin�w
        if (!this.authService.isAdmin()) return;

        // sprawdzenie trybu edycji
        if (this.isEditMode) {
            // wywo�anie aktualizacji kategorii
            this.kategoriaService.updateKategoria(this.kategoria.id, this.kategoria).subscribe({
                next: () => this.router.navigate(['/admin/kategorie']),
                error: (err) => console.error('B��d aktualizacji kategorii', err)
            });
        } else {
            // tworzenie nowej kategorii
            this.kategoriaService.createKategoria(this.kategoria).subscribe({
                next: () => this.router.navigate(['/admin/kategorie']),
                error: (err) => console.error('B��d tworzenia kategorii', err)
            });
        }
    }

    // funkcja anuluj�ca zmiany i powracaj�ca do listy
    cancel(): void {
        //  powr�t do listy kategorii
        this.router.navigate(['/admin/kategorie']); 
    }

    // funkcja przekierowuj�ca do panelu admina
    goToAdmin(): void {
        //  powr�t do panelu administratora
        this.router.navigate(['/admin']);
    }
}
