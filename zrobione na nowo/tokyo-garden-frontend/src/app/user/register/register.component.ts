import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';

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
  registerData: RegisterData = {
    nazwaUzytkownika: '',
    haslo1: '',
    haslo2: '',
    phone: '',
    regulamin: false
  };

  errors: string[] = [];

  constructor(private http: HttpClient, private router: Router) {}

  register() {
    this.errors = [];

    if (this.registerData.haslo1 !== this.registerData.haslo2) {
      this.errors.push('Hasła muszą być takie same.');
      return;
    }

    if (!this.registerData.regulamin) {
      this.errors.push('Musisz zaakceptować regulamin.');
      return;
    }

    if (!/^\d{9}$/.test(this.registerData.phone)) {
      this.errors.push('Numer telefonu musi mieć 9 cyfr.');
      return;
    }

    this.http.post<any>('/api/Uzytkownicy', {
      nazwa_uzytkownika: this.registerData.nazwaUzytkownika,
      haslo: this.registerData.haslo1,
      telefon: this.registerData.phone,
      typ_uzytkownika: "Uzytkownik"
    })
    .subscribe({
      next: () => {
        this.router.navigate(['/login']); // po rejestracji od razu na logowanie
      },
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
