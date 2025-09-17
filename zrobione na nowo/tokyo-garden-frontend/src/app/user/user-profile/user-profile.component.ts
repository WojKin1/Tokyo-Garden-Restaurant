import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UserService, UserDto } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';

// Komponent wy�wietlaj�cy profil u�ytkownika
@Component({
    selector: 'app-user-profile',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './user-profile.component.html',
    styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
    // Dane zalogowanego u�ytkownika
    user: UserDto | null = null;

    // Czy u�ytkownik jest administratorem
    isAdmin = false;

    // Komunikat b��du, je�li u�ytkownik nie zosta� odnaleziony
    error: string | null = null;

    // Wstrzykiwanie serwis�w
    constructor(
        private userService: UserService,
        private authService: AuthService,
        private router: Router
    ) { }

    // Inicjalizacja komponentu
    ngOnInit(): void {
        // Pobranie danych u�ytkownika z lokalnego magazynu
        this.user = this.userService.getStoredUser();

        // Sprawdzenie typu u�ytkownika
        if (this.user) {
            this.isAdmin = this.user.typUzytkownika?.toLowerCase() === 'admin';
        } else {
            this.error = 'Nie znaleziono zalogowanego u�ytkownika.';
        }
    }

    // Przej�cie do panelu administratora
    goToAdmin() {
        this.router.navigate(['/admin']);
    }

    // Wylogowanie u�ytkownika
    logout() {
        this.authService.logout().subscribe({
            next: () => {
                this.user = null;
                window.location.href = '/';
            },
            error: (err) => {
                console.error('B��d podczas wylogowania:', err);

                this.user = null;
                window.location.href = '/';
            }
        });
    }


}

