import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import {
  Administrativo,
  AdministrativoCrearDto,
  AdministrativoActualizarDto,
} from '../models/administrativo.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AdministrativoService {
  private apiUrl = `${environment.apiUrl}/Administrativo`;

  constructor(private http: HttpClient) {}

  getPorId(id: number): Observable<Administrativo> {
    return this.http.get<Administrativo>(`${this.apiUrl}/${id}`);
  }

  registrarAdministrativo(dto: AdministrativoCrearDto): Observable<void> {
    return this.http.post<void>(this.apiUrl, dto);
  }

  registrarAdministrador(dto: AdministrativoCrearDto): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/Administrador`, dto);
  }

  actualizar(dto: AdministrativoActualizarDto): Observable<void> {
    return this.http.put<void>(this.apiUrl, dto);
  }

  eliminar(id: number, documentoRespaldo: string): Observable<void> {
    let params = new HttpParams().set('DocumentoRespaldo', documentoRespaldo);
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { params: params });
  }
}
