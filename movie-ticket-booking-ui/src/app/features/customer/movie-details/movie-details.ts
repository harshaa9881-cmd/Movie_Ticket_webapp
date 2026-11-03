import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { MovieService } from '../../../core/services/movie.service';
import { Movie } from '../../../shared/models/movie.model';

@Component({
  selector: 'app-movie-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './movie-details.html',
  styleUrl: './movie-details.scss'
})
export class MovieDetails implements OnInit {

  movie: Movie | null = null;

  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage = 'Invalid movie ID.';
      return;
    }

    this.loadMovie(id);
  }

  loadMovie(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.movieService.getMovieById(id).subscribe({
      next: (response) => {
        this.movie = response;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Movie Details API Error:', error);
        this.errorMessage = 'Unable to load movie details.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
