import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { MovieService } from '../../../core/services/movie.service';
import { Movie } from '../../../shared/models/movie.model';

@Component({
  selector: 'app-movie-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './movie-list.html',
  styleUrl: './movie-list.scss'
})
export class MovieList implements OnInit {

  movies: Movie[] = [];
  searchTitle = '';

  isLoading = false;
  errorMessage = '';

  constructor(private movieService: MovieService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    
    this.loadMovies();
  }

  loadMovies(): void {

    this.isLoading = true;
    this.errorMessage = '';

    

    this.movieService.getMovies().subscribe({

      next: (response) => {
       
        this.movies = response.items;
        this.isLoading = false;

        this.cdr.detectChanges();

      },

      error: (error) => {

        console.error('Movie API Error:', error);

        this.errorMessage = 'Unable to load movies. Please try again.';

        this.isLoading = false;
      }

    });

  }

  searchMovies(): void {

    if (this.searchTitle.trim() === '') {
      this.loadMovies();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.movieService.searchMovies(this.searchTitle).subscribe({
      next: (response) => {
        this.movies = response;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to search movies. Please try again.';
        this.isLoading = false;
      }
    });
  }

  clearSearch(): void {
    this.searchTitle = '';
    this.loadMovies();
  }
}
