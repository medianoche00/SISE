
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
