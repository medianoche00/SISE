import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Injectable } from '@angular/core';

export interface TipoFormacion {
  idTipoFormacion: number;
  nombreTipoFormacion: string;
}

@Injectable({
  providedIn: 'root',
})

export class TipoFormacionService {
  private apiUrl = `${environment.apiUrl}/TipoFormacion/`;
  constructor(private http: HttpClient) {}

  getTipoFormaciones(): Observable<TipoFormacion[]> {
    return this.http.get<TipoFormacion[]>(`${this.apiUrl}`);
  }
}
