import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PozycjaService } from '../../services/pozycja.service';

export interface AlergenDto {
  id: number;
  nazwa_alergenu: string;
}

export interface KategoriaDto {
  id: number;
  nazwa_kategorii: string;
}

export interface PozycjaDto {
  id: number;
  nazwa_pozycji: string;
  opis: string;
  cena: number;
  skladniki: string;
  alergeny?: AlergenDto[];
  kategoria_menu?: KategoriaDto;
}

@Component({
  selector: 'app-pozycja-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './pozycja-list.component.html',
  styleUrls: ['./pozycja-list.component.css']
})
export class PozycjaListComponent implements OnInit {
  pozycje: PozycjaDto[] = [];
  loading = false;
  errorMessage = '';

  constructor(private pozycjaService: PozycjaService) {}

  ngOnInit(): void {
    this.loadPozycje();
  }

  loadPozycje(): void {
    this.loading = true;
    this.errorMessage = '';

    this.pozycjaService.getAllPozycje().subscribe({
      next: (data) => {
        this.pozycje = data || [];
        this.loading = false;
      },
      error: (err) => {
        console.error('Błąd ładowania pozycji:', err);
        this.errorMessage = 'Nie udało się załadować pozycji.';
        this.loading = false;
      }
    });
  }

  deletePozycja(id: number): void {
    if (!confirm('Czy na pewno chcesz usunąć tę pozycję?')) return;

    this.pozycjaService.deletePozycja(id).subscribe({
      next: () => {
        this.pozycje = this.pozycje.filter(p => p.id !== id);
      },
      error: (err) => {
        console.error('Błąd usuwania pozycji:', err);
        alert('Wystąpił błąd podczas usuwania pozycji.');
      }
    });
  }
}
