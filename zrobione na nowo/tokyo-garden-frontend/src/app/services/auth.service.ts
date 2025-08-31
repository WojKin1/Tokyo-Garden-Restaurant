import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UzytkownikDTO } from '../models/uzytkownik-dto';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private storageKey = 'currentUser';

    constructor(private http: HttpClient) { }

    get currentUser(): any {
        const userStr = localStorage.getItem(this.storageKey) || sessionStorage.getItem(this.storageKey);
        return userStr ? JSON.parse(userStr) : null;
    }

    isAdmin(): boolean {
        return this.currentUser?.typUzytkownika === 'Admin';
    }

    login(username: string, password: string): Observable<UzytkownikDTO> {
        return this.http.post<UzytkownikDTO>('/api/uzytkownicy/login', { Username: username, Password: password })
            .pipe(
                tap(user => {
                    if (user) {
                        localStorage.setItem(this.storageKey, JSON.stringify(user));
                    }
                })
            );
    }

    logout() {
        localStorage.removeItem(this.storageKey);
        sessionStorage.removeItem(this.storageKey);
    }
}
