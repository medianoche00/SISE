// Asegúrate de importar HttpClient
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';

export interface ExperienciaLaboral {
  idExperiencia?: number;
  empresa: string;
  cargo: string;
  fechaInicio: Date;
  fechaFin?: Date;
  actualmente: boolean;
  descripcion?: string;
}

@Injectable({ providedIn: 'root' })
export class ExperienciasService {
  private apiUrl = `${environment.apiUrl}/ExperienciaLaboral`;

  constructor(private http: HttpClient) {}

  obtenerExperiencias(): Observable<ExperienciaLaboral[]> {
    return this.http.get<ExperienciaLaboral[]>(`${this.apiUrl}`);
  }

  crearExperiencia(data: ExperienciaLaboral): Observable<any> {
    return this.http.post(`${this.apiUrl}`, data);
  }

  editarExperiencia(id: number, data: ExperienciaLaboral): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  eliminarExperiencia(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}