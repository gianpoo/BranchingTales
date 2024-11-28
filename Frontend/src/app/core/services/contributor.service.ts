import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contributor } from '../models/contributor.interface';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class ContributorService {
  private endpoint = 'Contributors';

  constructor(
    private http: HttpClient,
    private apiService: ApiService
  ) { }

  getAll(): Observable<{ contributors: Contributor[] }> {
    return this.http.get<{ contributors: Contributor[] }>(
      this.apiService.createUrl(this.endpoint)
    );
  }

  create(contributor: Omit<Contributor, 'id'>): Observable<Contributor> {
    return this.http.post<Contributor>(
      this.apiService.createUrl(this.endpoint),
      contributor
    );
  }

  update(contributor: Contributor): Observable<Contributor> {
    return this.http.put<Contributor>(
      this.apiService.createUrl(`${this.endpoint}/${contributor.id}`),
      contributor
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(
      this.apiService.createUrl(`${this.endpoint}/${id}`)
    );
  }

  search(searchTerm: string): Observable<Contributor[]> {
    return this.http.get<Contributor[]>(
      this.apiService.createUrl(`${this.endpoint}/search/${encodeURIComponent(searchTerm)}`)
    );
  }

  getById(id: number): Observable<Contributor> {
    return this.http.get<Contributor>(
      this.apiService.createUrl(`${this.endpoint}/${id}`)
    );
  }
} 