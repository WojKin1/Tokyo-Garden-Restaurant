import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  editForm!: FormGroup;
  errors: string[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.editForm = this.fb.group({
      id: [0, Validators.required],
      nazwaUzytkownika: ['', [Validators.required, Validators.minLength(3)]],
      password: [''],   // opcjonalnie
      password2: [''],  // opcjonalnie
      telefon: ['', [Validators.required, Validators.pattern(/^\d{9}$/)]]
    });

    const stored = this.userService.getStoredUser();
    if (stored) {
      // można też odświeżyć z backendu:
      // this.userService.getById(stored.id).subscribe(u => { ... });
      this.editForm.patchValue({
        id: stored.id,
        nazwaUzytkownika: stored.nazwaUzytkownika,
        telefon: stored.telefon ?? ''
      });
    } else {
      this.errors = ['Brak zalogowanego użytkownika.'];
    }
  }

  onSubmit(): void {
    this.errors = [];
    if (this.editForm.invalid) return;

    const { password, password2 } = this.editForm.value;
    if ((password || password2) && password !== password2) {
      this.errors.push('Hasła muszą być takie same.');
      return;
    }

    this.userService.updateUser(this.editForm.value).subscribe({
      next: () => {
        // zaktualizuj storage, żeby header/home widziały nowe dane
        const u = this.userService.getStoredUser();
        if (u) {
          u.nazwaUzytkownika = this.editForm.value.nazwaUzytkownika;
          u.telefon = this.editForm.value.telefon;
          // zapisz tam, gdzie był (local lub session)
          if (localStorage.getItem('currentUser')) {
            localStorage.setItem('currentUser', JSON.stringify(u));
          } else {
            sessionStorage.setItem('currentUser', JSON.stringify(u));
          }
        }
        this.router.navigate(['/']);
      },
      error: (err) => {
        console.error(err);
        this.errors = [err?.error ?? 'Wystąpił błąd połączenia z serwerem.'];
      }
    });
  }
}
