import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contributor } from '../models/contributor.interface';

@Injectable({
    providedIn: 'root'
})
export class ContributorService {
    private apiUrl = 'https://localhost:57679/Contributors';

    constructor(private http: HttpClient) { }

    getAll(): Observable<{ contributors: Contributor[] }> {
        return this.http.get<{ contributors: Contributor[] }>(this.apiUrl);
    }

    getById(id: number): Observable<Contributor> {
        return this.http.get<Contributor>(`${this.apiUrl}/${id}`);
    }

    create(contributor: Omit<Contributor, 'id'>): Observable<Contributor> {
        return this.http.post<Contributor>(this.apiUrl, contributor);
    }

    update(contributor: Contributor): Observable<Contributor> {
        return this.http.put<Contributor>(`${this.apiUrl}/${contributor.id}`, contributor);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    search(searchTerm: string): Observable<Contributor[]> {
        return this.http.get<Contributor[]>(`${this.apiUrl}/search/${encodeURIComponent(searchTerm)}`);
    }
} 