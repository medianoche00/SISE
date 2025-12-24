using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Table("Persona")]
[Index(nameof(DocumentoIdentidad), IsUnique = true)] // Índice único moderno
public class Persona
{
    [Key]
    [Column("idPersona")]
    public int IdPersona { get; set; }

    [Column("nombres")]
    [StringLength(100)]
    public string Nombres { get; set; } = null!;

    [Column("apellidoPaterno")]
    [StringLength(100)]
    public string ApellidoPaterno { get; set; } = null!;

    [Column("apellidoMaterno")]
    [StringLength(100)]
    public string ApellidoMaterno { get; set; } = null!;

    // CAMBIO: De Dni a DocumentoIdentidad para soportar C.E./Pasaporte
    [Column("documentoIdentidad")]
    [StringLength(20)]
    [Unicode(false)]
    public string DocumentoIdentidad { get; set; } = null!;

    [Column("telefono")]
    [StringLength(15)]
    public string? Telefono { get; set; }

    // Este es el correo de contacto personal, distinto al del login
    [Column("correoPersonal")]
    [StringLength(150)]
    public string? CorreoPersonal { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("Persona")]
    public virtual ICollection<Egresado> Egresados { get; set; } = new List<Egresado>();

    [InverseProperty("Persona")]
    public virtual ICollection<Representante> Representantes { get; set; } = new List<Representante>();
}
