import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Screen } from '../../shared/models/screen.model';

@Injectable({
  providedIn: 'root'
})
export class ScreenService {

  private apiUrl = environment.apiUrl + '/screens';

  constructor(private http: HttpClient) { }

  getScreens(theatreId?: number): Observable<Screen[]> {
    let params = new HttpParams();

    if (theatreId) {
      params = params.set('theatreId', theatreId);
    }

    return this.http.get<Screen[]>(this.apiUrl, { params });
  }

  getScreenById(id: number): Observable<Screen> {
    return this.http.get<Screen>(`${this.apiUrl}/${id}`);
  }

  addScreen(screen: Partial<Screen>): Observable<Screen> {
    return this.http.post<Screen>(this.apiUrl, screen);
  }

  updateScreen(id: number, screen: Partial<Screen>): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, screen);
  }

  deleteScreen(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
