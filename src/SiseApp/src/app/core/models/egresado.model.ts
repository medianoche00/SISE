
export interface Egresado {
  idEgresado: number;
  idCarrera: number;
  nombreCarrera: string;
  codigoUniversitario: string;
  anioEgreso: number;
  estado: string;
}

export interface EgresadoCrearDto {
  idPersona: number;
  idCarrera: number;
  anioEgreso: number;
  codigoUniversitario: string;
  documentoRespaldo: string;
  username: string;
  email: string;
  password: string;
}

export interface EgresadoActualizarDto {
  idEgresado: number;
  idCarrera: number;
  anioEgreso: number;
  codigoUniversitario: string;
  documentoRespaldo: string;
  estado: string;
}
