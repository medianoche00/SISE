import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs'; // of es para el mock data temporal
import { environment } from '../../../environments/environment.development';
import { OfertaLaboral } from '../models/oferta.model';

@Injectable({
  providedIn: 'root'
})
export class OfertaService {
  // Ajusta tu URL base
  private apiUrl = `${environment.apiUrl}/OfertaLaboral/`; 

  constructor(private http: HttpClient) { }

  getOfertasActivas(): Observable<OfertaLaboral[]> {
    return this.http.get<OfertaLaboral[]>(this.apiUrl + 'activas');
  }

  postularOferta(idOferta: number): Observable<any> {
    // Aquí iría tu lógica de postulación
    return this.http.post(`${this.apiUrl}/postular/${idOferta}`, {});
  }
}