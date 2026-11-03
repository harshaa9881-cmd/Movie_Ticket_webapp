import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { ShowSeatService } from '../../../core/services/show-seat.service';
import { BookingService } from '../../../core/services/booking.service';
import { ShowSeat, SeatRow } from '../../../shared/models/show-seat.model';
import { CreateBookingRequest } from '../../../shared/models/booking.model';

@Component({
  selector: 'app-seat-selection',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './seat-selection.html',
  styleUrl: './seat-selection.scss'
})
export class SeatSelection implements OnInit {

  showId = 0;
  seatRows: SeatRow[] = [];
  selectedSeats: ShowSeat[] = [];

  customerName = '';
  customerEmail = '';
  customerPhone = '';

  isLoading = false;
  isBooking = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private showSeatService: ShowSeatService,
    private bookingService: BookingService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.showId = Number(this.route.snapshot.paramMap.get('showId'));

    if (!this.showId) {
      this.errorMessage = 'Invalid show ID.';
      return;
    }

    this.loadSeats();
  }

  loadSeats(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.showSeatService.getShowSeats(this.showId).subscribe({
      next: (response) => {
        this.createSeatRows(response);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Seat API Error:', error);
        this.errorMessage = 'Unable to load seats. Please try again.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  createSeatRows(seats: ShowSeat[]): void {
    this.seatRows = [];

    seats.forEach(seat => {
      let row = this.seatRows.find(item => item.rowName === seat.rowName);

      if (!row) {
        row = { rowName: seat.rowName, seats: [] };
        this.seatRows.push(row);
      }

      row.seats.push(seat);
    });

    this.seatRows.forEach(row => {
      row.seats.sort((a, b) => a.seatNumber - b.seatNumber);
    });

    this.seatRows.sort((a, b) => a.rowName.localeCompare(b.rowName));
  }

  isAvailable(seat: ShowSeat): boolean {
    return seat.status.toLowerCase() === 'available';
  }

  isSelected(seat: ShowSeat): boolean {
    return this.selectedSeats.some(item => item.seatId === seat.seatId);
  }

  toggleSeat(seat: ShowSeat): void {
    if (!this.isAvailable(seat)) {
      return;
    }

    if (this.isSelected(seat)) {
      this.selectedSeats = this.selectedSeats.filter(
        item => item.seatId !== seat.seatId
      );
    } else {
      this.selectedSeats.push(seat);
    }
  }

  getTotalPrice(): number {
    return this.selectedSeats.reduce(
      (total, seat) => total + seat.price,
      0
    );
  }

  clearSelection(): void {
    this.selectedSeats = [];
  }

  createBooking(): void {
    if (this.selectedSeats.length === 0) {
      this.errorMessage = 'Please select at least one seat.';
      return;
    }

    if (!this.customerName.trim() ||
      !this.customerEmail.trim() ||
      !this.customerPhone.trim()) {
      this.errorMessage = 'Please enter all customer details.';
      return;
    }

    const request: CreateBookingRequest = {
      customerName: this.customerName.trim(),
      customerEmail: this.customerEmail.trim(),
      customerPhone: this.customerPhone.trim(),
      showId: this.showId,
      seatIds: this.selectedSeats.map(seat => seat.seatId)
    };

    this.isBooking = true;
    this.errorMessage = '';

    this.bookingService.createBooking(request).subscribe({
      next: (booking) => {
        this.isBooking = false;
        this.cdr.detectChanges();

        this.router.navigate(
          ['/customer/booking-confirmation'],
          { state: { booking } }
        );
      },
      error: (error) => {
        this.isBooking = false;
        this.errorMessage =
          error.error?.message || 'Unable to create booking. Please try again.';

        this.cdr.detectChanges();

        this.loadSeats();
      }
    });
  }
}
