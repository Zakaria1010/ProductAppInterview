import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/api/auth/login`;
  private jwtToken: string | null = null

  constructor(private http: HttpClient) {}

  login(username: string, password: string, credentials: { username: string; password: string; }): Observable<any> {
    return this.http.post<any>(this.apiUrl, credentials).pipe(
      tap(response => {
        if (response.token) {
          this.jwtToken = response.token;
          localStorage.setItem('token', response.token)
          localStorage.setItem('role', response.role);
        }
      })
    );
  }

  logout(): void {
    this.jwtToken = null;
    localStorage.removeItem('token')
    localStorage.removeItem('role')
  }

  getToken(): string | null {
    if (!this.jwtToken) {
      this.jwtToken = localStorage.getItem('token')
    }
    return this.jwtToken;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token;
  }
}