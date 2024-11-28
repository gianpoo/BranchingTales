import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Prompt } from '@models/prompt.interface';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class PromptService {
  private endpoint = 'Prompts';

  constructor(
    private http: HttpClient,
    private apiService: ApiService
  ) { }

  getById(id: number): Observable<Prompt> {
    return this.http.get<Prompt>(
      this.apiService.createUrl(`${this.endpoint}/${id}`)
    );
  }
} 