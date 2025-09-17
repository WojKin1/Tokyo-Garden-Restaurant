// src/main.ts
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';

import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

// Interceptor wymuszający wysyłkę cookies (withCredentials: true)
import { credentialsInterceptor } from './app/interceptors/credentials.interceptor';

// Twój istniejący interceptor (np. dodający nagłówki autoryzacji)
import { authInterceptor } from './app/services/auth.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        credentialsInterceptor, // ważne, żeby był pierwszy
        authInterceptor,
      ])
    ),
    provideAnimations(),
  ],
}).catch(err => console.error(err));
