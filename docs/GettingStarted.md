# Getting started

## Setting up the environment

### Install the dependencies

- [.NET8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16)

### Install the .NET tools listed in the manifest

```bash
$ dotnet tool restore
```

## Running the project locally

### Create a database

The project needs an SQL Server database. You can either create an
[Azure SQL](https://azure.microsoft.com/en-us/products/azure-sql/database) database,
or install the [SQL Server 2022 Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
and create a database on your local machine.

### Create your `appsettings.{Environment}.json` file

For local development replace the `{Environment}` with `Development`.
Configure the connection string of your database in this file:

```json
{
    "ConnectionStrings": {
        "CinemaTicketBooking": "Server=myServer;Database=myDatabase;Trusted_Connection=True;"
    }
}
```

See [ASP.NET Core documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-9.0#appsettingsjson) for more details.

### Seed the database with demo data

This step is optional, but makes trying out the API endpoints possible without having to
create test data in database. In order to achieve this, you can either create your
database by importing a `.bacpac` file, or you can run the `CinemaTicketBooking.Utilities project`
with the `seed_database` command and seed data from a JSON file.
You can find a suitable `.bacpac` and a JSON file in the `configurations/DatabaseSeeding` folder.
If you want to seed your database with JSON data, please make sure that the schema of the database is created
by running the [migration command](#applying-migrations-to-the-database) first.

### Run the project

```bash
# Executed in the CinemaTicketBooking.Web folder
$ dotnet run
```

Open the URL contained by the log messages, for example: `Now listening on: https://localhost:50721`.
You can start exploring the API with the [Swagger UI](https://swagger.io/tools/swagger-ui/).
Just add `/swagger` to the URL in your browser.

## Working with database migrations

[Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) represent changes to the database schema.

### Creating database migration files

```bash
# Executed in the CinemaTicketBooking.Infrastructure folder
$ dotnet ef migrations add MIGRATION_NAME --startup-project ../CinemaTicketBooking.Web
```

### Applying migrations to the database

```bash
# Executed in the CinemaTicketBooking.Infrastructure folder
$ dotnet ef database update --connection CONNECTION_STRING --startup-project ../CinemaTicketBooking.Web
```
