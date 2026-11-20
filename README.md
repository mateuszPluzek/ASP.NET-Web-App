## CRM .NET template app
## Table Of contents
- [Introduction](#introduction)
- [Installation](#installation)
- [Features](#features)
- [Future content](#futurecontent)
## Introduction
This app was developed as a learning project using ASP.NET Core with Blazor for the front end. It is a template web application that allows users to create accounts. When logged in, users can change their profile information.

Users must verify their email to activate their account. Features such as "Forgot my password" also rely on email (a reset link is sent to the user's email). After logging in, authentication is handled using JWT.

The website supports two languages: Polish and English (US) (en-US).

This application does not include any domain-specific features; it only provides the essential functionality that most web apps need. It can serve as a starting point for any website that requires user registration backed by a database.

## Installation

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- [Git](https://git-scm.com/)
- A compatible SQL database (e.g., SQL Server, LocalDB, PostgreSQL)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## Instal

### 1. Clone the Repository

```bash
git clone https://github.com/mateuszPluzek/ASP.NET-Web-App.git
cd ASP.NET-Web-App
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure the Application

Create an `appsettings.Development.json` file or update the existing configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AspNetWebApp;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

For sensitive values, use [.NET user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
```

### 4. Configure the Database

Install the Entity Framework CLI if it is not already available:

```bash
dotnet tool install --global dotnet-ef
```

Apply the migrations:

```bash
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

Open the HTTPS URL printed in the terminal, typically:

```text
https://localhost:5001
```

For development with automatic reload:

```bash
dotnet watch run
```
## Email Setup

Configure email delivery before enabling features such as account confirmation, password resets, or email notifications.

### Prerequisites

- An SMTP server
- A sender address
- SMTP credentials, if the provider requires authentication
- TLS/SSL enabled, depending on the provider

### Configuration

Add the email settings to `appsettings.Development.json` or configure them through environment variables:

```json
{
  "Email": {
    "Host": "smtp.example.com",
    "Port": 587,
    "Username": "YOUR_SMTP_USERNAME",
    "Password": "YOUR_SMTP_PASSWORD",
    "FromAddress": "noreply@example.com",
    "FromName": "ASP.NET Web App",
    "EnableSsl": true
  }
}
```

Prefer environment variables or user secrets for credentials. Examples:

```bash
dotnet user-secrets set "Email:Host" "smtp.example.com"
dotnet user-secrets set "Email:Port" "587"
dotnet user-secrets set "Email:Username" "YOUR_SMTP_USERNAME"
dotnet user-secrets set "Email:Password" "YOUR_SMTP_PASSWORD"
dotnet user-secrets set "Email:FromAddress" "noreply@example.com"
dotnet user-secrets set "Email:FromName" "ASP.NET Web App"
dotnet user-secrets set "Email:EnableSsl" "true"
```

> [!WARNING]
> Do not commit real passwords or API keys to the repository. Use user secrets for local development and environment variables (or a secrets manager) in production.
> 
## Features

### Swagger support

Built-in Swagger support, allowing for easy API testing with JWT authorization.

<img width="1464" height="757" alt="Swagger UI" src="https://github.com/user-attachments/assets/707eb869-c60e-47b1-8cfd-783c210840c2" />

### Localization

An easy-to-extend localization resource, allowing support for additional languages to be added.

<img width="1000" height="509" alt="Localization" src="https://github.com/user-attachments/assets/6e79d9a8-17dc-42b6-a62b-8e1a9fb976ac" />

### Simple UI

Allows testing of language support and dark/light theme.

<img width="1920" height="226" alt="UI theme toggle" src="https://github.com/user-attachments/assets/d5b996e3-7e0f-407c-8da8-f4cba3b3dee5" />

### User profile

A page that allows the user to view and change their information. It also supports uploading a custom profile image.

<img width="1705" height="713" alt="User profile page" src="https://github.com/user-attachments/assets/6fa40e10-d30e-4307-b9f8-afe88fb91d3d" />

### Developer-friendly error display

Errors intended for the end user are displayed in a dedicated box, making it easier to test the app during development.

<img width="1690" height="250" alt="Error display" src="https://github.com/user-attachments/assets/cc30c04a-aed2-454d-befd-59467bc14e6c" />

### Email verification

Changes to your account, as well as activating it while you are not logged in, require you to enter a verification code sent to your email.

<img width="710" height="368" alt="Email verification" src="https://github.com/user-attachments/assets/1f44ae9a-1495-4d52-b828-fb1873c7618f" />

## Future Content

Planned updates:

- JWT refresh tokens
- Refreshing user info after changes (currently, user data is stored locally after logging in)
- Error logging to the database
- Allowing the user to change their profile description
- Searching other users
- Admin panel
