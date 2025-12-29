import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { OfertaLaboral } from '../models/oferta.model';

@Injectable({
  providedIn: 'root',
})
export class OfertaService {
  private apiUrl = `${environment.apiUrl}/OfertaLaboral/`;

  constructor(private http: HttpClient) {}

  getOfertasDisponibles(): Observable<OfertaLaboral[]> {
    return this.http.get<OfertaLaboral[]>(`${this.apiUrl}disponibles`);
  }
}
