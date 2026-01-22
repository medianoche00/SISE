export interface Representante {
  idRepresentante: number;
  idEmpresa: number;
  nombreEmpresa: string;
  idPersona: number;
  idUsuario: number;
  cargo: string;
  estado: string;
}

export interface RepresentanteCrearDto {
  username: string;
  email: string;
  password: string;
  idEmpresa: number;
  idPersona: number;
  cargo?: string;
  documentoRespaldo: string;
}

export interface RepresentanteActualizarDto {
  idRepresentante: number;
  idEmpresa: number;
  cargo?: string;
  documentoRespaldo: string;
}
