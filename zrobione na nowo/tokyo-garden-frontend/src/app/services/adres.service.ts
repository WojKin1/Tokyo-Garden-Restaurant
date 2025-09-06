import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AdresService {
    private apiUrl = '/api/adresy';

    constructor(private http: HttpClient) { }

    createAdres(adres: any): Observable<any> {
        return this.http.post<any>(this.apiUrl, adres);
    }

    getAllAdresy(): Observable<any[]> {
        return this.http.get<any[]>(this.apiUrl);
    }
}
