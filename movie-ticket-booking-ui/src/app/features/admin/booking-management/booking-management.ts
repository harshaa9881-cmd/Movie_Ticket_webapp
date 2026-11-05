import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookingService } from '../../../core/services/booking.service';
import { BookingResponse } from '../../../shared/models/booking.model';

@Component({
  selector: 'app-booking-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './booking-management.html',
  styleUrl: './booking-management.scss'
})
export class BookingManagement implements OnInit {

  bookings: BookingResponse[] = [];
  selectedBooking: BookingResponse | null = null;

  showId = '';
  status = '';

  message = '';
  errorMessage = '';

  constructor(private bookingService: BookingService) { }

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.message = '';
    this.errorMessage = '';

    this.bookingService.getBookings(
      this.showId ? Number(this.showId) : undefined,
      this.status || undefined
    ).subscribe({
      next: (response: any) => {
        this.bookings = Array.isArray(response)
          ? response
          : response.items || [];
      },
      error: () => {
        this.errorMessage = 'Unable to load bookings.';
      }
    });
  }

  viewBooking(id: number): void {
    this.selectedBooking = null;

    this.bookingService.getBookingById(id).subscribe({
      next: (booking) => {
        this.selectedBooking = booking;
      },
      error: () => {
        this.errorMessage = 'Unable to load booking details.';
      }
    });
  }

  cancelBooking(id: number): void {
    if (!confirm('Are you sure you want to cancel this booking?')) {
      return;
    }

    this.bookingService.cancelBooking(id).subscribe({
      next: () => {
        this.message = 'Booking cancelled successfully.';
        this.selectedBooking = null;
        this.loadBookings();
      },
      error: () => {
        this.errorMessage = 'Unable to cancel this booking.';
      }
    });
  }

  clearFilters(): void {
    this.showId = '';
    this.status = '';
    this.loadBookings();
  }
}
