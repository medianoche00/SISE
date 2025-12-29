import { OfertaLaboral } from "./oferta.model";

export interface Postulacion {
    idPostulacion: number;
    idRepresentanteEvaluador: number;
    fechaPostulacion: Date;
    fechaEvaluacion: Date | null;
    estado: string;
    comentarios: string;
    cartaPresentacion: string;

    oferta: OfertaLaboral;
}