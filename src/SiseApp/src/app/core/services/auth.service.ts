import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment.development';

interface LoginRequest {
  userNameOrEmail: string;
  password: string;
}

interface AuthResponse {
  token: string;
  tokenType: string;
  expiresAt: string;
  userName?: string;
  userId?: number;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  private tokenKey = 'sise_token';
  private authSubject = new BehaviorSubject<AuthResponse | null>(this.getStoredAuth());

  auth$ = this.authSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  login(userNameOrEmail: string, password: string): Observable<AuthResponse> {
    const body: LoginRequest = { userNameOrEmail, password };
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, body).pipe(
      tap(res => {
        this.setStoredAuth(res);
        this.authSubject.next(res);
      })
    );
  }

  logout() {
    this.clearStoredAuth();
    this.authSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    const auth = this.getStoredAuth();
    return auth?.token ?? null;
  }

  isLoggedIn(): boolean {
    const auth = this.getStoredAuth();
    if (!auth) return false;
    const expires = new Date(auth.expiresAt);
    return expires > new Date();
  }

  private setStoredAuth(res: AuthResponse) {
    localStorage.setItem(this.tokenKey, JSON.stringify(res));
  }

  private getStoredAuth(): AuthResponse | null {
    const v = localStorage.getItem(this.tokenKey);
    if (!v) return null;
    try {
      return JSON.parse(v) as AuthResponse;
    } catch {
      return null;
    }
  }

  private clearStoredAuth() {
    localStorage.removeItem(this.tokenKey);
  }
  
  getRole(): string {
    const auth = this.getStoredAuth();
    if (!auth) return '';
    return auth.role? auth.role : '';
  }

  getUsername(): string {
    const auth = this.getStoredAuth();
    if (!auth) return '';
    return auth.userName? auth.userName : '';
  }
}