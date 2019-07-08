namespace OAuthServer
{
    using Bogus;
    using CSharpJWT.Domain;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;

    /// <summary>
    /// Using Bogus for fake data, refer the link: https://github.com/bchavez/Bogus
    /// </summary>
    public class SeedData
    {
        private readonly CSharpJWTContext _context;

        private readonly IPasswordHasher<User> _passwordHasher;

        public SeedData(CSharpJWTContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public void SeedUser()
        {

            if (!_context.Users.Any())
            {
                for (var i = 0; i <= 1; i++)
                {
                    var testUsers = new Faker<User>()
                    .CustomInstantiator(f => new User())
                    .RuleFor(u => u.UserName, (f, u) => f.Internet.Email());

                    var user = testUsers.Generate();

                     user.PasswordHash = _passwordHasher.HashPassword(user, "123456");

                    user.NormalizedUserName = user.UserName;

                    _context.Users.Add(user);
                }

                _context.SaveChanges();
            }

        }

        public void SeedClient()
        {
            var clientId = "www.c-sharp.vn";

            if (!_context.Clients.Any(x => x.ClientId.Equals(clientId)))
            {
                _context.Clients.Add(new Client
                {
                    ClientId = clientId,
                    ClientName = "c-sharp",
                    ClientUri = "https://c-sharp.vn",
                    Enabled = true,
                    Secret  = "CSharpJWT"
                });

                _context.SaveChanges();
            }
        }
    }
}
