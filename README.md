# Description:
The library helps you manage users, tenants. Besides, managing the lifetime of "access_token and refresh_token, revoke "access_token", sharing token between applications. Support "Authorization" attribute easily to validate "Audiences, Issuer, Roles...".

# Configuration:
1. Generate Secret key you can use [putty](https://www.putty.org/) to do that or any application you knew.
2. Open "OAuthServer" appsettings replace your config,
    - "JWTSettings": {
    - "Issuer": "http://localhost:5000", ==> **Replace any of the hosts**
    - "SecretPath": "{**Your Path File on machine**}", ==> **The file use generates "IssuerSigningKey".**
    - "ValidateClient": true ==> **Default always validation "client" if set by false the system will ignore that, the option often use for "multiple tenants" pattern if one should be set it by false**
  - },
  - "ConnectionStrings": {
    - "CSharpJWT": "{**your connectionstring**}",
    - "DistCache_ConnectionString":{**This [link](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-2.2) will help you understanding it**}.
  - },
2. James Monroe
3. John Quincy Adams

