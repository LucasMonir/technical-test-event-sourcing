# TechnicalTest - Fake Blog Api
> A small CQRS/Hexagonal architecture project

## Requirements:
- .NET SKD 10.0.X
- dotnet-ef tooling
- Optional: SQLite3 CLI or any SQL database explorer
- Docker

## Migrating the Database:
- To migrate the database, use the following command

>dotnet ef database update \
>  --project TechnicalTest.Infrastructure.Persistence \
>  --startup-project TechnicalTest.Api

## Running in VisualStudio
- Select the startup method to use the dockerfile present in the project