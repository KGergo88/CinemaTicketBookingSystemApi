# Setting up the project

## Installing the .NET tools listed in the manifest

```bash
$ dotnet tool restore
```

# Working with database migrations

## Creating database migration files

```bash
# Executed in the CinemaTicketBooking.Infrastructure folder
$ dotnet ef migrations add MIGRATION_NAME --startup-project ../CinemaTicketBooking.Web
```

## Applying migrations to the database
```bash
# Executed in the CinemaTicketBooking.Infrastructure folder
$ dotnet ef database update --startup-project ../CinemaTicketBooking.Web
```
