import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Movie, PagedResult } from '../../shared/models/movie.model';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  private apiUrl = environment.apiUrl + '/movies';

  constructor(private http: HttpClient) { }

  getMovies(
    pageNumber: number = 1,
    pageSize: number = 12,
    title: string = ''
  ): Observable<PagedResult<Movie>> {

    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (title.trim() !== '') {
      params = params.set('title', title);
    }

    return this.http.get<PagedResult<Movie>>(this.apiUrl, { params });
  }

  getMovieById(id: number): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/${id}`);
  }

  searchMovies(title: string): Observable<Movie[]> {
    const params = new HttpParams().set('title', title);

    return this.http.get<Movie[]>(`${this.apiUrl}/search`, { params });
  }
}
