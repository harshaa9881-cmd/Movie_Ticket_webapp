import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Theatre } from '../../shared/models/theatre.model';

@Injectable({
  providedIn: 'root'
})
export class TheatreService {

  private apiUrl = environment.apiUrl + '/theatres';

  constructor(private http: HttpClient) { }

  getTheatres(): Observable<Theatre[]> {
    return this.http.get<Theatre[]>(this.apiUrl);
  }

  getTheatreById(id: number): Observable<Theatre> {
    return this.http.get<Theatre>(`${this.apiUrl}/${id}`);
  }

  addTheatre(theatre: Partial<Theatre>): Observable<Theatre> {
    return this.http.post<Theatre>(this.apiUrl, theatre);
  }

  updateTheatre(id: number, theatre: Partial<Theatre>): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, theatre);
  }

  deleteTheatre(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
