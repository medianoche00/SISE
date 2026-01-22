export interface Carrera {
  idCarrera: number;
  nombreCarrera: string;
  estado: string;
}

export interface CargoAdministrativo {
  idCargoAdministrativo: number;
  nombreCargo: string;
  estado: string;
}

export interface Rol {
  idRol: number;
  nombreRol: string;
}

export interface Empresa {
  idEmpresa: number;
  razonSocial: string;
  estado: string;
}
