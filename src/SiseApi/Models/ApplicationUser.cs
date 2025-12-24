using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiseApi.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [ForeignKey("IdUsuario")]
        public virtual Persona? Persona { get; set; }
        // public bool Estado { get; set; }

        [InverseProperty("Usuario")]
        public virtual ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();

        [InverseProperty("Usuario")]
        public virtual ICollection<Egresado> Egresados { get; set; } = new List<Egresado>();
        
        [InverseProperty("Usuario")]
        public virtual ICollection<Representante> Representantes { get; set; } = new List<Representante>();
    }
}
