import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';

export interface MenuItemVm {
  id: number;
  nazwa: string;
  opis: string;
  cena: number;
  skladniki: string;
  kategoriaId?: number | null;
  kategoriaNazwa?: string | null;
  imageBase64?: string | null;
}

@Injectable({ providedIn: 'root' })
export class MenuService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<MenuItemVm[]> {
    return this.http.get<any[]>('/api/PozycjeMenu').pipe(
      map(items => (items ?? []).map(i => this.normalize(i)))
    );
  }

  private normalize(i: any): MenuItemVm {
    // Bezpiecznie odczytujemy zarówno snake_case jak i camelCase z ToDto()
    const kategoria = i.kategoria_menu ?? i.kategoriaMenu ?? i.kategoria ?? null;
    return {
      id: i.id ?? 0,
      nazwa: i.nazwa_pozycji ?? i.nazwaPozycji ?? '',
      opis: i.opis ?? '',
      cena: Number(i.cena ?? 0),
      skladniki: i.skladniki ?? '',
      kategoriaId: kategoria?.id ?? kategoria?.kategoriaId ?? null,
      kategoriaNazwa: kategoria?.nazwa_kategorii ?? kategoria?.nazwaKategorii ?? kategoria?.nazwa ?? null,
      imageBase64: i.image_data ?? i.imageData ?? null
    };
  }
}
