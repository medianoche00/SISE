import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import {
  Carrera,
  CargoAdministrativo,
  Empresa,
  Rol,
} from '../models/catalogos.model';

@Injectable({
  providedIn: 'root',
})
export class CatalogosService {
  private apiUrl = `${environment.apiUrl}/Catalogos`;

  constructor(private http: HttpClient) {}

  getCarreras(): Observable<Carrera[]> {
    return this.http.get<Carrera[]>(`${this.apiUrl}/Carreras`);
  }

  getCargosAdministrativos(): Observable<CargoAdministrativo[]> {
    return this.http.get<CargoAdministrativo[]>(
      `${this.apiUrl}/CargosAdministrativos`,
    );
  }

  getRoles(): Observable<Rol[]> {
    return this.http.get<Rol[]>(`${this.apiUrl}/Roles`);
  }

  getEmpresas(): Observable<Empresa[]> {
    return this.http.get<Empresa[]>(`${this.apiUrl}/Empresas`);
  }
}
