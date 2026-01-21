import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { DepartamentoDto } from '../models/ubicacion.model';

@Injectable({
  providedIn: 'root',
})
export class UbicacionService {
  private apiUrl = `${environment.apiUrl}/Ubicacion`;

  constructor(private http: HttpClient) {}

  getUbicacionesCompleta(): Observable<DepartamentoDto[]> {
    return this.http.get<DepartamentoDto[]>(this.apiUrl);
  }
}
