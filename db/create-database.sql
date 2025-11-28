/* ============================================================================
   RegistrationDb - server-level provisioning script (ONE-SHOT)
   - This script is intended to be executed EXACTLY ONCE per environment
   - Responsibilities:
       * Create RegistrationDb database
       * Configure basic database options (optional)
       * Create SQL login + db user + role for the application
   - Schema, tables, constraints, indexes, and seed data are managed by
     EF Core migrations from the application.
   ============================================================================*/

SET NOCOUNT ON;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;

/* 1) Create database */
CREATE DATABASE RegistrationDb;
GO

-- Full recovery mode: For production OLTP, FULL is typical.
-- IMPORTANT: If you switch to FULL recovery model in production, you MUST configure a transaction log backup strategy
-- ALTER DATABASE RegistrationDb SET RECOVERY FULL;

-- Optimistic reads: better concurrency for many workloads.
-- ALTER DATABASE RegistrationDb SET READ_COMMITTED_SNAPSHOT ON;

/* Optional: database options (many orgs enforce these via policies already) */
ALTER DATABASE RegistrationDb SET PAGE_VERIFY CHECKSUM;
GO

/* 2) Create login (server level) */
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

/* 3) Map login to user in RegistrationDb and grant least privilege */
USE RegistrationDb;
GO

CREATE USER app_registration FOR LOGIN app_registration_login;
GO

CREATE SCHEMA registration AUTHORIZATION dbo;
GO

CREATE ROLE app_registration_rw;
GO

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::registration TO app_registration_rw;
GRANT EXECUTE ON SCHEMA::registration TO app_registration_rw;
GO

ALTER ROLE app_registration_rw ADD MEMBER app_registration;
GO

ALTER USER app_registration WITH DEFAULT_SCHEMA = registration;
GO

PRINT 'RegistrationDb server-level provisioning complete. Run the application to apply EF Core migrations.';
GO

/* ============================================================================
   ADDITIONAL: Migration user (for EF Core migrations)
   - This login is used ONLY to run migrations (not by the app at runtime)
   ============================================================================*/

USE master;
GO

CREATE LOGIN app_registration_migrator
WITH PASSWORD = 'Change_This_Migrator_Password!',
    CHECK_POLICY = ON;
GO

USE RegistrationDb;
GO

CREATE USER app_registration_migrator FOR LOGIN app_registration_migrator;
GO

ALTER ROLE db_owner ADD MEMBER app_registration_migrator;
GO

PRINT 'Migration user app_registration_migrator created and added to db_owner.';
GO