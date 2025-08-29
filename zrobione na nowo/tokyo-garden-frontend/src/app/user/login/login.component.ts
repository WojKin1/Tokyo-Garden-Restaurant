import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginData = { login: '', password: '', rememberMe: false };
  errors: string[] = [];
  loading = false;

  constructor(private http: HttpClient, private router: Router, private auth: AuthService) {}

  login(): void {
    if (!this.loginData.login || !this.loginData.password) {
      this.errors = ['Proszę wypełnić wszystkie pola.'];
      return;
    }

    this.errors = [];
    this.loading = true;

    this.http.post<any>('/api/Uzytkownicy/login', {
      username: this.loginData.login,
      password: this.loginData.password
    })
    .subscribe({
      next: (user) => {
        // Zapisz usera do storage
        const storage = this.loginData.rememberMe ? localStorage : sessionStorage;
        storage.setItem('currentUser', JSON.stringify(user));

        this.loading = false;
        this.router.navigate(['/']);
      },
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
