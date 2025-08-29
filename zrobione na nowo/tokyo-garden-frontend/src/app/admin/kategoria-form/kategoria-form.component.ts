import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { KategoriaService } from '../../services/kategoria.service';


@Component({
  selector: 'app-kategoria-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './kategoria-form.component.html',
  styleUrls: ['./kategoria-form.component.css']
})
export class KategoriaFormComponent implements OnInit {
  kategoria: any = {};
  isEditMode = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private kategoriaService: KategoriaService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.kategoriaService.getKategoriaById(+id).subscribe(data => {
        this.kategoria = data;
      });
    }
  }

  saveKategoria(): void {
    if (this.isEditMode) {
      this.kategoriaService
        .updateKategoria(this.kategoria.id, this.kategoria)
        .subscribe(() => this.router.navigate(['/admin/kategorie']));
    } else {
      this.kategoriaService
        .createKategoria(this.kategoria)
        .subscribe(() => this.router.navigate(['/admin/kategorie']));
    }
  }

  cancel(): void {
    this.router.navigate(['/admin/kategorie']);
  }
}
