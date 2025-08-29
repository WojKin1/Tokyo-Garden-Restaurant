import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PozycjaService {
  private apiUrl = '/api/PozycjeMenu';

  constructor(private http: HttpClient) {}

  getAllPozycje(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getPozycjaById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  createPozycja(pozycja: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, pozycja);
  }

  updatePozycja(id: number, pozycja: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, pozycja);
  }

  deletePozycja(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
