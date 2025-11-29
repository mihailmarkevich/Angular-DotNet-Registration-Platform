# Registration App (.NET + Angular)

A production-ready user registration system with modern security and clean architecture principles.  
It features:

- Multi-step Angular wizard (Company → User → Terms → Result)
- ASP.NET Core Web API backend
- SQL Server persistence via Entity Framework Core
- PBKDF2 password hashing
- CSP-based XSS protection
- Strict least-privilege database accounts

---

## 1. Application Overview

This application implements a complete **self-registration workflow**.  
Users can:

1. Search existing companies (autocomplete).
2. Create a new company if none exists.
3. Enter personal details (first name, last name, username, email).
4. Validate username availability in real-time via an async API.
5. Accept Terms of Service and Privacy Policy.
6. Submit the full registration request:
   - Validates all inputs
   - Ensures industry exists
   - Ensures username is unique
   - Creates company if needed
   - Creates the initial user
   - Executes all operations inside a **single database transaction**

API surface:

- `POST /api/registration` — completes the registration workflow
- `GET /api/registration/check-username` — returns username availability
- `GET /api/companies/search` — autocomplete for companies
- `GET /api/industries` — industries list

---

## 2. Technologies Used

### Backend
- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core**
- **Microsoft SQL Server (MSSQL)** as the database engine
- **PBKDF2 password hashing**
- **Clean architecture-style structure**:
  - Application (interfaces, features)
  - Domain (entities)
  - Infrastructure (EF Core, repositories, security)
  - Web (controllers, DTOs, mappings)

### Frontend
- **Angular**
- **Angular Material**
- **Reactive Forms** with sync & async validation

---

## 3. Security Considerations

### ✔ CSRF Protection
The application **does not use cookies**, therefore CSRF attacks are not possible.

- When authentication is introduced, the recommended approach is:
  - Use Bearer tokens in the `Authorization` header (no cookies) → CSRF avoided by design.
  - If cookies are ever used for auth, add proper anti-forgery tokens and `SameSite` cookies.

---

### ✔ XSS Prevention

**Frontend (Angular)**  
- Angular’s interpolation escapes HTML.  
- No innerHTML used.

**Backend**
- Request validation rejects dangerous input.
- API responses are returned as JSON, never HTML.
- CSP header enabled:

```
Content-Security-Policy:
  default-src 'self';
  script-src 'self';
  object-src 'none';
  base-uri 'self';
  frame-ancestors 'none';
```

---

### ✔ Password Security (PBKDF2)

Passwords are hashed with:
- PBKDF2
- 100,000 iterations
- 128-bit random salt
- Constant‑time comparison

---

### ✔ Database Least Privilege

Two SQL accounts:

1. **app_registration_migrator** — migration user (DDL)
2. **app_registration_login** — runtime user (DML only)

⚠️ **IMPORTANT: You must change the default passwords for both SQL users immediately after installation.**

---

## 4. Installation & Setup

### 4.1 Prerequisites

- .NET 8 SDK  
- Node.js (LTS)  
- Angular CLI (recommended)  
- Microsoft SQL Server  
- `dotnet` CLI available in PATH

---

### 4.2 Backend Setup

#### Step 1 — Create the database

Run:

```
db/create-database.sql
```

This creates:
- The `RegistrationDb` database  
- SQL user: **app_registration_migrator**  
- SQL user: **app_registration_login**  
- Required permissions  

⚠️ **IMPORTANT: Change passwords for both users before production use.**

---

#### Step 2 — Configure connection strings

`Server.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=RegistrationDb;User Id=app_registration_login;Password=Change_This_Password_Immediately!;TrustServerCertificate=True;"
}
```

---

#### Step 3 — Restore & build backend

```bash
dotnet restore
dotnet build
dotnet tool restore
```

---

#### Step 4 — Run EF migrations with app_registration_migrator login

```bash
dotnet ef database update --context AppDbContext --connection "Server=YOUR_SERVER;Database=RegistrationDb;User Id=app_registration_migrator;Password=Change_This_Password_Immediately!;TrustServerCertificate=True;"
```

This applies all migrations using the migration user.

---

#### Step 5 — Run the API

```bash
cd Server.API
dotnet run
```

Swagger UI:  
```
https://localhost:<port>/swagger
```

---

### 4.3 Frontend Setup (Angular)

#### Step 1 — Install packages

```bash
cd Client
npm install
```

#### Step 2 — Configure API URL

`src/environments/environment.ts`:

```ts
export const environment = {
  production: false,
  apiBaseUrl: "https://localhost:<api-port>"
};
```

#### Step 3 — Run Angular dev server

```bash
npm start
```

Open:

```
http://localhost:4200
```

---

## 4.4 HTTPS / TLS Notes

This repository does **not** bundle TLS certificates (for simplicity and easier evaluation).

Production deployment **must**:

- Use HTTPS everywhere
- Redirect HTTP → HTTPS
- Enable HSTS
- Use a real TLS certificate (Let’s Encrypt recommended)

ASP.NET minimal production config:

```csharp
app.UseHttpsRedirection();
app.UseHsts();
```

TLS is mandatory for secure password transmission.

