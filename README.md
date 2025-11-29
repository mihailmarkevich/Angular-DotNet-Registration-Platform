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
- HTTP client services

---

## 3. Security Considerations

### ✔ CSRF Protection
The application **does not use cookies** for authentication or sessions.  
All API calls are performed via JSON requests without implicit credentials.

- When authentication is introduced, the recommended approach is:
  - Use Bearer tokens in the `Authorization` header (no cookies) → CSRF avoided by design.
  - If cookies are ever used for auth, add proper anti-forgery tokens and `SameSite` cookies.

---

### ✔ XSS Prevention

**Frontend (Angular)**  
- Angular’s interpolation (`{{ }}`) automatically escapes HTML.  
- No innerHTML rendering of user input.  
- Forms use built-in validation.

**Backend**
- DTO validation forbids HTML-like characters where inappropriate (`<`, `>`).
- API responses are returned as JSON, never HTML.
- CSP header is enabled:

```
Content-Security-Policy:
  default-src 'self';
  script-src 'self';
  object-src 'none';
  base-uri 'self';
  frame-ancestors 'none';
```

This blocks:
- Inline scripts
- External untrusted scripts
- Frame embedding
- Most XSS vectors

Combined with Angular’s encoding, the system is protected from stored & reflected XSS attacks.

---

### ✔ Password Security (PBKDF2)

- Passwords are hashed using **PBKDF2** with:
  - Random 128-bit salt
  - 100,000 iterations
  - 256-bit derived key
- Comparison uses constant-time verification.

---

### ✔ Database Least Privilege

Two SQL accounts:

1. **Migration user**
   - Has DDL rights for applying EF migrations.

2. **Application user**
   - Has minimal read/write permissions on runtime tables.

This minimizes the damage surface in case credentials are compromised.

---

## 4. Installation & Setup

### 4.1 Prerequisites

- **.NET 8 SDK**
- **Node.js** (LTS)
- **Angular CLI** (recommended)
- **Microsoft SQL Server**
- `dotnet` CLI available in PATH

---

### 4.2 Backend Setup

#### Step 1 — Create the database

Run the script:

```
db/create-database.sql
```

It creates:
- Database
- Migration user
- Application user
- Permissions

---

#### Step 2 — Configure connection strings

`Server.API/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=RegistrationDb;User Id=app_user;Password=***;TrustServerCertificate=True;"
}
```

`app_registration_migrator/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=RegistrationDb;User Id=migration_user;Password=***;TrustServerCertificate=True;"
}
```

---

#### Step 3 — Restore & build backend

```bash
dotnet restore
dotnet build
```

---

#### Step 4 — Run EF migrations

```bash
cd src/app_registration_migrator
dotnet run
```

---

#### Step 5 — Run the API

```bash
cd src/Server.API
dotnet run
```

Swagger UI:  
```
https://localhost:<port>/swagger
```

Ensure CORS includes Angular:

```json
"AllowedOrigins": [ "http://localhost:4200" ]
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

Visit:

```
http://localhost:4200
```

## 4.4 HTTPS / TLS Notes

For simplicity of installation, review, and local testing, this repository **does not include HTTPS certificates or TLS configuration** out of the box.

The application runs using:

- **HTTP** for the Angular development server (`http://localhost:4200`)
- **HTTP or the automatically generated ASP.NET Developer Certificate** for the backend

This lightweight setup is intentional to minimize onboarding time.

### ⚠️ Production Requirements

For any real deployment:

- **Enable HTTPS** for the API.
- Use a valid TLS certificate (e.g., Let’s Encrypt or reverse proxy termination).
- Redirect all HTTP traffic to HTTPS.
- Enable HSTS (`Strict-Transport-Security` header).
- Ensure Angular is configured to call HTTPS endpoints.

ASP.NET Core minimal production middleware:

```csharp
app.UseHttpsRedirection();
app.UseHsts();
```

TLS is **mandatory** to ensure user passwords and sensitive data are transmitted securely.