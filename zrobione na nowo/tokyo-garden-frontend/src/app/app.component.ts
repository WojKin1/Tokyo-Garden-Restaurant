import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'Tokyo Garden';

  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    // Przywraca sesję z cookie po odświeżeniu strony
    this.auth.restoreSession();
  }
}
