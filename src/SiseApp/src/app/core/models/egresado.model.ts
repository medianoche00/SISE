export interface RegistroEgresadoRequest {
  // Datos Personales
  nombres: string;
  apellidoPaterno: string;
  apellidoMaterno: string;
  documentoIdentidad: string;
  telefono?: string;
  correoPersonal?: string;

  // Datos Académicos
  idCarrera: number;
  codigoUniversitario: string;
  añoEgreso: number;
}

export interface Carrera {
  idCarrera: number;
  nombreCarrera: string;
}
