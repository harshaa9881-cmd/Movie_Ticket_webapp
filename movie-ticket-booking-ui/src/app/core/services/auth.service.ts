import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  message: string;
  token?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl + '/Auth';

  private tokenKey = 'movieTicketToken';

  constructor(private http: HttpClient) { }

  register(request: RegisterRequest): Observable<AuthResponse> {

    return this.http.post<AuthResponse>(
      `${this.apiUrl}/register`,
      request
    );

  }

  login(request: LoginRequest): Observable<AuthResponse> {

    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      request
    ).pipe(

      tap(response => {

        if (response.token) {
          localStorage.setItem(this.tokenKey, response.token);
        }

      })

    );

  }

  getToken(): string | null {

    return localStorage.getItem(this.tokenKey);

  }

  logout(): void {

    localStorage.removeItem(this.tokenKey);

  }

}
