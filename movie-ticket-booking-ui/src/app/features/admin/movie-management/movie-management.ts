import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MovieService } from '../../../core/services/movie.service';
import { Movie } from '../../../shared/models/movie.model';

@Component({
  selector: 'app-movie-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './movie-management.html',
  styleUrl: './movie-management.scss'
})
export class MovieManagement implements OnInit {

  movies: Movie[] = [];

  movieForm: Partial<Movie> = this.getEmptyMovie();

  isEditing = false;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private movieService: MovieService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.loadMovies();
  }

  getEmptyMovie(): Partial<Movie> {
    return {
      title: '',
      description: '',
      durationMinutes: 0,
      releaseDate: '',
      language: '',
      genre: ''
    };
  }

  loadMovies(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.movieService.getMovies(1, 100).subscribe({
      next: (response) => {
        this.movies = response.items;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Unable to load movies.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  saveMovie(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (
      !this.movieForm.title?.trim() ||
      !this.movieForm.language?.trim() ||
      !this.movieForm.genre?.trim() ||
      !this.movieForm.durationMinutes ||
      !this.movieForm.releaseDate
    ) {
      this.errorMessage = 'Please fill in all required movie details.';
      return;
    }

    if (this.isEditing && this.movieForm.movieId) {
      this.movieService.updateMovie(
        this.movieForm.movieId,
        this.movieForm
      ).subscribe({
        next: () => {
          this.successMessage = 'Movie added successfully.';
          this.resetForm();
          this.loadMovies();
          this.cdr.detectChanges();
        },
        error: () => {
          this.errorMessage = 'Unable to add movie.';
          this.cdr.detectChanges();
        }
      });
    } else {
      this.movieService.addMovie(this.movieForm).subscribe({
        next: () => {
          this.successMessage = 'Movie added successfully.';
          this.resetForm();
          this.loadMovies();
          this.cdr.detectChanges();
        },
        error: () => {
          this.errorMessage = 'Unable to add movie.';
          this.cdr.detectChanges();
        }
      });
    }
  }

  editMovie(movie: Movie): void {
    this.movieForm = { ...movie };
    this.isEditing = true;
    this.errorMessage = '';
    this.successMessage = '';
  }

  deleteMovie(id: number): void {
    if (!confirm('Are you sure you want to delete this movie?')) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.movieService.deleteMovie(id).subscribe({
      next: () => {
        this.successMessage = 'Movie deleted successfully.';
        this.loadMovies();
      },
      error: () => {
        this.errorMessage = 'Unable to delete movie.';
      }
    });
  }

  resetForm(): void {
    this.movieForm = this.getEmptyMovie();
    this.isEditing = false;
  }
}
