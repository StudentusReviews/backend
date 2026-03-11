# backend

## Run Locally

Clone the project

```bash
  git clone https://github.com/StudentusReviews/backend.git
```

Go to the project directory

```bash
  cd backend
```

## Backend Configuration

The backend uses `appsettings.json` and `appsettings.Development.json` for base configuration. When running with Docker, you provide overrides using a `.env` file in the same directory as `compose.yaml`. You can copy `.env.example` to `.env` to get started.

> **Note on Environment Variables:** ASP.NET Core uses a colon (`:`) to separate configuration sections (e.g., `Section:Key`). However, since colons are not valid in environment variable names on all platforms, the framework uses a double underscore (`__`) as a delimiter. For example, to override `Logging:LogLevel:Default` via an environment variable, you would define `Logging__LogLevel__Default` in your `.env` file.

Here are the main configuration options and what they do:

### Database Connections
The app connects to two PostgreSQL databases. You can override these in your `.env` file:
* `DatabaseConnections` - Database connection details
  * `DatabaseConnections:MainDatabase` - Primary application database configuration root.
    * `DatabaseConnections:MainDatabase:Host` (string) - Database host address.
    * `DatabaseConnections:MainDatabase:Port` (integer) - Database connection port.
    * `DatabaseConnections:MainDatabase:Database` (string) - Name of the database.
    * `DatabaseConnections:MainDatabase:Username` (string) - PostgreSQL username.
    * `DatabaseConnections:MainDatabase:Password` (string) - PostgreSQL password.
  * `DatabaseConnections:DataProtectionDatabase` - Database that safely stores ASP.NET data protection keys.
    * `DatabaseConnections:DataProtectionDatabase:Host` (string) - Database host address.
    * `DatabaseConnections:DataProtectionDatabase:Port` (integer) - Database connection port.
    * `DatabaseConnections:DataProtectionDatabase:Database` (string) - Name of the database.
    * `DatabaseConnections:DataProtectionDatabase:Username` (string) - PostgreSQL username.
    * `DatabaseConnections:DataProtectionDatabase:Password` (string) - PostgreSQL password.

### Migrations
* `Migrations` - Database migration behavior.
  * `Migrations:ApplyMigrationsOnStartup` (boolean) - Automatically applies pending Entity Framework database migrations on startup. Usually set to true in Development.

### AWS Secrets Manager
The app supports loading secrets from [AWS Systems Manager Parameter Store](https://docs.aws.amazon.com/systems-manager/latest/userguide/systems-manager-parameter-store.html) (including values backed by Secrets Manager via SSM) using the `Amazon.Extensions.Configuration.SystemsManager` package. When enabled, all parameters under the configured path are merged into the app's configuration, allowing sensitive values (e.g. database passwords, API keys) to be stored securely in AWS instead of the `.env` file.

AWS Secrets are **only loaded in non-Development environments**. In `appsettings.Development.json`, `Secrets:UseAws` defaults to `false` so local development uses the `.env` file as usual.

* `Secrets` - Controls whether AWS secrets are loaded at startup.
  * `Secrets:UseAws` (boolean) - Set to `true` to pull configuration from AWS SSM/Secrets Manager. In `appsettings.json` (production) this defaults to `true`; in `appsettings.Development.json` it defaults to `false`.

When `Secrets:UseAws` is `true`, you must also provide:
* `AwsSecretPath` (string) - The SSM Parameter Store path prefix to load (e.g. `/aws/reference/secretsmanager/my_secret`). Set this in your `.env` file or as an environment variable. The app will throw a startup error if this value is missing.

> **AWS credentials**: The app uses the default AWS credential chain (environment variables `AWS_ACCESS_KEY_ID` / `AWS_SECRET_ACCESS_KEY` / `AWS_SESSION_TOKEN`, shared credentials file, IAM role, etc.). Make sure the runtime environment has an IAM role or credentials with at least `ssm:GetParametersByPath` and `secretsmanager:GetSecretValue` permissions on the configured path.

### Security and Hashing
* `EmailSecrets` - Cryptographic keys used for secure operations.
  * `EmailSecrets:EmailHashKey` (string) - A 512-bit hexadecimal string (128 characters). Used to irreversibly hash user emails for privacy.
  * `EmailSecrets:EmailVerificationTokenHashKey` (string) - A 512-bit hexadecimal string (128 characters). Used to securely hash email verification tokens.
> *To generate a 512-bit hex string, you can run: `openssl rand -hex 64`*

### Account Management and Emails
* `Resend` - Email delivery service configuration.
  * `Resend:Key` (string) - Your API key for the Resend service, which is used to send emails.
* `AccountConfirmation` - Verification email configuration.
  * `AccountConfirmation:EmailVerificationTokenExpirationHours` (integer) - Determines how long an email verification token remains valid in hours. Must be > 0.
  * `AccountConfirmation:SendConfirmationLetterFromAddress` (string) - The sender address for registration emails (e.g. `Account confirmation <auth@domain.com>`). Must be a valid email format.
  * `AccountConfirmation:ConfirmationLetterSubject` (string) - The subject line used for verification emails.
* `Login` - Account lockout and brute-force prevention structure.
  * `Login:MaxFailedAccessAttempts` (integer) - The number of consecutive failed login attempts allowed before a user account is temporarily locked. Must be > 0.
  * `Login:LockoutTimeMinutes` (integer) - The duration (in minutes) a user stays locked out after reaching the max failed attempts. Must be > 0.

### Web API Settings
* `Cors` - Cross-Origin Resource Sharing policy setup.
  * `Cors:AllowedOrigins` (array) - List of URLs permitted to access the API explicitly allowing credentials, all headers, and all methods (e.g. `['http://localhost:8001']`). 
* `HeaderForwarding` - Configures the application to accept `X-Forwarded-For` and `X-Forwarded-Proto` headers when running behind a reverse proxy (like Nginx or an Ingress controller) in non-Development environments.
  * `HeaderForwarding:KnownNetworks` - An array of trusted IP networks (requires `Prefix` and `PrefixLength` from 0-128).
  * `HeaderForwarding:KnownProxies` - An array of exact trusted proxy IP addresses (requires `IpAddress`).
  * *Note: If your proxy is not listed in these arrays, ASP.NET Core will drop the forwarded headers for security reasons.*
* `HealthCheck` - Infrastructure monitoring parameters.
  * `HealthCheck:RequireHost` (string) - In production, you can specify a required host header to pass the health check endpoint (`/healthz`).

### ASP.NET Core Settings (in compose.yaml)
* `ASPNETCORE_ENVIRONMENT` (string) - Sets the runtime environment (e.g., `Development`, `Production`).
* `ASPNETCORE_URLS` (string) - Specifies the ports and protocols Kestrel listens on (e.g., `https://+:8081;http://+:8080`).

### OpenIddict (Authentication Server)
The API runs its own OpenID Connect (OIDC) server using OpenIddict.

By default, OpenIddict uses development certificates when `ASPNETCORE_ENVIRONMENT` is set to `Development`. If you deploy to production, you must provide `.pfx` or `.p12` certificates for token generation:

1. Place your `.pfx` certificates for signing and encryption into a dedicated folder on your host.
2. Set `ENCRYPTION_CERTIFICATES_FOLDER_ABSOLUTE_PATH` to the absolute path of this folder. Docker will mount it to `/encryption_certs` inside the container.
3. Set `OPENIDDICT_ENCRYPTION_CERTIFICATE_FILE_CONTAINER_PATH` to the path inside the container (e.g., `/encryption_certs/encryption.pfx`).
4. Set `OPENIDDICT_SIGNING_CERTIFICATE_FILE_CONTAINER_PATH` to the path inside the container (e.g., `/encryption_certs/signing.pfx`).
5. Provide the respective certificate passwords using `OPENIDDICT_ENCRYPTION_CERTIFICATE_PASSWORD` and `OPENIDDICT_SIGNING_CERTIFICATE_PASSWORD`.

#### Client Applications Configuration

OpenIddict clients (applications that can authenticate via this server, such as your frontend) are configured in `appsettings.json` under the `OpenIddict:Applications` array. 

During startup, the application automatically synchronizes the clients defined in `appsettings.json` with the database. **If you need to add a new client or update an existing one's redirect URIs, simply modify `appsettings.json` and restart the backend.**

Important fields for a client application:
* `ClientId` (e.g. `frontend`) - The unique identifier of the client.
* `ClientType` - `Public` (for SPAs without a backend) or `Confidential` (for apps that can securely store a secret).
* `RedirectUris` / `PostLogoutRedirectUris` - Must perfectly match the URLs your client application will return to.
* `Permissions` - Defines the endpoints, grant types, and scopes the client is allowed to use.

Additionally, for any *Confidential* client applications configured in `appsettings.json` (like `frontend`), define their secret key via `OpenIddictApplicationSecrets__<CLIENT_ID>` in your `.env` (e.g., `OpenIddictApplicationSecrets__frontend=your_hex_secret`).

Ensure you have docker installed and running

Run this command

```bash
  docker compose up -d
```

## Development

Add ApplicationDatabase migrations

```bash
    dotnet ef migrations add MigrationName --context ApplicationDatabaseContext --project src/AnonymousStudentReviews.Infrastructure  --startup-project src/AnonymousStudentReviews.Api -o ./Data/Migrations/ApplicationDatabase 
```

Add ApplicationDatabase migrations

```bash
    dotnet ef migrations add MigrationName --context DataProtectionDatabaseContext --project src/AnonymousStudentReviews.Infrastructure  --startup-project src/AnonymousStudentReviews.Api -o ./Data/Migrations/DataProtectionDatabase 
```

Apply migrations

```txt
    Since docker is used, migrations are applied automatically on startup
    when the project is run in Development environment for better developer experience
```

```bash
    dotnet ef database update --project src/AnonymousStudentReviews.Infrastructure  --startup-project src/AnonymousStudentReviews.Api
```

## Running Tests

To run all tests, run the following command

```bash
  dotnet test
```

To run unit tests, run the following command

```bash
  dotnet test tests/AnonymousStudentReviews.UnitTests
```


To run integration tests, run the following command

```bash
  dotnet test tests/AnonymousStudentReviews.IntegrationTests
```

## Setting up Git Commit Hooks

To save consistent code style and proper test execution, run the following command

Install pre-commit

```bash
  pip install pre-commit
```

Install the hooks

```bash
  pre-commit install
  pre-commit install --hook-type pre-push
```