import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  Seat,
  CreateSeatRequest,
  BulkSeatRequest
} from '../../shared/models/seat.model';

@Injectable({
  providedIn: 'root'
})
export class SeatService {

  private apiUrl = environment.apiUrl + '/Seats';

  constructor(private http: HttpClient) { }

  getSeatsByScreen(screenId: number): Observable<Seat[]> {
    return this.http.get<Seat[]>(
      `${this.apiUrl}/screen/${screenId}`
    );
  }

  getSeatById(id: number): Observable<Seat> {
    return this.http.get<Seat>(`${this.apiUrl}/${id}`);
  }

  addSeat(seat: CreateSeatRequest): Observable<Seat> {
    return this.http.post<Seat>(this.apiUrl, seat);
  }

  addBulkSeats(request: BulkSeatRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/bulk`, request);
  }

  updateSeat(id: number, seat: Partial<Seat>): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, seat);
  }

  deleteSeat(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
