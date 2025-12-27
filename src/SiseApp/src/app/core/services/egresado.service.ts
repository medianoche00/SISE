import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Carrera } from '../models/egresado.model';

@Injectable({
  providedIn: 'root'
})
export class EgresadoService {

  private apiUrl = 'http://localhost:5000/api/egresados';

  constructor(private http: HttpClient) { }

  obtenerCarreras(): Observable<Carrera[]> {
    return this.http.get<Carrera[]>(`${this.apiUrl}/carreras`);
  }

  actualizarPerfil(datos: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/actualizar-perfil`, datos);
  }
}
