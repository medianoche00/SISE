import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import {
  Persona,
  PersonaCrearDto,
  PersonaActualizarDto,
} from '../models/persona.model';

@Injectable({
  providedIn: 'root',
})
export class PersonaService {
  private apiUrl = `${environment.apiUrl}/Persona`;

  constructor(private http: HttpClient) {}

  // GET: Obtener todas las personas
  getAll(): Observable<Persona[]> {
    return this.http.get<Persona[]>(this.apiUrl);
  }

  // GET: Obtener por ID (si lo implementaste en el backend)
  getById(id: number): Observable<Persona> {
    return this.http.get<Persona>(`${this.apiUrl}/${id}`);
  }

  // POST: Crear persona
  create(dto: PersonaCrearDto): Observable<Persona> {
    return this.http.post<Persona>(this.apiUrl, dto);
  }

  // PUT: Actualizar persona
  update(dto: PersonaActualizarDto): Observable<void> {
    return this.http.put<void>(this.apiUrl, dto);
  }

  // DELETE: Eliminar persona
  // El documento de respaldo viaja como Query Parameter (?documentoRespaldo=xyz)
  delete(id: number, documentoRespaldo: string): Observable<void> {
    let params = new HttpParams().set('documentoRespaldo', documentoRespaldo);

    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      params: params,
    });
  }
}
