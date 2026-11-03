import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { BookingService } from '../../../core/services/booking.service';
import { BookingResponse } from '../../../shared/models/booking.model';

@Component({
  selector: 'app-booking-history',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './booking-history.html',
  styleUrl: './booking-history.scss'
})
export class BookingHistory implements OnInit {

  bookings: BookingResponse[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(private bookingService: BookingService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.bookingService.getBookings().subscribe({
      next: (response) => {
        this.bookings = response;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Booking History API Error:', error);
        this.errorMessage = 'Unable to load bookings.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  cancelBooking(id: number): void {
    if (!confirm('Are you sure you want to cancel this booking?')) {
      return;
    }

    this.bookingService.cancelBooking(id).subscribe({
      next: () => this.loadBookings(),
      error: () => {
        this.errorMessage = 'Unable to cancel this booking.';
        this.cdr.detectChanges();
      }
    });
  }
}
