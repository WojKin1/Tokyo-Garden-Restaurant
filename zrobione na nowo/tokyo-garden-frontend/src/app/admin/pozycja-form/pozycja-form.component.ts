import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { PozycjaService } from '../../services/pozycja.service';
import { KategoriaService } from '../../services/kategoria.service';
import { AlergenService } from '../../services/alergen.service';

export interface KategoriaDto {
  id: number;
  nazwa_kategorii: string;
}

export interface AlergenDto {
  id: number;
  nazwa_alergenu: string;
}

export interface PozycjaDto {
  id?: number;
  nazwa_pozycji: string;
  opis: string;
  cena: number;
  skladniki: string;
  kategoria_id: number;
  alergeny: number[];
  image_data?: string;
}

@Component({
  selector: 'app-pozycja-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './pozycja-form.component.html',
  styleUrls: ['./pozycja-form.component.css']
})
export class PozycjaFormComponent implements OnInit {
  pozycja: PozycjaDto = {
    nazwa_pozycji: '',
    opis: '',
    cena: 0,
    skladniki: '',
    kategoria_id: 0,
    alergeny: []
  };

  isEditMode = false;
  kategorie: KategoriaDto[] = [];
  alergeny: AlergenDto[] = [];
  selectedFile: File | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private pozycjaService: PozycjaService,
    private kategoriaService: KategoriaService,
    private alergenService: AlergenService
  ) {}

  ngOnInit(): void {
    this.loadKategorie();
    this.loadAlergeny();

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEditMode = true;
      const id = Number(idParam);
      this.pozycjaService.getPozycjaById(id).subscribe({
        next: (data) => {
          this.pozycja = {
            ...data,
            alergeny: data.alergeny?.map((a: any) => a.id) || []
          };
        },
        error: (err) => console.error('Błąd ładowania pozycji', err)
      });
    }
  }

  loadKategorie(): void {
    this.kategoriaService.getAllKategorie().subscribe({
      next: (data) => this.kategorie = data,
      error: (err) => console.error('Błąd ładowania kategorii', err)
    });
  }

  loadAlergeny(): void {
    this.alergenService.getAllAlergeny().subscribe({
      next: (data) => this.alergeny = data,
      error: (err) => console.error('Błąd ładowania alergenów', err)
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  savePozycja(): void {
    const formData = new FormData();
    formData.append('nazwa_pozycji', this.pozycja.nazwa_pozycji);
    formData.append('opis', this.pozycja.opis);
    formData.append('cena', this.pozycja.cena.toString());
    formData.append('skladniki', this.pozycja.skladniki);
    formData.append('kategoria_id', this.pozycja.kategoria_id.toString());
    formData.append('alergeny', JSON.stringify(this.pozycja.alergeny));
    if (this.selectedFile) {
      formData.append('zdjecie', this.selectedFile);
    }

    if (this.isEditMode && this.pozycja.id) {
      this.pozycjaService.updatePozycja(this.pozycja.id, formData).subscribe({
        next: () => this.router.navigate(['/admin/pozycje']),
        error: (err) => console.error('Błąd aktualizacji pozycji', err)
      });
    } else {
      this.pozycjaService.createPozycja(formData).subscribe({
        next: () => this.router.navigate(['/admin/pozycje']),
        error: (err) => console.error('Błąd tworzenia pozycji', err)
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/admin/pozycje']);
  }
}
