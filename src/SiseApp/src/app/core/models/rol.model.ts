// Interfaces para los DTOs de creación
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

export interface AdministrativoCrearDto {
  idPersona: number;
  idCargoAdministrativo: number;
  documentoRespaldo: string;
  username: string;
  email: string;
  password: string;
}

export interface RepresentanteCrearDto {
  idPersona: number;
  idEmpresa: number;
  cargo: string;
  documentoRespaldo: string;
  username: string;
  email: string;
  password: string;
}
export interface RolOpcion {
  codigo: string;
  nombre: string;
}
