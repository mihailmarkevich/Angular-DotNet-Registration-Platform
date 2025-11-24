/* ============================================================================
   RegistrationDb - initial provisioning script (one-shot)
   - This script is intended to be executed EXACTLY ONCE
   - Creates database
   - Sets sane OLTP database options
   - Creates tables and constraints
   - Seeds lookup data
   - Creates least-privilege app principal (login + user + role)
   ============================================================================*/

SET NOCOUNT ON;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;

/* ============================================================================
   1. Create database
   ============================================================================*/
CREATE DATABASE RegistrationDb;
GO

-- Full recovery mode: For production OLTP, FULL is typical.
-- IMPORTANT: If you switch to FULL recovery model in production, you MUST configure a transaction log backup strategy
-- ALTER DATABASE RegistrationDb SET RECOVERY FULL;

-- Optimistic reads: better concurrency for many workloads.
-- ALTER DATABASE RegistrationDb SET READ_COMMITTED_SNAPSHOT ON;

-- Make page corruption detectable; default in modern versions but set explicitly.
ALTER DATABASE RegistrationDb SET PAGE_VERIFY CHECKSUM;
GO

/* ============================================================================
   3. Use RegistrationDb
   ============================================================================*/
USE RegistrationDb;
GO

/* ============================================================================
   4. Dedicated schema for the application
      - Keep app objects out of dbo for clearer ownership & security
   ============================================================================*/
CREATE SCHEMA registration AUTHORIZATION dbo;
GO

/* ============================================================================
   5. Core tables (in [registration] schema)
   ============================================================================*/

-- Industries
CREATE TABLE registration.Industries
(
    Id   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Industries PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL
);
GO

-- Companies
CREATE TABLE registration.Companies
(
    Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Companies PRIMARY KEY,
    Name       NVARCHAR(255) NOT NULL,
    IndustryId INT NOT NULL,
    CreatedAt  DATETIME2(3) NOT NULL 
               CONSTRAINT DF_Companies_CreatedAt DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Companies_Industries
        FOREIGN KEY (IndustryId) REFERENCES registration.Industries(Id)
            ON DELETE NO ACTION
);
GO

-- Users
CREATE TABLE registration.Users
(
    Id                INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    CompanyId         INT NOT NULL,
    FirstName         NVARCHAR(100) NOT NULL,
    LastName          NVARCHAR(100) NOT NULL,
    UserName          NVARCHAR(100) NOT NULL,
    Email             NVARCHAR(256) NULL,
    PasswordHash      VARBINARY(512) NOT NULL,
    PasswordSalt      VARBINARY(256) NULL,
    CreatedAt         DATETIME2(3) NOT NULL 
                      CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME(),
    TermsAcceptedAt   DATETIME2(3) NOT NULL,
    PrivacyAcceptedAt DATETIME2(3) NOT NULL,

    CONSTRAINT FK_Users_Companies
        FOREIGN KEY (CompanyId) REFERENCES registration.Companies(Id)
            ON DELETE CASCADE
);
GO

/* ============================================================================
   6. Indexes
   ============================================================================*/

-- Unique index on username (global uniqueness)
CREATE UNIQUE INDEX IX_Users_UserName 
    ON registration.Users (UserName);
GO

-- Helpful index to support joins/filtering by CompanyId (1 company -> many users)
CREATE INDEX IX_Users_CompanyId 
    ON registration.Users (CompanyId);
GO

-- Helpful index to support joins/filtering by IndustryId
CREATE INDEX IX_Companies_IndustryId 
    ON registration.Companies (IndustryId);
GO

-- Search companies by name (used on front)
CREATE INDEX IX_Companies_Name ON registration.Companies (Name);

/* ============================================================================
   7. Seed lookup data (one-shot)
   ============================================================================*/
INSERT INTO registration.Industries (Name) VALUES
(N'Manufacturing'),
(N'IT Services'),
(N'Healthcare'),
(N'Finance'),
(N'Retail');
GO

/* ============================================================================
   8. Security: application principal with least privilege
      - app_registration_login  (server login, created in master)
      - app_registration        (db user)
      - app_registration_rw     (db role scoped to [registration] schema)
   ============================================================================*/

-- 8.1 Create login (server level) – run in master
USE master;
GO

/* 
   NOTE ABOUT PASSWORDS (IMPORTANT):

   - This password is a placeholder for demos / local setups.
   - For any serious environment:
       * Do NOT commit real passwords into source control.
       * Create the login with a strong password provided
         via a secure channel (secret store, CI/CD variable, etc.).
       * Rotate passwords regularly in coordination with
         application configuration (connection strings).
*/
CREATE LOGIN app_registration_login
WITH PASSWORD = 'Change_This_Password_Immediately!', 
    CHECK_POLICY = ON;
GO

-- 8.2 Map login to user in RegistrationDb
USE RegistrationDb;
GO

CREATE USER app_registration FOR LOGIN app_registration_login;
GO

-- 8.3 Create a role and grant rights only on [registration] schema
CREATE ROLE app_registration_rw;
GO

-- Data access on app schema only
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::registration TO app_registration_rw;

-- If you later add stored procedures/functions in the [registration] schema,
-- this will allow the app to execute them, but NOT everything in dbo.
GRANT EXECUTE ON SCHEMA::registration TO app_registration_rw;
GO

ALTER ROLE app_registration_rw ADD MEMBER app_registration;
GO

-- Default schema = registration, so unqualified names will resolve there
ALTER USER app_registration WITH DEFAULT_SCHEMA = registration;
GO

PRINT 'RegistrationDb provisioning complete.';
GO