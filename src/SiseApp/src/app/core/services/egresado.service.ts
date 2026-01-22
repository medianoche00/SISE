import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { Egresado, EgresadoActualizarDto, EgresadoCrearDto } from '../models/egresado.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EgresadoService {
  private apiUrl = `${environment.apiUrl}/Egresado`;

  constructor(private http: HttpClient) {}

  getPorId(idEgresado: number): Observable<Egresado> {
    return this.http.get<Egresado>(`${this.apiUrl}/${idEgresado}`);
  }

  registrar(dto: EgresadoCrearDto): Observable<void> {
    return this.http.post<void>(this.apiUrl, dto);
  }

  actualizar(dto: EgresadoActualizarDto): Observable<void> {
    return this.http.put<void>(this.apiUrl, dto);
  }

  eliminar(idEgresado: number, documentoRespaldo: string): Observable<void> {
    let params = new HttpParams().set('DocumentoRespaldo', documentoRespaldo);

    return this.http.delete<void>(`${this.apiUrl}/${idEgresado}`, {
      params: params,
    });
  }
}
