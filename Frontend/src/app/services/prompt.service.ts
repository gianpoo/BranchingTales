import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Prompt } from '../models/prompt.interface';

@Injectable({
    providedIn: 'root'
})
export class PromptService {
    private apiUrl = 'https://localhost:57679/Prompts';

    constructor(private http: HttpClient) { }

    getById(id: number): Observable<Prompt> {
        return this.http.get<Prompt>(`${this.apiUrl}/${id}`);
    }
} 