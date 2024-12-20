import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PromptService {
  private apiUrl = environment.apiUrl;
  private currentChatId = 1;

  constructor(private http: HttpClient) { }

  create(text: string, limit: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/Chats`, { text, limit });
  }

  addPrompt(text: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Chats/${this.currentChatId}/Prompts`, { text });
  }

  getOptions(): Observable<{ options: string[] }> {
    return this.http.get<{ options: string[] }>(`${this.apiUrl}/Chats/${this.currentChatId}/options`);
  }

  getChatPrompts(chatId: number): Observable<{ prompts: Array<{ id: number, text: string }> }> {
    return this.http.get<{ prompts: Array<{ id: number, text: string }> }>(
      `${this.apiUrl}/Chats/${chatId}/Prompts`
    );
  }
} 
