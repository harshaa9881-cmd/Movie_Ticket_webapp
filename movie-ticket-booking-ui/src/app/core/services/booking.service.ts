import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  BookingResponse,
  CreateBookingRequest
} from '../../shared/models/booking.model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  private apiUrl = environment.apiUrl + '/bookings';

  constructor(private http: HttpClient) { }

  createBooking(request: CreateBookingRequest): Observable<BookingResponse> {
    return this.http.post<BookingResponse>(this.apiUrl, request);
  }

  getBookings(showId?: number, status?: string): Observable<BookingResponse[]> {
    let params = new HttpParams();

    if (showId !== undefined) {
      params = params.set('showId', showId);
    }

    if (status) {
      params = params.set('status', status);
    }

    return this.http.get<BookingResponse[]>(this.apiUrl, { params });
  }

  getBookingById(id: number): Observable<BookingResponse> {
    return this.http.get<BookingResponse>(`${this.apiUrl}/${id}`);
  }

  cancelBooking(id: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/cancel`, {});
  }
}
