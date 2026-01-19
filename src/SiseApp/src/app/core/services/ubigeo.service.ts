import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

// Interfaces para el tipado (puedes moverlas a un archivo de models)
export interface Departamento { id: number; nombre: string; }
export interface Provincia { id: number; departamentoId: number; nombre: string; }
export interface Distrito { id: number; provinciaId: number; nombre: string; }

@Injectable({
  providedIn: 'root'
})
export class UbigeoService {

  // DATOS SIMULADOS (ESTO VENDRA DE TU BD)
  private departamentos: Departamento[] = [
    { id: 1, nombre: 'Lima' },
    { id: 2, nombre: 'Arequipa' },
    { id: 3, nombre: 'Cusco' }
  ];

  private provincias: Provincia[] = [
    { id: 101, departamentoId: 1, nombre: 'Lima' },
    { id: 102, departamentoId: 1, nombre: 'Cañete' },
    { id: 201, departamentoId: 2, nombre: 'Arequipa' },
    { id: 202, departamentoId: 2, nombre: 'Caylloma' },
    { id: 301, departamentoId: 3, nombre: 'Cusco' }
  ];

  private distritos: Distrito[] = [
    { id: 1001, provinciaId: 101, nombre: 'Miraflores' },
    { id: 1002, provinciaId: 101, nombre: 'San Isidro' },
    { id: 1003, provinciaId: 101, nombre: 'Los Olivos' },
    { id: 1004, provinciaId: 102, nombre: 'Asia' },
    { id: 1005, provinciaId: 102, nombre: 'Mala' },
    { id: 2001, provinciaId: 201, nombre: 'Yanahuara' },
    { id: 3001, provinciaId: 301, nombre: 'Wanchaq' }
  ];

  constructor() { }

  // 1. Obtener Departamentos
  getDepartamentos(): Observable<Departamento[]> {
    return of(this.departamentos).pipe(delay(200)); // Simula latencia de red
  }

  // 2. Obtener Provincias por ID de Departamento
  getProvincias(departamentoId: number): Observable<Provincia[]> {
    const filtrado = this.provincias.filter(p => p.departamentoId === departamentoId);
    return of(filtrado).pipe(delay(200));
  }

  // 3. Obtener Distritos por ID de Provincia
  getDistritos(provinciaId: number): Observable<Distrito[]> {
    const filtrado = this.distritos.filter(d => d.provinciaId === provinciaId);
    return of(filtrado).pipe(delay(200));
  }
}
