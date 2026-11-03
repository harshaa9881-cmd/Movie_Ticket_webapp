import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { TheatreService } from '../../../core/services/theatre.service';
import { Theatre } from '../../../shared/models/theatre.model';

@Component({
  selector: 'app-theatre-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './theatre-list.html',
  styleUrl: './theatre-list.scss'
})
export class TheatreList implements OnInit {

  theatres: Theatre[] = [];
  isLoading = false;
  errorMessage = '';

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
        this.errorMessage = 'Unable to load theatres. Please try again.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
