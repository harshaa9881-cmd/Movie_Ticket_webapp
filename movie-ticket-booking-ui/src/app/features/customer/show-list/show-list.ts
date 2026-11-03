import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { ShowService } from '../../../core/services/show.service';
import { Show } from '../../../shared/models/show.model';

@Component({
  selector: 'app-show-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './show-list.html',
  styleUrl: './show-list.scss'
})
export class ShowList implements OnInit {

  shows: Show[] = [];
  theatreId = 0;

  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private showService: ShowService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.theatreId = Number(
      this.route.snapshot.paramMap.get('theatreId')
    );

    if (!this.theatreId) {
      this.errorMessage = 'Invalid theatre ID.';
      return;
    }

    this.loadShows();
  }

  loadShows(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.showService.getShowsByTheatre(this.theatreId).subscribe({
      next: (response) => {
        this.shows = response;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Show API Error:', error);
        this.errorMessage = 'Unable to load shows. Please try again.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
