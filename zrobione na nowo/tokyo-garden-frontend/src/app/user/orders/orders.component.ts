import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CartService } from '../../services/cart.service';
import { OrderService } from '../../services/order.service';
import { AdresService } from '../../services/adres.service';
import { CartItem } from '../../models/cart-item';
import { AuthService } from '../../services/auth.service';
import { forkJoin } from 'rxjs';
import { UserService } from '../../services/user.service';

interface OrderData {
    orderOptions: string;
    city: string;
    street: string;
    houseNumber: string;
    apartmentNumber: string;
    postalCode: string;
    orderComments: string;
    paymentMethod: string;
}

@Component({
    selector: 'app-orders',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './orders.component.html',
    styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
    cart: CartItem[] = [];
    totalPrice = 0;
    additionalFee = 0;
    showAddressFields = false;
    orderData: OrderData = {
        orderOptions: 'W Restauracji',
        city: '',
        street: '',
        houseNumber: '',
        apartmentNumber: '',
        postalCode: '',
        orderComments: '',
        paymentMethod: 'Karta Płatnicza'
    };

    constructor(
        private cartService: CartService,
        private http: HttpClient,
        private router: Router,
        private orderService: OrderService,
        private adresService: AdresService,
        private authService: AuthService,
        private userService: UserService,
    ) { }

    userOrders: any[] = [];

    ngOnInit() {
        forkJoin({
            orders: this.orderService.getOrdersWithUsers(),
            users: this.userService.getAllUsers()
        }).subscribe(({ orders, users }) => {
            this.userOrders = orders.map(order => ({
                ...order,
                user: order.uzytkownik || users.find(u => u.id === order.uzytkownikid) || null
            }));
        });

        this.loadCart();
    }

    loadCart() {
        this.cartService.getCart().subscribe((data: CartItem[]) => {
            this.cart = data;
            this.totalPrice = this.cart.reduce((sum, item) => sum + item.cena * item.ilosc, 0);
            this.updateDeliveryOption();
        });
    }

    updateDeliveryOption() {
        this.additionalFee = 0;
        this.showAddressFields = false;
        if (this.orderData.orderOptions === 'Na Dowóz') {
            this.additionalFee = 20;
            this.showAddressFields = true;
        } else if (this.orderData.orderOptions === 'Na Wynos') {
            this.additionalFee = 5;
        }
    }

    submitOrder() {
        const now = new Date();
        const currentDate = now.toISOString().replace('Z', '+02:00');

        const user = this.authService.currentUser;
        if (!user?.id) {
            alert('Nie jesteś zalogowany. Zaloguj się, aby złożyć zamówienie.');
            this.router.navigate(['/login']);
            return;
        }
        //const userId = user.id;

        const createOrderPayload = (additionalInfo: string) => ({
            opcje_zamowienia: this.orderData.orderOptions,
            metoda_platnosci: this.orderData.paymentMethod,
            laczna_cena: this.totalPrice + this.additionalFee,
            dodatkowe_informacje: additionalInfo,
            data_zamowienia: currentDate,
            pozycje: this.cart.map(item => ({
                ilosc: item.ilosc,
                cena: item.cena,
                pozycja_menu: { id: item.id }
            }))
        });




        if (this.orderData.orderOptions === 'Na Dowóz') {
            const newAdres = {
                miasto: this.orderData.city,
                ulica: this.orderData.street,
                nr_domu: this.orderData.houseNumber?.toString(),
                nr_mieszkania: this.orderData.apartmentNumber?.toString() || ""
            };

            console.log('Sprawdzanie czy adres już istnieje...');

            this.adresService.getAllAdresy().subscribe({
                next: (adresy: any[]) => {
                    const existing = adresy.find(a =>
                        a.miasto === newAdres.miasto &&
                        a.ulica === newAdres.ulica &&
                        a.nr_domu === newAdres.nr_domu &&
                        a.nr_mieszkania === newAdres.nr_mieszkania
                    );

                    const saveAdres$ = existing
                        ? Promise.resolve(existing) 
                        : this.adresService.createAdres(newAdres).toPromise(); 

                    saveAdres$.then((adresResponse: any) => {
                        console.log('Adres do użycia:', adresResponse);

                        const adresParts = [];
                        if (newAdres.miasto) adresParts.push(`Miasto: ${newAdres.miasto}`);
                        if (newAdres.ulica) adresParts.push(`Ulica: ${newAdres.ulica}`);
                        if (newAdres.nr_domu) adresParts.push(`Nr domu: ${newAdres.nr_domu}`);
                        if (newAdres.nr_mieszkania) adresParts.push(`Nr mieszkania: ${newAdres.nr_mieszkania}`);

                        const adresString = adresParts.join('; ');
                        const dodatkoweInfo = this.orderData.orderComments
                            ? `${adresString}<br><br>${this.orderData.orderComments}`
                            : adresString;

                        const orderPayload = createOrderPayload(dodatkoweInfo);
                        console.log('Payload zamówienia wysyłany do API:', orderPayload);

                        this.orderService.createOrder(orderPayload).subscribe({
                            next: () => {
                                alert('Zamówienie złożone pomyślnie!');
                                this.cartService.clearCart();
                                this.router.navigate(['/']);
                            },
                            error: (err) => {
                                console.error('Błąd składania zamówienia:', err);
                                alert('Błąd podczas składania zamówienia.');
                            }
                        });
                    }).catch(err => {
                        console.error('Błąd podczas obsługi adresu:', err);
                        alert('Błąd podczas sprawdzania adresu.');
                    });
                },
                error: (err) => {
                    console.error('Błąd pobierania adresów:', err);
                    alert('Błąd podczas pobierania adresów.');
                }
            });
        } else {
            const orderPayload = createOrderPayload(this.orderData.orderComments);
            console.log('Payload zamówienia wysyłany do API:', orderPayload);

            this.orderService.createOrder(orderPayload).subscribe({
                next: () => {
                    alert('Zamówienie złożone pomyślnie!');
                    this.cartService.clearCart();
                    this.router.navigate(['/']);
                },
                error: (err) => {
                    console.error('Błąd składania zamówienia:', err);
                    alert('Błąd podczas składania zamówienia.');
                }
            });
        }
    }

    goToHome(): void {
        this.router.navigate(['/']);
    }
}
