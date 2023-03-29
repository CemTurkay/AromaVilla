# AramoVilla
A Sample N-layered .NET Core Project Demonstrasting Clean Architecture and the Generic Reporsitory Pattern.

## Packages

### ApplicationCore
```
Install-Package Ardalis.Specification -v 6.1.0
```
### Infrastructure
```
Install-Package Microsoft.EntityFrameworkCore -v 6.0.15
Install-Package Microsoft.EntityFrameworkCore.Tools -v 6.0.15
Install-Package Npgsql.EntityFrameworkCore.PostgreSQL -v 6.0.8
```

## Migrations
Before running the following comands, make sure that Web is set as startup project.
Run the following comands on project "Infrastructure".

### Infrastructure
```
Add-Migration InitialCreate -context ShopContext -OutputDir Data/Migrations
Update-Database -context ShopContext
```