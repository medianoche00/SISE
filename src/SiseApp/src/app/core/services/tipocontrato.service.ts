import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Injectable } from '@angular/core';

export interface TipoContrato {
  idTipoContrato: number;
  nombreTipo: string;
}

@Injectable({
  providedIn: 'root',
})

export class TipoContratoService {
  private apiUrl = `${environment.apiUrl}/TipoContrato/`;
  constructor(private http: HttpClient) {}

  getTiposContrato(): Observable<TipoContrato[]> {
    return this.http.get<TipoContrato[]>(`${this.apiUrl}`);
  }
}
