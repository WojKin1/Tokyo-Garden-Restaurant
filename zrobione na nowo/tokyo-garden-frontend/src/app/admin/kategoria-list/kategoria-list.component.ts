import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { KategoriaService } from '../../services/kategoria.service';

export interface KategoriaDto {
  id: number;
  nazwa_kategorii: string;
}

@Component({
  selector: 'app-kategoria-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './kategoria-list.component.html',
  styleUrls: ['./kategoria-list.component.css'],
})
export class KategoriaListComponent implements OnInit {
  kategorie: KategoriaDto[] = [];

  constructor(private kategoriaService: KategoriaService) {}

  ngOnInit(): void {
    this.loadKategorie();
  }

  loadKategorie(): void {
    this.kategoriaService.getAllKategorie().subscribe({
      next: (data) => (this.kategorie = data),
      error: (err) => console.error('Błąd ładowania kategorii', err),
    });
  }

  deleteKategoria(id: number): void {
    if (confirm('Czy na pewno chcesz usunąć tę kategorię?')) {
      this.kategoriaService.deleteKategoria(id).subscribe({
        next: () => this.loadKategorie(),
        error: (err) => console.error('Błąd usuwania kategorii', err),
      });
    }
  }
}
