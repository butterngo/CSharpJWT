namespace CSharpJWT.Services
{
    using System.Threading.Tasks;

    public interface IScopeService
    {
        Task<Domain.Scope> CreateAsync(string name);

        Task<Domain.Scope> FindByIdAsync(string id);
    }
}
