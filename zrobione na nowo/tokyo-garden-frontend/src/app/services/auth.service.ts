import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

// Dopasuj to do swojego DTO z backendu/templatu
export interface User {
  id: number;
  nazwaUzytkownika: string;
  typUzytkownika: string;
  telefon?: string | null;
}

export interface LoginPayload {
  username: string;
  password: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  // W env masz już pełny URL API z /api
  private readonly api = (environment as any).apiUrl || (environment as any).apiBaseUrl;

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  /** Strumień z aktualnym userem (lub null) */
  currentUser$ = this.currentUserSubject.asObservable();

  /** Getter kompatybilny ze starym użyciem: this.authService.currentUser */
  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  constructor(private http: HttpClient) {}

  /** Odczyt aktualnego usera z cookie-sesji */
  me(): Observable<User> {
    return this.http
      .get<User>(`${this.api}/uzytkownicy/me`, { withCredentials: true })
      .pipe(tap(u => this.currentUserSubject.next(u)));
  }

  /** Przywrócenie sesji na starcie aplikacji (wywołaj w AppComponent) */
  restoreSession(): void {
    this.me().subscribe({
      next: () => {},
      error: () => this.currentUserSubject.next(null)
    });
  }

  /** Logowanie – backend wystawia cookie, my zapamiętujemy usera */
  login(payload: LoginPayload): Observable<User> {
    return this.http
      .post<User>(`${this.api}/uzytkownicy/login`, payload, { withCredentials: true })
      .pipe(tap(u => this.currentUserSubject.next(u)));
  }

  /** Wylogowanie – backend zdejmuje cookie, my czyścimy pamięć */
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
}
