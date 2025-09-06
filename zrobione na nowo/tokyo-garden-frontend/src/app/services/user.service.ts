import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

export interface UserDto {
    id: number;
    nazwaUzytkownika: string;
    telefon?: string;
    typUzytkownika?: string;
}

// Interfejs dla payloadu aktualizacji użytkownika
export interface UpdatePayload {
    id: number;
    nazwaUzytkownika: string;
    telefon?: string;
    password?: string; // Opcjonalne hasło do aktualizacji
}

@Injectable({ providedIn: 'root' })
export class UserService {
    private readonly apiUrl = '/api/Uzytkownicy';
    private readonly storageKey = 'currentUser';

    constructor(private http: HttpClient) { }

    /** użytkownik zapisany po logowaniu (localStorage/sessionStorage) */
    getStoredUser(): UserDto | null {
        const raw = localStorage.getItem(this.storageKey) || sessionStorage.getItem(this.storageKey);
        return raw ? (JSON.parse(raw) as UserDto) : null;
    }

    /** odśwież dane z API */
    getById(id: number): Observable<UserDto> {
        return this.http.get<UserDto>(`${this.apiUrl}/${id}`);
    }

    updateUser(data: UpdatePayload): Observable<any> {
        const current = this.getStoredUser();
        if (!current) return throwError(() => new Error('Brak zalogowanego użytkownika'));
        const payload: any = {
            id: data.id,
            nazwa_uzytkownika: data.nazwaUzytkownika,
            telefon: data.telefon ?? current.telefon ?? null,
            typ_uzytkownika: current.typUzytkownika ?? 'Uzytkownik',
            haslo: data.password || '' // Zawsze wysyłaj haslo, nawet puste
        };
        console.log('Wysyłany payload do updateUser:', payload); // Debug
        return this.http.put(`${this.apiUrl}/${data.id}`, payload);
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

    getAllUsers(): Observable<UserDto[]> {
        return this.http.get<UserDto[]>('/api/Uzytkownicy');
    }

}