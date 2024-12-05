import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PromptService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  create(text: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Chats`, { text });
  }

  addPrompt(text: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Chats/prompts`, { text });
  }

  getRandomResponse(): Observable<{ options: string[] }> {
    return this.http.get<{ options: string[] }>(`${this.apiUrl}/Chats/responses/random`);
  }

  getAllPrompts(): Observable<{ prompts: Array<{ id: number, text: string }> }> {
    return this.http.get<{ prompts: Array<{ id: number, text: string }> }>(`${this.apiUrl}/Chats/prompts`);
  }
} 