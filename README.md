# Description:
The library helps you manage users, tenants. Besides, managing the lifetime of "access_token and refresh_token, revoke "access_token", sharing token between applications. Support "Authorization" attribute easily to validate "Audiences, Issuer, Roles...".
[CSharp.JWT Server Nuget](https://www.nuget.org/packages/CSharp.JWT/)
[CSharp.JWT Client Nuget](https://www.nuget.org/packages/CSharpJWT.Client/)

# Configuration:
1. Generate Secret key you can use [putty](https://www.putty.org/) to do that or any application you knew.
2. Open "OAuthServer" appsettings replace your config,
    - "CSharpJWT": {
    - "Issuer": "http://localhost:5000", ==> **Replace any of the hosts**
    - "SecurityKey": "**Your secret key**", ==> **The file use generates "IssuerSigningKey".**
    - "ValidateClient": true ==> **Default always validation "client" if set by false the system will ignore that, the option often use for "multiple tenants" pattern if one should be set it by false**
  - },
  - "ConnectionStrings": {
    - "CSharpJWT": **your connectionstring**,
    - "DistCache_ConnectionString":**This [link](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-2.2) will help you understanding it**.
  - },
2. Created DistCache through command "dotnet sql-cache create "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DistCache;Integrated Security=True;" dbo TestCache".
**Remind**: Make sure your machine was installed [dotnet sql-cache](https://www.nuget.org/packages/dotnet-sql-cache/).
3. Open terminal or command line and execute [update-database](https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database) 
# Note:
If you want to get more understanding about that, you can access  the website [c-sharp.vn](https://www.c-sharp.vn/dot-net-core/multi-tenancy-with-full-stack-jwt-part-1-7c1083).
