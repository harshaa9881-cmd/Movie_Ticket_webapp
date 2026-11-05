import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SeatService } from '../../../core/services/seat.service';
import { ScreenService } from '../../../core/services/screen.service';
import { Seat } from '../../../shared/models/seat.model';
import { Screen } from '../../../shared/models/screen.model';

@Component({
  selector: 'app-seat-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './seat-management.html',
  styleUrl: './seat-management.scss'
})
export class SeatManagement implements OnInit {

  screens: Screen[] = [];
  seats: Seat[] = [];

  selectedScreenId = 0;

  rowName = '';
  startSeatNumber = 1;
  endSeatNumber = 10;
  seatType = 'Regular';

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private seatService: SeatService,
    private screenService: ScreenService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadScreens();
  }

  loadScreens(): void {
    this.screenService.getScreens().subscribe({
      next: (response) => {
        this.screens = response;
        this.cdr.detectChanges();

        console.log('Screens available for seat management:', response);
      },
      error: (error) => {
        console.error('Screen Dropdown API Error:', error);
        this.errorMessage = 'Unable to load screens.';
        this.cdr.detectChanges();
      }
    });
  }

  loadSeats(): void {
    if (!this.selectedScreenId) {
      this.seats = [];
      this.cdr.detectChanges();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.seatService.getSeatsByScreen(this.selectedScreenId).subscribe({
      next: (response) => {
        this.seats = response;
        this.isLoading = false;
        this.cdr.detectChanges();

        console.log('Seats loaded:', response);
      },
      error: (error) => {
        console.error('Seat API Error:', error);
        this.errorMessage = 'Unable to load seats.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  addBulkSeats(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedScreenId || !this.rowName.trim()) {
      this.errorMessage = 'Please select a screen and enter a row name.';
      return;
    }

    if (
      this.startSeatNumber < 1 ||
      this.endSeatNumber < this.startSeatNumber
    ) {
      this.errorMessage = 'Please enter a valid seat number range.';
      return;
    }

    const seats = [];

    for (
      let seatNumber = this.startSeatNumber;
      seatNumber <= this.endSeatNumber;
      seatNumber++
    ) {
      seats.push({
        rowName: this.rowName.trim(),
        seatNumber: seatNumber,
        seatType: this.seatType
      });
    }

    this.seatService.addBulkSeats({
      screenId: this.selectedScreenId,
      seats: seats
    }).subscribe({
      next: () => {
        this.successMessage = 'Seats created successfully.';
        this.loadSeats();
      },
      error: () => {
        this.errorMessage = 'Unable to create seats. Check for duplicate seats.';
      }
    });
  }

  deleteSeat(id: number): void {
    if (!confirm('Are you sure you want to delete this seat?')) {
      return;
    }

    this.seatService.deleteSeat(id).subscribe({
      next: () => {
        this.successMessage = 'Seat deleted successfully.';
        this.loadSeats();
      },
      error: () => {
        this.errorMessage = 'Unable to delete seat.';
      }
    });
  }
}
