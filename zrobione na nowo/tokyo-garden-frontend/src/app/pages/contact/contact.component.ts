import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent {
  messageData = { name: '', email: '', message: '' };
  sending = false;

  constructor(private http: HttpClient) {}

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
      error: () => {
        alert('Nie udało się wysłać wiadomości.');
        this.sending = false;
      }
    });
  }
}
