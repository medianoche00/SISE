import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import {
  Representante,
  RepresentanteCrearDto,
  RepresentanteActualizarDto,
} from '../models/representante.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RepresentanteService {
  private apiUrl = `${environment.apiUrl}/Representante`;

  constructor(private http: HttpClient) {}

  getPorId(idRepresentante: number): Observable<Representante> {
    return this.http.get<Representante>(`${this.apiUrl}/${idRepresentante}`);
  }

  registrar(dto: RepresentanteCrearDto): Observable<void> {
    return this.http.post<void>(this.apiUrl, dto);
  }

  actualizar(dto: RepresentanteActualizarDto): Observable<void> {
    return this.http.put<void>(this.apiUrl, dto);
  }

  eliminar(idRepresentante: number, documentoRespaldo: string): Observable<void> {
    let params = new HttpParams().set('DocumentoRespaldo', documentoRespaldo);

    return this.http.delete<void>(`${this.apiUrl}/${idRepresentante}`, {
      params: params,
    });
  }
}
