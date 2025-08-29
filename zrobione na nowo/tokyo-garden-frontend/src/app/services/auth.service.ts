import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private storageKey = 'currentUser';

  get currentUser(): any {
    const user = localStorage.getItem(this.storageKey) || sessionStorage.getItem(this.storageKey);
    return user ? JSON.parse(user) : null;
  }

  isLoggedIn(): boolean {
    return !!this.currentUser;
  }

  logout() {
    localStorage.removeItem(this.storageKey);
    sessionStorage.removeItem(this.storageKey);
  }
}
