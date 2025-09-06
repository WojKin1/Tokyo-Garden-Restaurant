import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router'; 
import { CartService, CartItem } from '../../services/cart.service';
import { Subscription } from 'rxjs';
import { UserService } from '../../services/user.service';

@Component({
    selector: 'app-header',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit, OnDestroy {
    isLoggedIn = false;
    isAdmin = false;
    currentUserName: string | null = null;
    cartCount = 0;
    private cartSubscription: Subscription | null = null;

    constructor(
        private cartService: CartService,
        private userService: UserService,
        private router: Router 
    ) { }

    ngOnInit(): void {
        console.log('HeaderComponent.ngOnInit dzia³a');
        this.loadUser();
        this.loadCart();
    }

    loadUser(): void {
        const current = this.userService.getStoredUser();
        if (current) {
            this.isLoggedIn = true;
            this.currentUserName = current.nazwaUzytkownika;
            this.isAdmin = current.typUzytkownika === 'Admin';
            console.log('U¿ytkownik zalogowany:', this.currentUserName, 'Admin:', this.isAdmin);
        } else {
            this.isLoggedIn = false;
            this.currentUserName = null;
            this.isAdmin = false;
            console.log('Brak zalogowanego u¿ytkownika');
        }
    }

    logout(): void {
        localStorage.removeItem('currentUser');
        sessionStorage.removeItem('currentUser');
        this.isLoggedIn = false;
        this.currentUserName = null;
        this.isAdmin = false;
        console.log('Wylogowano u¿ytkownika');
    }

    loadCart(): void {
        this.cartSubscription = this.cartService.getCart().subscribe({
            next: (items: CartItem[]) => {
                this.cartCount = items.reduce((sum, item) => sum + item.ilosc, 0);
                console.log('Liczba przedmiotów w koszyku:', this.cartCount);
            },
            error: (err) => {
                this.cartCount = 0;
                console.error('B³¹d podczas ³adowania koszyka:', err);
            },
            complete: () => {
                console.log('Subskrypcja koszyka zakoñczona');
            }
        });
    }

    navigateToCart(): void {
        this.router.navigate(['/cart']);
        console.log('Przekierowano do /cart');
    }

    ngOnDestroy(): void {
        this.cartSubscription?.unsubscribe();
        console.log('Subskrypcja koszyka anulowana');
    }
}