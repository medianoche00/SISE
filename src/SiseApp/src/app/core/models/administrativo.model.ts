export interface Administrativo {
  idAdministrativo: number;
  idCargoAdministrativo: number;
  nombreCargo: string; // Viene del JOIN
  idPersona: number;
  idUsuario: number;
  estado: string;
}

export interface AdministrativoCrearDto {
  username: string;
  email: string;
  password: string;
  idCargoAdministrativo: number;
  idPersona: number;
  documentoRespaldo: string;
}

export interface AdministrativoActualizarDto {
  idAdministrativo: number;
  idCargoAdministrativo: number;
  documentoRespaldo: string;
  estado: string;
}
