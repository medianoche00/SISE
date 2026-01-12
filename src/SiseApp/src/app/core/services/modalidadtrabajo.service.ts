import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Injectable } from '@angular/core';

export interface ModalidadTrabajo {
  idModalidad: number;
  nombreModalidad: string;
}

@Injectable({
  providedIn: 'root',
})

export class ModalidadTrabajoService {
  private apiUrl = `${environment.apiUrl}/ModalidadTrabajo/`;
  constructor(private http: HttpClient) {}

  getModalidades(): Observable<ModalidadTrabajo[]> {
    return this.http.get<ModalidadTrabajo[]>(`${this.apiUrl}`);
  }
}
