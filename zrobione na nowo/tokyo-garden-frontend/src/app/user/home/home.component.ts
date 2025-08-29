import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';

interface MessageData {
  name: string;
  email: string;
  message: string;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  isLoggedIn = false;
  currentUserName: string | null = null;
  messageData: MessageData = { name: '', email: '', message: '' };
  sending = false;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.checkLoginStatus();
  }

  checkLoginStatus(): void {
    const userStr = localStorage.getItem('currentUser') || sessionStorage.getItem('currentUser');
    if (userStr) {
      const user = JSON.parse(userStr);
      this.isLoggedIn = true;
      this.currentUserName = user.nazwaUzytkownika;
    } else {
      this.isLoggedIn = false;
      this.currentUserName = null;
    }
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    sessionStorage.removeItem('currentUser');
    this.isLoggedIn = false;
    this.currentUserName = null;
  }

  sendMessage(): void {
    if (!this.messageData.name || !this.messageData.email || !this.messageData.message) {
      alert('Proszę wypełnić wszystkie pola formularza.');
      return;
    }

    this.sending = true;

    this.http.post('/api/messages', this.messageData).subscribe({
      next: () => {
        alert('Wiadomość wysłana!');
        this.messageData = { name: '', email: '', message: '' };
        this.sending = false;
      },
      error: (err) => {
        console.error('Błąd wysyłania wiadomości', err);
        alert('Nie udało się wysłać wiadomości.');
        this.sending = false;
      }
    });
  }
}
