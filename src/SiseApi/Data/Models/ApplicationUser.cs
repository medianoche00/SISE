using Microsoft.AspNetCore.Identity;

namespace SiseApi.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public virtual ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
        public virtual ICollection<Egresado> Egresados { get; set; } = new List<Egresado>();
        public virtual ICollection<Representante> Representantes { get; set; } = new List<Representante>();
        public virtual ICollection<Administrativo> Administrativos { get; set; } = new List<Administrativo>();
    }
}