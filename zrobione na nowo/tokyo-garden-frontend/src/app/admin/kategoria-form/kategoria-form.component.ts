import { Component, OnInit } from '@angular/core';
//potrzebne dla *ngIf i *ngFor
import { CommonModule } from '@angular/common';
//potrzebne dla [(ngModel)]
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { KategoriaService } from '../../services/kategoria.service';
//serwis sprawdzaj¹cy admina
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-kategoria-form',
    standalone: true,
    //importujemy modu³y potrzebne w komponencie
    imports: [CommonModule, FormsModule],
    templateUrl: './kategoria-form.component.html',
    styleUrls: ['./kategoria-form.component.css']
})
export class KategoriaFormComponent implements OnInit {

    // obiekt przechowuj¹cy dane kategorii
    kategoria: any = {};

    // flaga sprawdzaj¹ca czy edytujemy czy tworzymy nowy wpis
    isEditMode = false;

    constructor(
        //  do pobierania parametrów z URL
        private route: ActivatedRoute, 
        //  do przekierowañ w komponencie
        private router: Router,  
        //  serwis do obs³ugi kategorii
        private kategoriaService: KategoriaService,
        //  serwis do sprawdzania uprawnieñ admina
        private authService: AuthService
    ) { }

    // funkcja wywo³ywana przy inicjalizacji komponentu
    ngOnInit(): void {

        // blokada dostêpu dla nie-adminów
        if (!this.authService.isAdmin()) {
            // przekierowanie na stronê g³ówn¹ gdy brak uprawnieñ
            this.router.navigate(['/']);
            return;
        }

        // pobranie parametru id z URL
        const id = this.route.snapshot.paramMap.get('id');
        // sprawdzenie czy jesteœmy w trybie edycji
        if (id) {
            this.isEditMode = true; // ustawienie trybu edycji
            // pobranie danych kategorii z serwisu
            this.kategoriaService.getKategoriaById(+id).subscribe({
                next: (data) => (this.kategoria = data),
                error: (err) => console.error('B³¹d pobierania kategorii', err)
            });
        }
    }

    // funkcja zapisuj¹ca kategoriê
    saveKategoria(): void {

        // dodatkowa blokada dla nie-adminów
        if (!this.authService.isAdmin()) return;

        // sprawdzenie trybu edycji
        if (this.isEditMode) {
            // wywo³anie aktualizacji kategorii
            this.kategoriaService.updateKategoria(this.kategoria.id, this.kategoria).subscribe({
                next: () => this.router.navigate(['/admin/kategorie']),
                error: (err) => console.error('B³¹d aktualizacji kategorii', err)
            });
        } else {
            // tworzenie nowej kategorii
            this.kategoriaService.createKategoria(this.kategoria).subscribe({
                next: () => this.router.navigate(['/admin/kategorie']),
                error: (err) => console.error('B³¹d tworzenia kategorii', err)
            });
        }
    }

    // funkcja anuluj¹ca zmiany i powracaj¹ca do listy
    cancel(): void {
        //  powrót do listy kategorii
        this.router.navigate(['/admin/kategorie']); 
    }

    // funkcja przekierowuj¹ca do panelu admina
    goToAdmin(): void {
        //  powrót do panelu administratora
        this.router.navigate(['/admin']);
    }
}
