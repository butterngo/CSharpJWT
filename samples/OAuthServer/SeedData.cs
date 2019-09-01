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


            if (!_context.Users.Any())
            {
                var user = new User { UserName = "admin" };

                user.PasswordHash = _passwordHasher.HashPassword(user, "123456");

                user.NormalizedUserName = user.UserName;

                _context.Users.Add(user);

                _context.SaveChanges();
            }

            var user1 = _context.Users.FirstOrDefault();

            Console.WriteLine($"Username: {user1.UserName}");
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

            clientId = "www.client1.localhost";

            if (!_context.Clients.Any(x => x.ClientId.Equals(clientId)))
            {
                _context.Clients.Add(new Client
                {
                    ClientId = clientId,
                    ClientName = "client 1",
                    ClientUri = "http://localhost:5001",
                    Enabled = true,
                    Secret = "client1_CSharpJWT"
                });

                _context.SaveChanges();
            }

            clientId = "www.client2.localhost";

            if (!_context.Clients.Any(x => x.ClientId.Equals(clientId)))
            {
                _context.Clients.Add(new Client
                {
                    ClientId = clientId,
                    ClientName = "client 2",
                    ClientUri = "http://localhost:5002",
                    Enabled = true,
                    Secret = "client2_CSharpJWT"
                });

                _context.SaveChanges();
            }
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
