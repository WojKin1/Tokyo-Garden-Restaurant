import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-order-status',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './order-status.component.html',
    styleUrls: ['./order-status.component.css']
})
export class OrderStatusComponent implements OnInit {
    orders: any[] = [];
    filteredOrders: any[] = [];
    statusOptions = ['Nowy', 'W realizacji', 'Wyslany', 'Zakonczony', 'Anulowany'];
    recordLimitOptions = [5, 10, 15, 25, 50, 100];
    timeFilterOptions = [1, 3, 6, 12, 24, 36, 48];
    selectedRecordLimit: number = 15; 
    selectedTimeFilter: number = 6;
    hideCompletedAndCanceled: boolean = false; 

    constructor(
        private orderService: OrderService,
        private authService: AuthService,
        private router: Router
    ) { }

    ngOnInit() {
        if (!this.authService.isAdmin()) {
            alert('Brak uprawnien. Tylko administratorzy moga zarzadzac statusami zamowien.');
            this.router.navigate(['/']);
            return;
        }

        this.orderService.getOrdersWithUsers().subscribe({
            next: (orders) => {
                this.orders = orders;
                console.log('Raw orders with user data:', JSON.stringify(orders, null, 2));
                console.log('First order user data:', orders[0]?.uzytkownik);
                this.applyFilters();
                console.log('Filtered orders (raw JSON):', JSON.stringify(this.filteredOrders, null, 2));
                console.log('First filtered order:', this.filteredOrders[0]?.statusZamowienia);
            },
            error: (err) => {
                console.error('Error fetching orders:', err);
                alert('Wystapi³ b³ad podczas ³adowania zamowien.');
            }
        });
    }

    applyFilters() {
        const timeThreshold = new Date(Date.now() - this.selectedTimeFilter * 60 * 60 * 1000); 
        this.filteredOrders = this.orders
            .filter(order => new Date(order.dataZamowienia) >= timeThreshold) 
            .filter(order => !this.hideCompletedAndCanceled ||
                (order.statusZamowienia !== 'Zakonczony' && order.statusZamowienia !== 'Anulowany')) 
            .sort((a, b) => new Date(b.dataZamowienia).getTime() - new Date(a.dataZamowienia).getTime()) 
            .slice(0, this.selectedRecordLimit); 
    }

    updateOrderStatus(orderId: number, newStatus: string) {
        this.orderService.updateOrderStatus(orderId, newStatus).subscribe({
            next: (response) => {
                console.log('Order status updated:', response);
                alert('Status zamowienia zosta³ zaktualizowany.');
                const order = this.orders.find(o => o.id === orderId); 
                if (order) {
                    order.statusZamowienia = newStatus;
                    this.applyFilters(); 
                }
            },
            error: (err) => {
                console.error('Error updating order status:', err);
                const errorMessage = err.error?.message || err.statusText || 'Unknown server error';
                alert(`Wystapi³ b³ad podczas aktualizacji statusu: ${errorMessage}`);
            }
        });
    }

    goToAdmin(): void {
        this.router.navigate(['/admin']);
    }
}