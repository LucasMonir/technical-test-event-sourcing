# TechnicalTest - Fake Blog Api
> A small CQRS/Hexagonal architecture project + This version is my attempt at EventSourcing

## Requirements:
> .NET SKD 10.0.X
> dotnet-ef tooling
> Docker
> Optional: SQLite3 CLI or any SQL database explorer
> Personal preference: Visual Studio 2022 (For development and local testing)
> Optional: Postman or insomnia, for testing the endpoints

## Migrating the Database:
- To migrate the database and ensure the database is updated, use the following command

> dotnet ef database update --project TechnicalTest.Infrastructure --startup-project TechnicalTest.Api

## Running in VisualStudio
- Select the startup method to use the dockerfile present in the project
- In development environment you will be presented with the SwaggerUI with the available endpoints and tryout examples
- The Launch URL is: https://localhost:{port}}/swagger/index.html (Development only), the current port can be seen in launchsettings.json

## GET/POST Request (launching via docker)
> get requests on https://localhost:32779/Post/{guid}
> post requests on https://localhost:32779/Post/ with json body

## Creating image and running this as a docker container 
> To run the published application, use the command docker-compose up --build in the solution's root folder
> The ports used are standard 8080/8081 (HTTP/HTTPS)

## Running the tests outside Visual Studio
> dotnet restore
> dotnet test --collect:"XPlat Code Coverage" 

## Running inside Visual Studio
> Install the Fine Code Coverage tool (Personal preference)
> Run all the tests in the solution through Test Explorer
> Check the results via Fine Code Coverage for coverage per project or Summary to see the total

### Other info:
> This version of the application uses a CQRS + Repository pattern architecture with event sourcing
> there is a database as source of truth version as well, which i'm more familiar with.