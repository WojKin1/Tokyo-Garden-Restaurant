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
  // Baza API – w env masz już /api na końcu
  private readonly api = (environment as any).apiUrl || (environment as any).apiBaseUrl;

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  /** Observable z aktualnym userem (lub null) */
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  /** Wywołaj przy starcie aplikacji – odczyt zalogowanego usera z cookie-sesji */
  me(): Observable<User> {
    return this.http.get<User>(`${this.api}/uzytkownicy/me`, { withCredentials: true })
      .pipe(tap(u => this.currentUserSubject.next(u)));
  }

  /** Logowanie – backend wystawi cookie, a my odkładamy usera w pamięci */
  login(payload: LoginPayload): Observable<User> {
    return this.http.post<User>(`${this.api}/uzytkownicy/login`, payload, { withCredentials: true })
      .pipe(tap(u => this.currentUserSubject.next(u)));
  }

  /** Wylogowanie – backend zdejmie cookie, my czyścimy pamięć */
  logout(): Observable<void> {
    return this.http.post<void>(`${this.api}/uzytkownicy/logout`, {}, { withCredentials: true })
      .pipe(tap(() => this.currentUserSubject.next(null)));
  }

  /** Szybka informacja czy ktoś jest zalogowany */
  isAuthenticated(): boolean {
    return !!this.currentUserSubject.value;
  }

  /** Czy bieżący user ma rolę Admin (z DTO: typUzytkownika) */
  isAdmin(): boolean {
    const role = this.currentUserSubject.value?.typUzytkownika;
    return (role ?? '').toLowerCase() === 'admin';
  }

  /** Zwróć aktualnego usera synchronicznie (jeśli potrzebujesz) */
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }
}
