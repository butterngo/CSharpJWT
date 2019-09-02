namespace CSharpJWT.Services
{
    using System.Threading.Tasks;
    using CSharpJWT.Domain;
    using Microsoft.EntityFrameworkCore;

    public class ScopeService : IScopeService
    {
        protected DbSet<Scope> Scopes { get; private set; }

        private readonly CSharpJWTContext _context;

        public ScopeService(CSharpJWTContext context)
        {
            Scopes = context.Scopes;

            _context = context;
        }

        public async Task<Scope> CreateAsync(string name)
        {
            var scope = new Scope(name);

            Scopes.Add(scope);

            await _context.SaveChangesAsync();

            return scope;
        }

        public async Task<Scope> FindByIdAsync(string id)
         => await Scopes.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }
}
