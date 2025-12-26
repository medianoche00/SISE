import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegistroEgresadoRequest, Carrera } from '../models/egresado.model';

@Injectable({
  providedIn: 'root'
})
export class EgresadoService {

  private apiUrl = 'https://localhost:7000/api/egresados';

  constructor(private http: HttpClient) { }

  obtenerCarreras(): Observable<Carrera[]> {
    return this.http.get<Carrera[]>(`${this.apiUrl}/carreras`);
  }

  completarPerfil(datos: RegistroEgresadoRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/completar-perfil`, datos);
  }
}
