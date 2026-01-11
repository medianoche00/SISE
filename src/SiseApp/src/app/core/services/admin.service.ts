import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AdminService {
  private apiUrl = `${environment.apiUrl}/AdminUsuarios`;
  constructor(private http: HttpClient) { }

  getUsuarios(): Observable<any[]> { return this.http.get<any[]>(this.apiUrl); }
  crearUsuario(data: any): Observable<any> { return this.http.post(this.apiUrl, data); }
  cambiarEstado(id: number): Observable<any> { return this.http.put(`${this.apiUrl}/estado/${id}`, {}); }
}
