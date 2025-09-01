import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { PozycjaService } from '../../services/pozycja.service';
import { KategoriaService } from '../../services/kategoria.service';
import { AlergenService } from '../../services/alergen.service';
import { AuthService } from '../../services/auth.service';

// interfejs reprezentujący kategorię w systemie
export interface KategoriaDto {
    id: number;
    nazwa_kategorii: string;
}

// interfejs reprezentujący alergen w systemie
export interface AlergenDto {
    id: number;
    nazwa_alergenu: string;
}

// interfejs reprezentujący pozycję w menu restauracji
export interface PozycjaDto {
    id?: number;
    nazwa_pozycji: string;
    opis: string;
    cena: number;
    skladniki: string;
    kategoria_id: number;
    alergeny: number[];
    image_data?: string;
}

@Component({
    selector: 'app-pozycja-form',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './pozycja-form.component.html',
    styleUrls: ['./pozycja-form.component.css']
})
export class PozycjaFormComponent implements OnInit {

    // obiekt przechowujący dane aktualnej pozycji menu
    pozycja: PozycjaDto = {
        nazwa_pozycji: '',
        opis: '',
        cena: 0,
        skladniki: '',
        kategoria_id: 0,
        alergeny: []
    };

    // flaga sprawdzająca czy komponent działa w trybie edycji
    isEditMode = false;

    // tablica przechowująca wszystkie dostępne kategorie
    kategorie: KategoriaDto[] = [];

    // tablica przechowująca wszystkie dostępne alergeny
    alergeny: AlergenDto[] = [];

    // zmienna przechowująca wybrany plik ze zdjęciem
    selectedFile: File | null = null;

    constructor(
        // router do przekierowań między stronami
        private route: ActivatedRoute,
        private router: Router,
        // serwis do obsługi operacji na pozycjach
        private pozycjaService: PozycjaService,
        // serwis do pobierania kategorii
        private kategoriaService: KategoriaService,
        // serwis do pobierania alergenów
        private alergenService: AlergenService,
        // serwis autoryzacyjny do sprawdzania uprawnień
        private authService: AuthService
    ) { }

    // funkcja inicjalizująca komponent
    ngOnInit(): void {

        // blokada dostępu jeśli użytkownik nie jest administratorem
        if (!this.authService.isAdmin()) {

            // przekierowanie użytkownika na stronę główną
            this.router.navigate(['/']);
            return;
        }

        // załadowanie kategorii i alergenów potrzebnych w formularzu
        this.loadKategorie();
        this.loadAlergeny();

        // sprawdzenie czy istnieje parametr id w routingu
        const idParam = this.route.snapshot.paramMap.get('id');
        if (idParam) {
            this.isEditMode = true;
            const id = Number(idParam);

            // pobranie danych pozycji z backendu i przypisanie do formularza
            this.pozycjaService.getPozycjaById(id).subscribe({
                next: (data) => {
                    this.pozycja = {
                        ...data,
                        alergeny: data.alergeny?.map((a: any) => a.id) || []
                    };
                },

                // logowanie błędu w przypadku problemów z serwerem
                error: (err) => console.error('Błąd ładowania pozycji', err)
            });
        }
    }

    // metoda pobierająca wszystkie kategorie
    loadKategorie(): void {

        // dodatkowa ochrona przed dostępem dla nie-adminów
        if (!this.authService.isAdmin()) return;

        // wywołanie serwisu pobierającego kategorie
        this.kategoriaService.getAllKategorie().subscribe({
            next: (data) => this.kategorie = data,

            // logowanie błędu w przypadku problemów z serwerem
            error: (err) => console.error('Błąd ładowania kategorii', err)
        });
    }

    // metoda pobierająca wszystkie alergeny
    loadAlergeny(): void {

        // dodatkowa ochrona przed dostępem dla nie-adminów
        if (!this.authService.isAdmin()) return;

        // wywołanie serwisu pobierającego alergeny
        this.alergenService.getAllAlergeny().subscribe({
            next: (data) => this.alergeny = data,

            // logowanie błędu w przypadku problemów z serwerem
            error: (err) => console.error('Błąd ładowania alergenów', err)
        });
    }

    // metoda obsługująca wybór pliku zdjęcia
    onFileSelected(event: Event): void {

        // dodatkowa ochrona przed dostępem dla nie-adminów
        if (!this.authService.isAdmin()) return;

        // pobranie pliku z eventu inputa
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.selectedFile = input.files[0];
        }
    }

    // metoda zapisująca nową lub edytowaną pozycję
    savePozycja(): void {

        // blokada zapisu dla nieuprawnionych użytkowników
        if (!this.authService.isAdmin()) return;

        // utworzenie obiektu FormData do wysłania wraz z plikiem
        const formData = new FormData();
        formData.append('nazwa_pozycji', this.pozycja.nazwa_pozycji);
        formData.append('opis', this.pozycja.opis);
        formData.append('cena', this.pozycja.cena.toString());
        formData.append('skladniki', this.pozycja.skladniki);
        formData.append('kategoria_id', this.pozycja.kategoria_id.toString());
        formData.append('alergeny', JSON.stringify(this.pozycja.alergeny));
        if (this.selectedFile) {
            formData.append('zdjecie', this.selectedFile);
        }

        // zapisanie danych w trybie edycji lub nowej pozycji
        if (this.isEditMode && this.pozycja.id) {
            this.pozycjaService.updatePozycja(this.pozycja.id, formData).subscribe({
                next: () => this.router.navigate(['/admin/pozycje']),
                error: (err) => console.error('Błąd aktualizacji pozycji', err)
            });
        } else {
            this.pozycjaService.createPozycja(formData).subscribe({
                next: () => this.router.navigate(['/admin/pozycje']),
                error: (err) => console.error('Błąd tworzenia pozycji', err)
            });
        }
    }

    // metoda anulująca edycję i powracająca do listy
    cancel(): void {

        // przekierowanie do listy pozycji w panelu admina
        this.router.navigate(['/admin/pozycje']);
    }

    // metoda przekierowująca użytkownika do panelu admina
    goToAdmin(): void {

        // wykonanie przekierowania do głównego panelu administratora
        this.router.navigate(['/admin']);
    }
}
