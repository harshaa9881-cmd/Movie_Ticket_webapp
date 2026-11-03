import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ShowSeat } from '../../shared/models/show-seat.model';

@Injectable({
  providedIn: 'root'
})
export class ShowSeatService {

  private apiUrl = environment.apiUrl + '/shows';

  constructor(private http: HttpClient) { }

  getShowSeats(showId: number): Observable<ShowSeat[]> {
    return this.http.get<ShowSeat[]>(
      `${this.apiUrl}/${showId}/seats`
    );
  }
}
