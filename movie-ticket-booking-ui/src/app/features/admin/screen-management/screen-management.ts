import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ScreenService } from '../../../core/services/screen.service';
import { TheatreService } from '../../../core/services/theatre.service';
import { Screen } from '../../../shared/models/screen.model';
import { Theatre } from '../../../shared/models/theatre.model';

@Component({
  selector: 'app-screen-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './screen-management.html',
  styleUrl: './screen-management.scss'
})
export class ScreenManagement implements OnInit {

  screens: Screen[] = [];
  theatres: Theatre[] = [];

  screenForm: Partial<Screen> = {
    theatreId: 0,
    screenName: ''
  };

  selectedTheatreId = 0;
  isEditing = false;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private screenService: ScreenService,
    private theatreService: TheatreService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadTheatres();
    this.loadScreens();
  }

  loadTheatres(): void {
    this.theatreService.getTheatres().subscribe({
      next: (response) => {
        this.theatres = response;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Theatre API Error:', error);
        this.errorMessage = 'Unable to load theatres.';
        this.cdr.detectChanges();
      }
    });
  }

  loadScreens(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const theatreId = this.selectedTheatreId || undefined;

    this.screenService.getScreens(theatreId).subscribe({
      next: (response) => {
        this.screens = response;
        this.isLoading = false;
        this.cdr.detectChanges();

        console.log('Screens loaded:', response);
      },
      error: (error) => {
        console.error('Screen API Error:', error);
        this.errorMessage = 'Unable to load screens.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  saveScreen(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.screenForm.theatreId || !this.screenForm.screenName?.trim()) {
      this.errorMessage = 'Please select a theatre and enter a screen name.';
      return;
    }

    if (this.isEditing && this.screenForm.screenId) {
      this.screenService.updateScreen(
        this.screenForm.screenId,
        this.screenForm
      ).subscribe({
        next: () => {
          this.successMessage = 'Screen updated successfully.';
          this.resetForm();
          this.loadScreens();
        },
        error: () => {
          this.errorMessage = 'Unable to update screen.';
        }
      });
    } else {
      this.screenService.addScreen(this.screenForm).subscribe({
        next: () => {
          this.successMessage = 'Screen added successfully.';
          this.resetForm();
          this.loadScreens();
        },
        error: () => {
          this.errorMessage = 'Unable to add screen.';
        }
      });
    }
  }

  editScreen(screen: Screen): void {
    this.screenForm = { ...screen };
    this.isEditing = true;
    this.errorMessage = '';
    this.successMessage = '';
  }

  deleteScreen(id: number): void {
    if (!confirm('Are you sure you want to delete this screen?')) {
      return;
    }

    this.screenService.deleteScreen(id).subscribe({
      next: () => {
        this.successMessage = 'Screen deleted successfully.';
        this.loadScreens();
      },
      error: () => {
        this.errorMessage = 'Unable to delete screen.';
      }
    });
  }

  resetForm(): void {
    this.screenForm = {
      theatreId: this.selectedTheatreId || 0,
      screenName: ''
    };

    this.isEditing = false;
  }
}
