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

  getOfertasActivas(): Observable<OfertaLaboral[]> {
    return this.http.get<OfertaLaboral[]>(this.apiUrl + 'activas');
  }

  postularOferta(idOferta: number): Observable<any> {
    const body = {
      IdOferta: idOferta,
    };
    return this.http.post(`${this.apiUrl}/postular`, body);
  }

  getMisPostulaciones(): Observable<OfertaLaboral[]> {
    return this.http.get<OfertaLaboral[]>(
      `${this.apiUrl}/mis-postulaciones`
    );
  }

  cancelarPostulacion(idOferta: number): Observable<any> {
    return this.http.delete(
      `${this.apiUrl}/cancelar-postulacion/${idOferta}`
    );
  }
}
