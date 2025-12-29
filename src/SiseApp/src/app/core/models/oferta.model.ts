
export interface Empresa {
  idEmpresa: number;
  razonSocial: string;
  direccion: string;
  correo: string;
  telefono: string;
  descripcion: string;
  ruc: string;
  sitioWeb: string;
  logoUrl: string;
  estado: boolean;
  fechaCreacion: string;
}

export interface Modalidad {
  idModalidadTrabajo: number;
  nombreModalidad: string;
}

export interface TipoContrato {
  idTipoContrato: number;
  nombreTipo: string;
}

export interface OfertaLaboral {
  idOferta: number;
  //idEmpresa: number;
  titulo: string;
  descripcion: string;
  requisitos: string;
  ubicacion: string;
  sueldo: number;
  fechaPublicacion: string;
  fechaCierre: string;
  idEmpresa?: string;
  modalidad?: string;
  tipoContrato?: string;
  //estado: boolean;
  empresaRuc?: string;
  empresaRazonSocial?: string;
  empresaTelefono?: string;
  empresaCorreo?: string;
}