import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AlergenService } from '../../services/alergen.service';

export interface AlergenDto {
  id: number;
  nazwa_alergenu: string;
  opis_alergenu: string;
  pozycje_menu: any[]; // można później wyodrębnić interfejs PozycjaMenuDto
}

@Component({
  selector: 'app-alergen-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './alergen-form.component.html',
  styleUrls: ['./alergen-form.component.css']
})
export class AlergenFormComponent implements OnInit {
  alergen: AlergenDto = {
    id: 0,
    nazwa_alergenu: '',
    opis_alergenu: '',
    pozycje_menu: []
  };
  isEditMode = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private alergenService: AlergenService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.alergenService.getAlergen(+id).subscribe({
        next: (data) => (this.alergen = data),
        error: (err) => console.error('Błąd pobierania alergenu', err)
      });
    }
  }

  saveAlergen(): void {
    if (this.isEditMode) {
      this.alergenService.updateAlergen(this.alergen).subscribe({
        next: () => this.router.navigate(['/admin/alergeny']),
        error: (err) => console.error('Błąd aktualizacji alergenu', err)
      });
    } else {
      this.alergenService.createAlergen(this.alergen).subscribe({
        next: () => this.router.navigate(['/admin/alergeny']),
        error: (err) => console.error('Błąd tworzenia alergenu', err)
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/admin/alergeny']);
  }
}
