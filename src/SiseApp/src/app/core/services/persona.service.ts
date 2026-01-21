import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Persona } from '../models/persona.model';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class PersonaService {
  private apiUrl = `${environment.apiUrl}/Persona`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Persona[]> {
    return this.http.get<Persona[]>(this.apiUrl);
  }

  create(persona: Persona, docRespaldo: string): Observable<Persona> {
    const payload = {
      ...persona,
      DocumentoRespaldo: docRespaldo,
    };

    return this.http.post<Persona>(this.apiUrl, payload);
  }

  update(persona: Persona, docRespaldo: string): Observable<Persona> {
    const payload = {
      ...persona,
      DocumentoRespaldo: docRespaldo,
    };

    return this.http.put<Persona>(
      `${this.apiUrl}/${persona.idPersona}`,
      payload,
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
