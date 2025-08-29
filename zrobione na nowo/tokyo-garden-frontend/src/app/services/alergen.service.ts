import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlergenService {
  private apiUrl = '/api/alergeny'; // dopasuj do WebAPI

  constructor(private http: HttpClient) {}

  getAllAlergeny(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getAlergen(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  createAlergen(alergen: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, alergen);
  }

  updateAlergen(alergen: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${alergen.id}`, alergen);
  }

  deleteAlergen(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
