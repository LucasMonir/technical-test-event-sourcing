# TechnicalTest - Fake Blog Api
- A small CQRS/Hexagonal architecture project + This version is my attempt at EventSourcing!
- Alternatively: There is repository with a "Database as Source of Truth" version as well -> <a href="https://github.com/LucasMonir/Technical-test-no-event-sourcing">here</a>.

## Requirements:
- .NET SDK 10.0.X
- dotnet-ef tooling
- Docker
- Optional: SQLite3 CLI or any SQL database explorer
- Personal preference: Visual Studio 2022 (For development and local testing)
- Optional: Postman or insomnia, for testing the endpoints

## Migrating the Database:
- To migrate the database and ensure the database is updated, use the following command

- dotnet ef database update --project TechnicalTest.Infrastructure --startup-project TechnicalTest.Api

# Running The App

## Running container in visual studio + browser
- Select the startup method to use the dockerfile present in the project
- In development environment you will be presented with the SwaggerUI with the available endpoints and tryout examples
- The Launch URL is: https://localhost:{port}/swagger/index.html (Development only), the current port can be seen in launchsettings.json

## GET/POST Request - Docker compose up
- get requests on https://localhost:{port}/post/{id}
	Endpoint also allows the "includeAuthor" boolean flag if the user wants to see the author information:
	https://localhost:{port}/post/{id}?includeAuthor=true

- post requests on https://localhost:{port}/post with json body
	Info: Currently due to the simple structure of the app, there are 2 possibilities of creation which I have decided to do:
	* 1 - With a new author; sending the Name/Surname of the author will create a new user in the database
	* 2 - With existing author: sending only the authorId will look for existing authors in the database!

## Building image and running as a docker container 
- To run the published application, use the command `docker-compose up --build` in the solution's root folder

- The ports used are standard 8080/8081 (HTTP/HTTPS) when running on docker, but can be changed in the docker-compose file

# Testing
- The application targets at least 90% test coverage

- Coverage information obtained through the "Fine Code Coverage" extension points to 96% of Line Coverage and 90% of Branch Coverage

## Running the tests outside Visual Studio
- dotnet restore
- dotnet test --collect:"XPlat Code Coverage" 

## Running inside Visual Studio
- Install the Fine Code Coverage tool (Personal preference)

- Run all the tests in the solution through Test Explorer

- Check the results via Fine Code Coverage for coverage per project or Summary to see the total

### Other info:
- This version of the application uses a CQRS + Repository pattern architecture with event sourcing using projection from events to present data

- The event sourcing implementation was custom made

## Pending improvements - Important notes:
- ProjectionWorkers should be a generic handler to avoid concurrency in the projections

- Logging with Log4Net or Serilog must be added

- <span style="color:red">Db polling shall be switched to push model or using event queues or applying Lazy Projections, constant hits to database are a waste of 
 computing even in small scale apps, and the current implementation is just for demonstration purposes, at the moment the choice made was to have 
 the background workers polling and projecting events beforehand to speed up GETS. Not a wise choice for a real production app</span>

- Upgrading this system to use RabbitMq or Confluent with protobuf would be my choice in a refactor.