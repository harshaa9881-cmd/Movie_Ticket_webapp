import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { ShowSeatService } from '../../../core/services/show-seat.service';
import { ShowSeat, SeatRow } from '../../../shared/models/show-seat.model';

@Component({
  selector: 'app-seat-selection',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './seat-selection.html',
  styleUrl: './seat-selection.scss'
})
export class SeatSelection implements OnInit {

  showId = 0;

  seatRows: SeatRow[] = [];
  selectedSeats: ShowSeat[] = [];

  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private showSeatService: ShowSeatService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.showId = Number(
      this.route.snapshot.paramMap.get('showId')
    );

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

      let row = this.seatRows.find(
        item => item.rowName === seat.rowName
      );

      if (!row) {
        row = {
          rowName: seat.rowName,
          seats: []
        };

        this.seatRows.push(row);
      }

      row.seats.push(seat);
    });

    this.seatRows.forEach(row => {
      row.seats.sort((a, b) => a.seatNumber - b.seatNumber);
    });

    this.seatRows.sort((a, b) =>
      a.rowName.localeCompare(b.rowName)
    );
  }

  isAvailable(seat: ShowSeat): boolean {
    return seat.status.toLowerCase() === 'available';
  }

  isSelected(seat: ShowSeat): boolean {
    return this.selectedSeats.some(
      item => item.seatId === seat.seatId
    );
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
}
