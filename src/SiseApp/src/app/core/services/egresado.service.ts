import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Carrera } from '../models/egresado.model';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class EgresadoService {

  private apiUrl = `${environment.apiUrl}/Egresados`;

  constructor(private http: HttpClient) { }

  obtenerCarreras(): Observable<Carrera[]> {
    return this.http.get<Carrera[]>(`${this.apiUrl}/carreras`);
  }

  actualizarPerfil(datos: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/actualizar-perfil`, datos);
  }

  obtenerMiPerfil() {
  return this.http.get<any>(`${this.apiUrl}/mi-perfil-egresado`);
}
}
