import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export interface LoggedUser {
  id: number;
  nazwaUzytkownika: string;
  typUzytkownika: string;
  telefon?: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly base = environment.apiUrl; // np. https://.../api
  private currentUserSubject = new BehaviorSubject<LoggedUser | null>(this.getStoredUser());
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  private getStoredUser(): LoggedUser | null {
    try {
      const raw = localStorage.getItem('currentUser');
      return raw ? (JSON.parse(raw) as LoggedUser) : null;
    } catch { return null; }
  }

  private setUser(u: LoggedUser | null) {
    if (u) localStorage.setItem('currentUser', JSON.stringify(u));
    else localStorage.removeItem('currentUser');
    this.currentUserSubject.next(u);
  }

  login(nazwa_uzytkownika: string, haslo: string) {
    // Backend: POST /api/Uzytkownicy/login { Username, Password }
    return this.http.post<LoggedUser>(`${this.base}/Uzytkownicy/login`, {
      username: nazwa_uzytkownika,
      password: haslo
    }).pipe(map(u => { this.setUser(u); return u; }));
  }

  logout() {
    this.http.post(`${this.base}/Uzytkownicy/logout`, {}, { responseType: 'text' }).subscribe();
    this.setUser(null);
  }

  /** Przywróć sesję z cookie po odświeżeniu strony */
  restoreSession() {
    this.http.get<LoggedUser>(`${this.base}/Uzytkownicy/me`)
      .pipe(
        map(u => { this.setUser(u); return u; }),
        catchError(() => { this.setUser(null); return of(null); })
      )
      .subscribe();
  }

  get currentUser(): LoggedUser | null {
    return this.currentUserSubject.value;
  }
}
