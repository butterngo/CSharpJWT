namespace CSharpJWT.Domain
{
    using Microsoft.AspNetCore.Identity;

    public class Role: IdentityRole
    {
        public Role() { }
        public Role(string roleName) : base(roleName) { }
    }
}
