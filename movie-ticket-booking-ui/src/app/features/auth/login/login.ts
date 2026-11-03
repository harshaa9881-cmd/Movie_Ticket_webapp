import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {

  email = '';
  password = '';

  errorMessage = '';
  successMessage = '';

  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onLogin(): void {

    this.errorMessage = '';
    this.successMessage = '';
    this.isLoading = true;

    this.authService.login({
      email: this.email,
      password: this.password
    }).subscribe({

      next: (response) => {

        this.successMessage = response.message;

        this.isLoading = false;

        if (this.authService.isAdmin()) {
          this.router.navigate(['/admin']);
        }
        else if (this.authService.isCustomer()) {
          this.router.navigate(['/customer']);
        }
        else {
          this.errorMessage = 'Unable to identify your user role.';
        }

      },

      error: (error) => {

        this.errorMessage =
          error.error?.message ||
          error.error?.title ||
          'Login failed. Please check your credentials.';

        this.isLoading = false;

      }

    });

  }

}
