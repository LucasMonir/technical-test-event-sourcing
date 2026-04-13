# TechnicalTest - Fake Blog Api
> A small CQRS/Hexagonal architecture project + This version is my attempt at EventSourcing!
> Alternatively: There is repository with a "Database as Source of Truth" version as well -> <a href="https://github.com/LucasMonir/Technical-test-no-event-sourcing">here</a>.

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
	Endpoint also allows the "includeAuthor" boolean flag if the user wants to see the authjor information:
	https://localhost:32779/Post/{guid}?includeAuthor=true

> post requests on https://localhost:32779/Post/ with json body
	Info: Currently due to the simple structure of the app, there are 2 possibilities of creation which I have decided to do:
	1 - With a new author; sending the Name/Surname of the author will create a new user in the database
	2 - With existing author: sending only the authorId will look for existing authors in the database!


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
> This version of the application uses a CQRS + Repository pattern architecture with event sourcing using projection from events to present data
> The eventsourcing of the project was custom made