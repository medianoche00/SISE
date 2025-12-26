// src/app/core/models/oferta.model.ts
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
  fechaCreacion: string; // O Date
  // ... otros campos si son necesarios
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
  idEmpresa: number;
  titulo: string;
  descripcion: string;
  requisitos: string;
  ubicacion: string;
  sueldo: number;
  fechaPublicacion: string; // O Date
  fechaCierre: string; // O Date
  idEmpresaNavigation?: Empresa;
  idModalidadTrabajoNavigation?: Modalidad;
  idTipoContratoNavigation?: TipoContrato;
  estado: boolean;
  // ... agrega el resto si lo necesitas
}