import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Postulacion } from '../models/postular.model';

@Injectable({
  providedIn: 'root',
})
export class PostularService {
  private apiUrl = `${environment.apiUrl}/Postulaciones/`;
  constructor(private http: HttpClient) {}

  postularOferta(idOferta: number, cartaPresentacion: string): Observable<any> {
    const body = {
        IdOferta: idOferta,
        CartaPresentacion: cartaPresentacion
    };
    return this.http.post(`${this.apiUrl}postular`, body);
  }

  cancelarPostulacion(idOferta: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}cancelar/${idOferta}`);
    }
    
  misPostulaciones(): Observable<Postulacion[]> {
    return this.http.get<Postulacion[]>(`${this.apiUrl}mis-postulaciones`);
  }
}
