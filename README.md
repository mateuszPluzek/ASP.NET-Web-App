## CRM .NET template app
## Table Of contents
- [Introduction](#introduction)
- [Installation](#installation)
- [Features](#features)
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
