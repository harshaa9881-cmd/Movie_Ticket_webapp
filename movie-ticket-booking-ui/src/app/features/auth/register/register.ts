import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {

  fullName = '';
  email = '';
  password = '';

  errorMessage = '';
  successMessage = '';

  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onRegister(): void {

    this.errorMessage = '';
    this.successMessage = '';
    this.isLoading = true;

    this.authService.register({
      fullName: this.fullName,
      email: this.email,
      password: this.password
    }).subscribe({

      next: (response) => {

        this.successMessage = response.message;
        this.isLoading = false;

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1500);

      },

      error: (error) => {

        this.errorMessage =
          error.error?.message ||
          error.error?.title ||
          'Registration failed. Please try again.';

        this.isLoading = false;

      }

    });

  }

}
