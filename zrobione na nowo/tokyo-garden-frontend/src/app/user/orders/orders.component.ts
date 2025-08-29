import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CartService } from '../../services/cart.service';

interface CartItem {
  id: number;
  nazwa_pozycji: string;
  cena: number;
  image_data?: string;
}

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
    private router: Router
  ) {}

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.cartService.getCart().subscribe((data: CartItem[]) => {
      this.cart = data;
      this.totalPrice = this.cart.reduce((sum, item) => sum + item.cena, 0);
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
    if (
      this.orderData.orderOptions === 'Na Dowóz' &&
      (!this.orderData.city || !this.orderData.street)
    ) {
      alert('Proszę uzupełnić adres dostawy.');
      return;
    }

    const endpointMap: Record<string, string> = {
      'W Restauracji': 'orders-restaurant',
      'Na Wynos': 'orders-takeaway',
      'Na Dowóz': 'orders-delivery'
    };

    const payload = {
      ...this.orderData,
      totalPrice: this.totalPrice + this.additionalFee,
      cart: this.cart
    };

    this.http
      .post(
        `/api/${endpointMap[this.orderData.orderOptions]}`,
        payload
      )
      .subscribe({
        next: () => {
          alert('Zamówienie złożone pomyślnie!');
          this.router.navigate(['/']);
        },
        error: () => alert('Błąd podczas składania zamówienia.')
      });
  }
}
