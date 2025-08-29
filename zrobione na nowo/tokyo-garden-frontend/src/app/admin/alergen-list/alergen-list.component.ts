import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AlergenService } from '../../services/alergen.service';
import { AlergenDto } from '../alergen-form/alergen-form.component';

@Component({
  selector: 'app-alergen-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './alergen-list.component.html',
  styleUrls: ['./alergen-list.component.css'],
})
export class AlergenListComponent implements OnInit {
  alergeny: AlergenDto[] = [];

  constructor(private alergenService: AlergenService) {}

  ngOnInit(): void {
    this.loadAlergeny();
  }

  loadAlergeny(): void {
    this.alergenService.getAllAlergeny().subscribe({
      next: (data) => (this.alergeny = data),
      error: (err) => console.error('Błąd ładowania alergenów', err),
    });
  }

  deleteAlergen(id: number): void {
    if (confirm('Czy na pewno chcesz usunąć ten alergen?')) {
      this.alergenService.deleteAlergen(id).subscribe({
        next: () => this.loadAlergeny(),
        error: (err) => console.error('Błąd usuwania alergenu', err),
      });
    }
  }
}
