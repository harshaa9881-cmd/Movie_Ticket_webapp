import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateShowRequest, Show } from '../../shared/models/show.model';
import { environment } from '../../../environments/environment';


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
  addShow(show: CreateShowRequest): Observable<Show> {
    return this.http.post<Show>(this.apiUrl, show);
  }

  updateShow(id: number, show: CreateShowRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, show);
  }

  deleteShow(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
