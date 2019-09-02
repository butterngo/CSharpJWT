namespace CSharpJWT.Domain
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class CSharpJWTContext 
        : IdentityDbContext<User, Role, string>
    {
        public CSharpJWTContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<UserClient> UserClients { get; set; }

        public virtual DbSet<Scope> Scopes { get; set; }

        public virtual DbSet<ClientScope> ClientScopes { get; set; }
    }
}
