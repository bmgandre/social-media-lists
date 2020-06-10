# Social Media Lists 

# Description

This project provides three applications. Two of them, the console application and the web API will populate databases with fake data. The web App is a front-end for the API for searching posts.

## Sample.ConsoleApplication
Sample.ConsoleApplication is the playground for rapid experimentation and testing.
The console application will populate both relational and non-relational database and outputs the result of a query.

## WebApi
The WebApi is a REST application with OpenApi standards. On the first run, it will populate the databases in the same fashion that the console application do. This application also provides the Swagger UI on /swagger endpoint.

## WebApp
This is a simple front-end for querying posts. The project consists of a ASP.NET core application that hosts a Angular application, though it is possible to start it using the conventional Angular CLI tools.


# Build requirements

## .NET Core 3.1 SDK
https://dotnet.microsoft.com/download

## Entity Framework Core tools reference - .NET CLI
```
dotnet tool install --global dotnet-ef
```

## Node.js and npm
https://nodejs.org/en/download/


# Development environment requirements
## Visual Studio 2019 with the following workloads installed
- ASP.NET and web development
- Node.js development
- .NET Core cross-platform development

## Elasticsearch 7.7
https://www.elastic.co/downloads/elasticsearch

# Running

All project can run on Windows and Linux on command line. For debugging, at the time of this writing, Visual Studio 2019 is the best option, since VS Code tools for .NET requires third party extensions with no support for SpecFlow (cucumber for .Net).

## Running on command line

### Test projects
```
cd src
dotnet restore
dotnet build
dotnet test --project Test.Unit
dotnet test --project Test.Integration
```

### Sample.ConsoleApplication
```
cd src
dotnet restore
dotnet build
dotnet run --project Sample.ConsoleApplication
```

## WebApi
On command line it will start listening on port 5000. Go to http://localhost:5000/swagger to interact with it using Swagger UI.
```
cd src
dotnet restore
dotnet build
dotnet run --project WebApi
```

## WebApp
To start the application using ASP.NET host application:
```
cd src
dotnet restore
dotnet build
dotnet run --project WebApp
```

If you prefer Angular CLI tools:
```bash
cd src/WebApp/ClientApp
npm install
export PATH=$(pwd)/node_modules/.bin:$PATH
ng serve --watch # --watch for hot reloading
```

## Running on Visual Studio
### Test project
- Open the Test Explorer and click Run All option.

### All other projects
- Select the appropriated project and click to start. 
- If you need to run and debug multiple projects, right click on the solution and select Start Multiple Projects option.
