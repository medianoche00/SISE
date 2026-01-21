import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { UsuarioAsignado } from '../models/usuario-asignado.model';

@Injectable({
  providedIn: 'root',
})
export class ExpedienteService {
  private apiUrl = `${environment.apiUrl}/Expediente`;

  constructor(private http: HttpClient) {}

  getUsuariosPorPersona(idPersona: number): Observable<UsuarioAsignado[]> {
    return this.http.get<UsuarioAsignado[]>(`${this.apiUrl}/${idPersona}`);
  }
}
