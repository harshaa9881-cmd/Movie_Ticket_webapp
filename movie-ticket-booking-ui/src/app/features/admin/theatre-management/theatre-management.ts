import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TheatreService } from '../../../core/services/theatre.service';
import { Theatre } from '../../../shared/models/theatre.model';

@Component({
  selector: 'app-theatre-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './theatre-management.html',
  styleUrl: './theatre-management.scss'
})
export class TheatreManagement implements OnInit {

  theatres: Theatre[] = [];

  theatreForm: Partial<Theatre> = {
    name: '',
    location: ''
  };

  isEditing = false;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private theatreService: TheatreService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.loadTheatres();
  }

  loadTheatres(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.theatreService.getTheatres().subscribe({
      next: (response) => {
        this.theatres = response;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Theatre API Error:', error);
        this.errorMessage = 'Unable to load theatres.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  saveTheatre(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.theatreForm.name?.trim() ||
      !this.theatreForm.location?.trim()) {
      this.errorMessage = 'Please enter the theatre name and location.';
      return;
    }

    if (this.isEditing && this.theatreForm.theatreId) {
      this.theatreService.updateTheatre(
        this.theatreForm.theatreId,
        this.theatreForm
      ).subscribe({
        next: () => {
          this.successMessage = 'Theatre updated successfully.';
          this.resetForm();
          this.loadTheatres();
        },
        error: () => {
          this.errorMessage = 'Unable to update theatre.';
        }
      });
    } else {
      this.theatreService.addTheatre(this.theatreForm).subscribe({
        next: () => {
          this.successMessage = 'Theatre added successfully.';
          this.resetForm();
          this.loadTheatres();
        },
        error: () => {
          this.errorMessage = 'Unable to add theatre.';
        }
      });
    }
  }

  editTheatre(theatre: Theatre): void {
    this.theatreForm = { ...theatre };
    this.isEditing = true;
    this.errorMessage = '';
    this.successMessage = '';
  }

  deleteTheatre(id: number): void {
    if (!confirm('Are you sure you want to delete this theatre?')) {
      return;
    }

    this.theatreService.deleteTheatre(id).subscribe({
      next: () => {
        this.successMessage = 'Theatre deleted successfully.';
        this.loadTheatres();
      },
      error: () => {
        this.errorMessage = 'Unable to delete theatre.';
      }
    });
  }

  resetForm(): void {
    this.theatreForm = { name: '', location: '' };
    this.isEditing = false;
  }
}
