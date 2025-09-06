import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth.service';

// Komponent obsługujący logowanie użytkownika
@Component({
    selector: 'app-login',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterLink, HttpClientModule],
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    // Obiekt przechowujący dane formularza logowania
    loginData = { login: '', password: '', rememberMe: false };

    // Tablica przechowująca komunikaty błędów logowania
    errors: string[] = [];

    // Flaga informująca o trwającym procesie logowania
    loading = false;

    // Konstruktor z wstrzyknięciem HttpClient, Router i AuthService
    constructor(private http: HttpClient, private router: Router, private auth: AuthService) { }

    // Funkcja obsługująca logowanie użytkownika
    login(): void {
        // Sprawdzenie czy pola login i hasło są wypełnione
        if (!this.loginData.login || !this.loginData.password) {
            this.errors = ['Proszę wypełnić wszystkie pola.'];
            return;
        }
        // Resetowanie błędów przed wysłaniem żądania
        this.errors = [];
        // Ustawienie flagi ładowania na true
        this.loading = true;
        // Wysłanie danych logowania na backend
        this.http.post<any>('/api/Uzytkownicy/login', {
            Username: this.loginData.login, // Poprawiono na wielką literę
            Password: this.loginData.password // Poprawiono na wielką literę
        })
            .subscribe({
                // Obsługa poprawnego logowania
                next: (user) => {
                    // Wybór storage w zależności od zaznaczenia "rememberMe"
                    const storage = this.loginData.rememberMe ? localStorage : sessionStorage;
                    storage.setItem('currentUser', JSON.stringify(user));
                    // Wyłączenie flagi ładowania po zalogowaniu
                    this.loading = false;
                    // Przekierowanie użytkownika na stronę główną
                    this.router.navigate(['/']);
                },
                // Obsługa błędów przy logowaniu
                error: (err) => {
                    this.loading = false;
                    const msg = err?.status === 401
                        ? 'Nieprawidłowy login lub hasło.'
                        : 'Błąd połączenia z serwerem.';
                    this.errors = [msg];
                    console.error('Błąd logowania:', err);
                }
            });
    }
}
