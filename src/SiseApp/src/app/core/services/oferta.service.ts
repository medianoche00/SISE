import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs'; // of es para el mock data temporal
import { environment } from '../../../environments/environment.development';
import { OfertaLaboral } from '../models/oferta.model';

@Injectable({
  providedIn: 'root',
})
export class OfertaService {
  // Ajusta tu URL base
  private apiUrl = `${environment.apiUrl}/OfertaLaboral/`;

  constructor(private http: HttpClient) {}

  getOfertasActivas(): Observable<OfertaLaboral[]> {
    return this.http.get<OfertaLaboral[]>(this.apiUrl + 'activas');
  }

  postularOferta(idOferta: number, idUsuario: number): Observable<any> {
    const body = {
      IdOferta: idOferta,
      IdUsuario: idUsuario,
    };
    return this.http.post(`${this.apiUrl}/postular`, body);
  }

  getMisPostulaciones(idUsuario: number): Observable<OfertaLaboral[]> {
    return this.http.get<OfertaLaboral[]>(
      `${this.apiUrl}/mis-postulaciones/${idUsuario}`
    );
  }

  cancelarPostulacion(idOferta: number, idUsuario: number): Observable<any> {
    return this.http.delete(
      `${this.apiUrl}/cancelar-postulacion/${idOferta}/${idUsuario}`
    );
  }
}
