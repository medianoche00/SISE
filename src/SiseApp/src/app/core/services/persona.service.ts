import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Persona } from '../models/persona.interface'; // Importación corregida
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PersonaService {
  private apiUrl = `${environment.apiUrl}/personas`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Persona[]> {
    return this.http.get<Persona[]>(this.apiUrl);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
