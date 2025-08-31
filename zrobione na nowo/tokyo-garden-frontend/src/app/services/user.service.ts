import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

export interface UserDto {
  id: number;
  nazwaUzytkownika: string;
  telefon?: string;
  typUzytkownika?: string;
}

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly apiUrl = '/api/Uzytkownicy';
  private readonly storageKey = 'currentUser';

  constructor(private http: HttpClient) {}

  /** użytkownik zapisany po logowaniu (localStorage/sessionStorage) */
  getStoredUser(): UserDto | null {
    const raw = localStorage.getItem(this.storageKey) || sessionStorage.getItem(this.storageKey);
    return raw ? (JSON.parse(raw) as UserDto) : null;
  }

  /** odśwież dane z API */
  getById(id: number): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.apiUrl}/${id}`);
  }

  /**
   * Aktualizacja użytkownika
   * Wysyłamy snake_case zgodnie z encją w backendzie:
   * id, nazwa_uzytkownika, telefon, (opcjonalnie) haslo, typ_uzytkownika
   */
  updateUser(data: {
    id: number;
    nazwaUzytkownika: string;
    telefon?: string;
    password?: string; // opcjonalnie
  }): Observable<any> {
    const current = this.getStoredUser();
    if (!current) return throwError(() => new Error('Brak zalogowanego użytkownika'));

    const payload: any = {
      id: data.id,
      nazwa_uzytkownika: data.nazwaUzytkownika,
      telefon: data.telefon ?? current.telefon ?? null,
      typ_uzytkownika: current.typUzytkownika ?? 'Uzytkownik'
    };
    if (data.password) payload.haslo = data.password; // tylko gdy podano nowe hasło

    // usuń undefined
    const cleaned = JSON.parse(JSON.stringify(payload));
    return this.http.put(`${this.apiUrl}/${data.id}`, cleaned);
  }

  /** nadpisz storage po udanej edycji (żeby header/home pokazywały aktualne dane) */
  saveToStorage(user: UserDto) {
    if (localStorage.getItem(this.storageKey)) {
      localStorage.setItem(this.storageKey, JSON.stringify(user));
    } else {
      sessionStorage.setItem(this.storageKey, JSON.stringify(user));
    }
  }

    /** Pobierz profil zalogowanego użytkownika z API */
    getCurrentUserProfile(): Observable<UserDto> {
        const current = this.getStoredUser();
        if (!current) return throwError(() => new Error('Brak zalogowanego użytkownika'));
        return this.getById(current.id);
    }
}

