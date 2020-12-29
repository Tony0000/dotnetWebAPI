# DotNET WebAPI

This is a dotnet 3.2 web api base project with basic endpoints such as Login and User CRUD.

This is intended for example purposes only and should be further tweaked for staging or production.

## Features
You can find the full list of features bellow:
 - Repository pattern
 - Sync and async methods
 - Custom Action filters:
    - Model Validation filter
    - Not Found filter
 - Custom Action Result for errors
 - Data transfer object (DTO)
 - Token Jwt with default expiration in 24 hours

## Requirements 
 Have SQLEXPRESS 2013 or above installed on the machine (edit ConnectionString in appsettings.json accordingly). 
 The Startup class will run all pending migrations so there's no need to 'update-database' through nuget console. 
 Also, table Users will be seeded with some records after first launch.
