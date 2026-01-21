import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import {
  EgresadoCrearDto,
  AdministrativoCrearDto,
  RepresentanteCrearDto,
} from '../models/rol.model';

@Injectable({
  providedIn: 'root',
})
export class RolService {
  private apiUrl = `${environment.apiUrl}/Rol`;

  constructor(private http: HttpClient) {}

  registrarEgresado(dto: EgresadoCrearDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/RegistrarEgresado`, dto);
  }

  registrarRepresentante(dto: RepresentanteCrearDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/RegistrarRepresentante`, dto);
  }

  registrarAdministrativo(dto: AdministrativoCrearDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/RegistrarAdministrativo`, dto);
  }

  registrarAdministrador(dto: AdministrativoCrearDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/RegistrarAdministrador`, dto);
  }
}
