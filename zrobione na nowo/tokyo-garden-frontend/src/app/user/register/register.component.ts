import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';

// Interfejs przechowujący dane rejestracji użytkownika
interface RegisterData {
    nazwaUzytkownika: string;
    haslo1: string;
    haslo2: string;
    phone: string;
    regulamin: boolean;
}

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent {
    // Obiekt przechowujący dane formularza rejestracji
    registerData: RegisterData = {
        nazwaUzytkownika: '',
        haslo1: '',
        haslo2: '',
        phone: '',
        regulamin: false
    };

    // Tablica przechowująca komunikaty błędów walidacji
    errors: string[] = [];

    // Konstruktor z wstrzyknięciem HttpClient i Routera
    constructor(private http: HttpClient, private router: Router) { }

    // Funkcja obsługująca rejestrację użytkownika
    register() {
        // Resetowanie błędów przy każdej próbie rejestracji
        this.errors = [];

        // Sprawdzenie czy hasła są identyczne
        if (this.registerData.haslo1 !== this.registerData.haslo2) {
            this.errors.push('Hasła muszą być takie same.');
            return;
        }

        // Sprawdzenie akceptacji regulaminu przez użytkownika
        if (!this.registerData.regulamin) {
            this.errors.push('Musisz zaakceptować regulamin.');
            return;
        }

        // Walidacja numeru telefonu – musi mieć 9 cyfr
        if (!/^\d{9}$/.test(this.registerData.phone)) {
            this.errors.push('Numer telefonu musi mieć 9 cyfr.');
            return;
        }

        // Wysłanie danych rejestracyjnych na backend
        this.http.post<any>('/api/Uzytkownicy', {
            nazwa_uzytkownika: this.registerData.nazwaUzytkownika,
            haslo: this.registerData.haslo1,
            telefon: this.registerData.phone,
            typ_uzytkownika: "Uzytkownik"
        })
            .subscribe({
                // Obsługa poprawnej rejestracji
                next: () => {
                    this.router.navigate(['/login']); // po rejestracji od razu na logowanie
                },
                // Obsługa błędów przy rejestracji
                error: (err) => {
                    console.error('Register error', err);
                    if (err.status === 400) {
                        this.errors.push(err.error);
                    } else {
                        this.errors.push('Błąd podczas rejestracji.');
                    }
                }
            });
    }
}
