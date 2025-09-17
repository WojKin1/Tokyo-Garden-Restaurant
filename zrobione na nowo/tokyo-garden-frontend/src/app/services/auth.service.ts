// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user';

export interface LoginPayload {
  username: string;
  password: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api =
    (environment as any).apiUrl || (environment as any).apiBaseUrl;

  private currentUserSubject = new BehaviorSubject<User | null>(null);

  /** Strumień z aktualnym userem (lub null) */
  currentUser$ = this.currentUserSubject.asObservable();

  /** Getter kompatybilny ze starym użyciem: this.authService.currentUser */
  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  constructor(private http: HttpClient) {}

  /** Odczytaj użytkownika z sesji (cookie) i zapisz w pamięci */
  me(): Observable<User> {
    return this.http
      .get<User>(`${this.api}/uzytkownicy/me`, { withCredentials: true })
      .pipe(tap((u) => this.currentUserSubject.next(u)));
  }

  /** Wygodna metoda do przywrócenia sesji na starcie appki */
  restoreSession(): void {
    this.me().subscribe({
      next: () => {},
      error: () => {
        // brak sesji – zostaw null
        this.currentUserSubject.next(null);
      },
    });
  }

  /** Logowanie – backend ustawia cookie */
  login(payload: LoginPayload): Observable<User> {
    return this.http
      .post<User>(`${this.api}/uzytkownicy/login`, payload, {
        withCredentials: true,
      })
      .pipe(tap((u) => this.currentUserSubject.next(u)));
  }

  /** Wylogowanie – backend kasuje cookie */
  logout(): Observable<void> {
    return this.http
      .post<void>(`${this.api}/uzytkownicy/logout`, {}, { withCredentials: true })
      .pipe(tap(() => this.currentUserSubject.next(null)));
  }

  /** Czy ktoś jest zalogowany */
  isAuthenticated(): boolean {
    return !!this.currentUserSubject.value;
  }

  /** Czy bieżący user ma rolę Admin */
  isAdmin(): boolean {
    const role = this.currentUserSubject.value?.typUzytkownika;
    return (role ?? '').toLowerCase() === 'admin';
  }

  /** Dostęp synchroniczny – jeśli potrzebujesz */
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }
}
