namespace OAuthServer
{
    using Bogus;
    using CSharpJWT.Domain;
    using Microsoft.AspNetCore.Identity;
    using System;
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

            if (!_context.Users.Any(x=>x.UserName.Equals("admin")))
            {
                var user = new User { UserName = "admin" };

                user.PasswordHash = _passwordHasher.HashPassword(user, "123456");

                user.NormalizedUserName = user.UserName;

                _context.Users.Add(user);

                _context.SaveChanges();
            }

            if (!_context.Users.Any(x => x.UserName.Equals("client1")))
            {
                var user = new User { UserName = "client1" };

                user.PasswordHash = _passwordHasher.HashPassword(user, "123456");

                user.NormalizedUserName = user.UserName;

                _context.Users.Add(user);

                _context.SaveChanges();
            }

            if (!_context.Users.Any(x => x.UserName.Equals("client2")))
            {
                var user = new User { UserName = "client2" };

                user.PasswordHash = _passwordHasher.HashPassword(user, "123456");

                user.NormalizedUserName = user.UserName;

                _context.Users.Add(user);

                _context.SaveChanges();
            }
        }

        public void SeedClient()
        {
            SeedScope();

            var clientId = "www.c-sharp.vn";

            if (!_context.Clients.Any(x => x.ClientId.Equals(clientId)))
            {
                var client = new Client
                {
                    ClientId = clientId,
                    ClientName = "c-sharp",
                    ClientUri = "https://c-sharp.vn",
                    Enabled = true,
                    Secret = "CSharpJWT"
                };

                _context.Clients.Add(client);

                _context.ClientScopes.AddRange(_context.Scopes.Select(x => new ClientScope
                {
                    ClientId = client.Id,
                    ScopeId = x.Id
                }));

                _context.SaveChanges();
            }

            clientId = "www.client1.localhost";

            if (!_context.Clients.Any(x => x.ClientId.Equals(clientId)))
            {
                var client = new Client
                {
                    ClientId = clientId,
                    ClientName = "client 1",
                    ClientUri = "http://localhost:5001",
                    Enabled = true,
                    Secret = "client1_CSharpJWT"
                };

                _context.Clients.Add(client);

                _context.ClientScopes.AddRange(_context.Scopes.Where(x => !x.Name.Equals("oauthserver")).Select(x => new ClientScope
                {
                    ClientId = client.Id,
                    ScopeId = x.Id
                }));

                _context.SaveChanges();
            }

            clientId = "www.client2.localhost";

            if (!_context.Clients.Any(x => x.ClientId.Equals(clientId)))
            {
                var client = new Client
                {
                    ClientId = clientId,
                    ClientName = "client 2",
                    ClientUri = "http://localhost:5002",
                    Enabled = true,
                    Secret = "client2_CSharpJWT"
                };

                _context.Clients.Add(client);

                _context.ClientScopes.AddRange(_context.Scopes.Where(x => x.Name.Equals("client2")).Select(x => new ClientScope
                {
                    ClientId = client.Id,
                    ScopeId = x.Id
                }));

                _context.SaveChanges();
            }
        }

        public void SeedScope()
        {
            if (!_context.Scopes.Any(x => x.Name.Equals("oauthserver")))
            {
                _context.Scopes.Add(new Scope("oauthserver"));
            }

            if (!_context.Scopes.Any(x => x.Name.Equals("client1")))
            {
                _context.Scopes.Add(new Scope("client1"));
            }

            if (!_context.Scopes.Any(x => x.Name.Equals("client2")))
            {
                _context.Scopes.Add(new Scope("client2"));
            }

            _context.SaveChanges();
        }

        public void SeedUserClient()
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName.Equals("admin"));

            var client = _context.Clients.FirstOrDefault(x => x.ClientId.Equals("www.c-sharp.vn"));

            _context.UserClients.Add(new UserClient { UserId = user.Id, ClientId = client.Id });

            _context.SaveChanges();
        }

        public void SeedRole()
        {
            if (!_context.Roles.Any())
            {
                _context.Roles.Add(new Role("Administrator"));

                _context.Roles.Add(new Role("User"));

                _context.SaveChanges();
            }
        }

        public void SeedUserRole()
        {
            if (!_context.UserRoles.Any())
            {
                var users = _context.Users.ToList();

                var roles = _context.Roles.ToList();

                foreach (var user in users.Select((item, index) => new
                {
                    index = index,
                    item = item
                }))
                {
                    _context.UserRoles.Add(new IdentityUserRole<string>
                    {
                        UserId = user.item.Id,
                        RoleId = roles[user.index].Id
                    });
                }

                _context.SaveChanges();
            }
           
        }
    }
}
