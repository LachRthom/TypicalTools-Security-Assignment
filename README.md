# Typical Tech Tools

An ASP.NET Core MVC web portal for a fictional tech products company, built for my Diploma of IT (Advanced Programming) at TAFE Queensland, 2024.

This repo contains two versions:

- **TypicalTools**: the original portal. Products and customer comments are stored in CSV files.
- **TypicalTechTools**: a security-focused rebuild of the same portal (security assignment).

## Features

- Browse products and view product details
- Customers can add, edit and remove comments on products
- Login with role-based access, and admin-only product management (create, edit, delete)
- Warranty claim form download, and claim document upload

## Security improvements (TypicalTechTools)

- Moved from CSV files to a SQL Server database using Entity Framework Core, with migrations
- User roles controlling what each user can access
- Passwords hashed with BCrypt
- Uploaded files encrypted at rest with AES, through a custom encryption service
- Anti-forgery tokens on forms to prevent cross-site request forgery
- HTTPS redirection

## Tech

- C# / ASP.NET Core MVC
- Entity Framework Core with SQL Server
- BCrypt.Net, System.Security.Cryptography (AES)
- Razor views, HTML, CSS

## Running it locally

1. Install the .NET SDK and SQL Server Express
2. Open `TypicalTools.sln` in Visual Studio
3. Run `Update-Database` in the Package Manager Console to create the database
4. Run the project. A demo admin account is created on first run (see `DataAccess.cs`)

## Notes

The encryption key in `appsettings.json` is a demo value. In a production app it would be stored outside the code, for example in user secrets or Azure Key Vault.
