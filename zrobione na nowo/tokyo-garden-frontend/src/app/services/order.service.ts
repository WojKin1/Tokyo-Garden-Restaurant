import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class OrderService {
    private apiUrl = '/api/zamowienia';

    constructor(private http: HttpClient) { }

    createOrder(order: any): Observable<any> {
        return this.http.post<any>(this.apiUrl, order);
    }
}
