import { Routes } from '@angular/router';

export const routes: Routes = [
  // Strona główna
  { path: '', loadComponent: () => import('./user/home/home.component').then(m => m.HomeComponent) },
  { path: 'menu', loadComponent: () => import('./shared/menu/menu.component').then(m => m.MenuComponent) },
  
  // Nowe strony
  { path: 'about', loadComponent: () => import('./pages/about/about.component').then(m => m.AboutComponent) },
  { path: 'contact', loadComponent: () => import('./pages/contact/contact.component').then(m => m.ContactComponent) },

  // Logowanie i rejestracja
  { path: 'login', loadComponent: () => import('./user/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./user/register/register.component').then(m => m.RegisterComponent) },

  // Koszyk i zamówienia
  { path: 'cart', loadComponent: () => import('./user/cart/cart.component').then(m => m.CartComponent) },
  { path: 'orders', loadComponent: () => import('./user/orders/orders.component').then(m => m.OrdersComponent) },

  //Moj profil
  { path: 'user/profile', loadComponent: () => import('./user/user-profile/user-profile.component').then(m => m.UserProfileComponent) },
    { path: 'profile', redirectTo: 'user/profile', pathMatch: 'full' },

  // Edycja danych użytkownika
  { path: 'user/edit', loadComponent: () => import('./user/user-edit/user-edit.component').then(m => m.UserEditComponent) },

  //Panel admina:
  { path: 'admin', loadComponent: () => import('./admin/admin-panel/admin-panel.component').then(m => m.AdminPanelComponent) },

  // Admin - alergeny
  { path: 'admin/alergeny', loadComponent: () => import('./admin/alergen-list/alergen-list.component').then(m => m.AlergenListComponent) },
  { path: 'admin/alergen/new', loadComponent: () => import('./admin/alergen-form/alergen-form.component').then(m => m.AlergenFormComponent) },
  { path: 'admin/alergen/edit/:id', loadComponent: () => import('./admin/alergen-form/alergen-form.component').then(m => m.AlergenFormComponent) },

  // Admin - kategorie
  { path: 'admin/kategorie', loadComponent: () => import('./admin/kategoria-list/kategoria-list.component').then(m => m.KategoriaListComponent) },
  { path: 'admin/kategoria/new', loadComponent: () => import('./admin/kategoria-form/kategoria-form.component').then(m => m.KategoriaFormComponent) },
  { path: 'admin/kategoria/edit/:id', loadComponent: () => import('./admin/kategoria-form/kategoria-form.component').then(m => m.KategoriaFormComponent) },

  // Admin - pozycje menu
  { path: 'admin/pozycje', loadComponent: () => import('./admin/pozycja-list/pozycja-list.component').then(m => m.PozycjaListComponent) },
  { path: 'admin/pozycja/new', loadComponent: () => import('./admin/pozycja-form/pozycja-form.component').then(m => m.PozycjaFormComponent) },
  { path: 'admin/pozycja/edit/:id', loadComponent: () => import('./admin/pozycja-form/pozycja-form.component').then(m => m.PozycjaFormComponent) },

  // Admin - zmiana statusu zamowienia
  { path: 'admin/zamowienia', loadComponent: () => import('./admin/order-status/order-status.component').then(m => m.OrderStatusComponent) },

  // Fallback na home
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
