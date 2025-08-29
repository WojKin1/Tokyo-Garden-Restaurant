import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

interface Alergen {
  id: number;
  nazwa_Alergenu: string;
  opis_Alergenu: string;
}

interface PozycjaMenu {
  id: number;
  nazwa_pozycji: string;
  opis: string;
  skladniki: string;
  cena: number;
  image_data?: string;
  alergeny: Alergen[];
  kategoria_menu?: { id: number; nazwa_kategorii: string };
}

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  groupedMenu: { [category: string]: PozycjaMenu[] } = {};

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<PozycjaMenu[]>('/api/PozycjeMenu').subscribe({
      next: (data) => {
        // grupowanie po kategoriach
        this.groupedMenu = data.reduce((acc, item) => {
          const category = item.kategoria_menu?.nazwa_kategorii || 'Inne';
          if (!acc[category]) acc[category] = [];
          acc[category].push(item);
          return acc;
        }, {} as { [category: string]: PozycjaMenu[] });
      },
      error: (err) => {
        console.error('Błąd ładowania menu:', err);
      }
    });
  }

  addToCart(item: PozycjaMenu): void {
    console.log('Dodano do koszyka:', item);
    // TODO: wywołaj serwis koszyka
  }
}
