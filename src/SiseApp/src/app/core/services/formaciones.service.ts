// Asegúrate de importar HttpClient
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';

export interface FormacionComplementaria {
  idFormacion?: number,
  idTipoFormacion?: number,
  tipoFormacion: string,
  nombreDelCurso: string,
  institucion: string,
  fechaInicio: Date,
  fechaFin: Date,
  estado: boolean,
}

@Injectable({ providedIn: 'root' })
export class FormacionComplementariasService {
  private apiUrl = `${environment.apiUrl}/FormacionComplementaria/`;

  constructor(private http: HttpClient) {}

  obtenerFormaciones(): Observable<FormacionComplementaria[]> {
    return this.http.get<FormacionComplementaria[]>(`${this.apiUrl}`);
  }

  crearFormacion(data: FormacionComplementaria): Observable<any> {
    return this.http.post(`${this.apiUrl}`, data);
  }

  editarFormacion(id: number, data: FormacionComplementaria): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  eliminarFormacion(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
