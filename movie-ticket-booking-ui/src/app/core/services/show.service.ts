import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Show } from '../../shared/models/show.model';

@Injectable({
  providedIn: 'root'
})
export class ShowService {

  private apiUrl = environment.apiUrl + '/Shows';

  constructor(private http: HttpClient) { }

  getShows(): Observable<Show[]> {
    return this.http.get<Show[]>(this.apiUrl);
  }

  getShowsByTheatre(theatreId: number): Observable<Show[]> {
    return this.http.get<Show[]>(
      `${this.apiUrl}/theatre/${theatreId}`
    );
  }

  getShowsByMovie(movieId: number): Observable<Show[]> {
    return this.http.get<Show[]>(
      `${this.apiUrl}/movie/${movieId}`
    );
  }
}
