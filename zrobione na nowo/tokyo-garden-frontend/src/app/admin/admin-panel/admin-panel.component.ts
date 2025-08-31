import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserService, UserDto } from '../../services/user.service';

@Component({
    selector: 'app-admin-panel',
    standalone: true,
    // Dodajemy wymagane modu³y, CommonModule i RouterModule
    imports: [CommonModule, RouterModule],
    templateUrl: './admin-panel.component.html',
    styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
    // Przechowuje aktualnego u¿ytkownika pobranego ze storage
    currentUser: UserDto | null = null;

    // Flaga informuj¹ca czy aktualny u¿ytkownik jest administratorem
    isAdmin = false;

    // Komunikat wyœwietlany w panelu informuj¹cy o statusie u¿ytkownika
    statusMessage = '';

    // Wstrzykujemy serwis UserService do komponentu
    constructor(private userService: UserService) { }

    // Metoda wywo³ywana przy inicjalizacji komponentu
    ngOnInit(): void {
        // Pobieramy dane zalogowanego u¿ytkownika ze storage
        this.currentUser = this.userService.getStoredUser();

        // Sprawdzamy czy u¿ytkownik istnieje w storage
        if (this.currentUser) {
            // Ustawiamy flagê isAdmin na true jeœli typ u¿ytkownika to Admin
            this.isAdmin = this.currentUser.typUzytkownika?.toLowerCase() === 'admin';
            // Tworzymy komunikat informuj¹cy czy u¿ytkownik jest administratorem
            this.statusMessage = `${this.currentUser.nazwaUzytkownika} ${this.isAdmin ? 'jest' : 'nie jest'} adminem`;
        } else {
            // Jeœli brak zalogowanego u¿ytkownika ustawiamy komunikat informuj¹cy o braku logowania
            this.statusMessage = 'Nie zalogowano.';
        }

        // Wypisujemy aktualnego u¿ytkownika w konsoli do debugowania
        console.log('currentUser:', this.currentUser);
        // Wypisujemy status administratora w konsoli do debugowania
        console.log('isAdmin:', this.isAdmin);
    }
}
