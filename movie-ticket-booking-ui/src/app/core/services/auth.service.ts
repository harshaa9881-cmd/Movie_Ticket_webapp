import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl + '/Auth';
  private tokenKey = 'movieTicketToken';

  private roleClaim =
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

  constructor(private http: HttpClient) { }

  register(request: {
    fullName: string;
    email: string;
    password: string;
  }): Observable<any> {

    return this.http.post(
      `${this.apiUrl}/register`,
      request
    );

  }

  login(request: {
    email: string;
    password: string;
  }): Observable<any> {

    return this.http.post<any>(
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

  getRole(): string | null {

    const token = this.getToken();

    if (!token) {
      return null;
    }

    try {

      const payload = token.split('.')[1];

      const decodedPayload = atob(
        payload.replace(/-/g, '+').replace(/_/g, '/')
      );

      const tokenData = JSON.parse(decodedPayload);

      return tokenData[this.roleClaim] || null;

    }
    catch {
      return null;
    }

  }

  isLoggedIn(): boolean {

    return this.getToken() !== null;

  }

  isAdmin(): boolean {

    return this.getRole() === 'Admin';

  }

  isCustomer(): boolean {

    return this.getRole() === 'Customer';

  }

  logout(): void {

    localStorage.removeItem(this.tokenKey);

  }

}
