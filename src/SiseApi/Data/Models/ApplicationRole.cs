using Microsoft.AspNetCore.Identity;

namespace SiseApi.Data.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}
