import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ShowService } from '../../../core/services/show.service';
import { MovieService } from '../../../core/services/movie.service';
import { ScreenService } from '../../../core/services/screen.service';
import { Show, CreateShowRequest } from '../../../shared/models/show.model';
import { Movie } from '../../../shared/models/movie.model';
import { Screen } from '../../../shared/models/screen.model';

@Component({
  selector: 'app-show-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './show-management.html',
  styleUrl: './show-management.scss'
})
export class ShowManagement implements OnInit {

  shows: Show[] = [];
  movies: Movie[] = [];
  screens: Screen[] = [];

  showForm: CreateShowRequest = {
    movieId: 0,
    screenId: 0,
    startTime: '',
    endTime: '',
    ticketPrice: 0
  };

  editingShowId: number | null = null;
  message = '';
  errorMessage = '';

  constructor(
    private showService: ShowService,
    private movieService: MovieService,
    private screenService: ScreenService
  ) { }

  ngOnInit(): void {
    this.loadShows();
    this.loadMovies();
    this.loadScreens();
  }

  loadShows(): void {
    this.showService.getShows().subscribe({
      next: (response: any) => {
        this.shows = Array.isArray(response) ? response : response.items || [];
      },
      error: () => {
        this.errorMessage = 'Unable to load shows.';
      }
    });
  }

  loadMovies(): void {
    this.movieService.getMovies(1, 100).subscribe({
      next: (response: any) => {
        this.movies = response.items || [];
      },
      error: () => {
        this.errorMessage = 'Unable to load movies.';
      }
    });
  }

  loadScreens(): void {
    this.screenService.getScreens().subscribe({
      next: (response: any) => {
        this.screens = Array.isArray(response) ? response : response.items || [];
      },
      error: () => {
        this.errorMessage = 'Unable to load screens.';
      }
    });
  }

  saveShow(): void {
    this.message = '';
    this.errorMessage = '';

    if (
      !this.showForm.movieId ||
      !this.showForm.screenId ||
      !this.showForm.startTime ||
      !this.showForm.endTime ||
      this.showForm.ticketPrice <= 0
    ) {
      this.errorMessage = 'Please enter all show details correctly.';
      return;
    }

    if (this.editingShowId) {
      this.showService.updateShow(this.editingShowId, this.showForm).subscribe({
        next: () => {
          this.message = 'Show updated successfully.';
          this.resetForm();
          this.loadShows();
        },
        error: () => {
          this.errorMessage = 'Unable to update show.';
        }
      });
    } else {
      this.showService.addShow(this.showForm).subscribe({
        next: () => {
          this.message = 'Show added successfully.';
          this.resetForm();
          this.loadShows();
        },
        error: () => {
          this.errorMessage = 'Unable to add show.';
        }
      });
    }
  }

  editShow(show: Show): void {
    this.editingShowId = show.showId;

    this.showForm = {
      movieId: show.movieId,
      screenId: show.screenId,
      startTime: this.toDateTimeLocal(show.startTime),
      endTime: this.toDateTimeLocal(show.endTime),
      ticketPrice: show.ticketPrice
    };

    this.message = '';
    this.errorMessage = '';
  }

  deleteShow(id: number): void {
    if (!confirm('Are you sure you want to delete this show?')) {
      return;
    }

    this.showService.deleteShow(id).subscribe({
      next: () => {
        this.message = 'Show deleted successfully.';
        this.loadShows();
      },
      error: () => {
        this.errorMessage = 'Unable to delete show. It may have associated bookings.';
      }
    });
  }

  resetForm(): void {
    this.editingShowId = null;

    this.showForm = {
      movieId: 0,
      screenId: 0,
      startTime: '',
      endTime: '',
      ticketPrice: 0
    };
  }

  private toDateTimeLocal(value: string): string {
    const date = new Date(value);
    const offset = date.getTimezoneOffset();
    return new Date(date.getTime() - offset * 60000)
      .toISOString()
      .slice(0, 16);
  }
}
