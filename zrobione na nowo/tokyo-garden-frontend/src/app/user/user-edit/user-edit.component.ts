import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService, UpdatePayload } from 'src/app/services/user.service';

@Component({
    selector: 'app-user-edit',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './user-edit.component.html',
    styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
    editForm!: FormGroup;
    errors: string[] = [];

    constructor(
        private fb: FormBuilder,
        private router: Router,
        private userService: UserService
    ) { }

    ngOnInit(): void {
        // Inicjalizacja formularza z walidatorami
        this.editForm = this.fb.group({
            id: [0, Validators.required],
            nazwaUzytkownika: ['', [Validators.required, Validators.minLength(3)]],
            password: [''],
            password2: [''],
            telefon: ['', [Validators.required, Validators.pattern(/^\d{9}$/)]]
        });

        // Pobranie danych zalogowanego użytkownika z serwisu
        const stored = this.userService.getStoredUser();
        if (stored) {
            // Wypełnienie formularza danymi użytkownika
            this.editForm.patchValue({
                id: stored.id,
                nazwaUzytkownika: stored.nazwaUzytkownika,
                telefon: stored.telefon ?? ''
            });
            console.log('Pobrano dane użytkownika do formularza:', stored);
        } else {
            // Obsługa braku zalogowanego użytkownika
            this.errors = ['Brak zalogowanego użytkownika.'];
            console.error('Brak zalogowanego użytkownika.');
        }
    }

    onSubmit(): void {
        console.log('Wywołano onSubmit, dane formularza:', this.editForm.value);
        this.errors = [];

        // Walidacja formularza
        if (this.editForm.invalid) {
            console.warn('Formularz jest nieprawidłowy:', this.editForm.errors);
            return;
        }

        const { password, password2 } = this.editForm.value;

        // Sprawdzenie zgodności haseł, jeśli podano
        if ((password || password2) && password !== password2) {
            this.errors.push('Hasła muszą być takie same.');
            console.error('Hasła się nie zgadzają.');
            return;
        }

        // Przygotowanie danych do aktualizacji
        const updatePayload: UpdatePayload = {
            id: this.editForm.value.id,
            nazwaUzytkownika: this.editForm.value.nazwaUzytkownika,
            telefon: this.editForm.value.telefon,
            password: this.editForm.value.password
        };
        console.log('Wysyłany payload:', updatePayload);

        // Wysłanie żądania aktualizacji użytkownika
        this.userService.updateUser(updatePayload).subscribe({
            next: (response) => {
                console.log('Sukces aktualizacji:', response);

                // Aktualizacja danych w lokalnym magazynie
                const u = this.userService.getStoredUser();
                if (u) {
                    u.nazwaUzytkownika = this.editForm.value.nazwaUzytkownika;
                    u.telefon = this.editForm.value.telefon;
                    this.userService.saveToStorage(u);
                }

                // Przekierowanie po udanej aktualizacji
                this.router.navigate(['/']);
            },
            error: (err) => {
                console.error('Błąd aktualizacji:', err);
                this.errors = [err?.error ?? 'Wystąpił błąd połączenia z serwerem.'];
            },
            complete: () => {
                console.log('Żądanie zakończone.');
            }
        });
    }
}
