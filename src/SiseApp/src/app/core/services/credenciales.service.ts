import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CredencialesService {
  private apiUrl = `${environment.apiUrl}/Credenciales`;

  constructor(private http: HttpClient) {}

  actualizarCredenciales(idUsuario: number, username: string, password: string, documentoRespaldo: string): Observable<any> {
    const url = `${this.apiUrl}`;
    const body = {
      idUsuario,
      username,
      password,
      documentoRespaldo
    };
    return this.http.put(url, body);
  }
}
