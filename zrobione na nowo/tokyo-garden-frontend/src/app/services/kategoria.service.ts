import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class KategoriaService {
  private apiUrl = '/api/kategorie';

  constructor(private http: HttpClient) {}

  getAllKategorie(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getKategoriaById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  createKategoria(kategoria: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, kategoria);
  }

  updateKategoria(id: number, kategoria: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, kategoria);
  }

  deleteKategoria(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
