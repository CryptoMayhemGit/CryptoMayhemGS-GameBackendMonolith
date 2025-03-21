# Mayhem Web Api Description

Rest api is used for communication between the player and the server. The entire solution was programmed on the basis of documentation prepared by game designer Rafa³.
As a result of changes in the team, the initial assumptions were changed, so there was a lot of unused code left in the solution.

##### Modules (classes, interfaces etc) that are not used:
- guilds
- buildings
- attributes
- improvements
- items
- bonuses

##### Tech stack:
- MediatR - used as a mediator with behavior mechanisms
- FluentValidation - used to validate requests
- Serilog - used to logs information in files and console
- AutoMapper - used to map dto <-> tables
- HealthChech - used to check if everything works well
- Dapper - used for more complex queries
- JwtToken - authorization header 
- EntityFramewor - ORM
- Redis - as a cache
- Swagger - represent rest methods in UI
- ApplicationInsights - for controller telemetry
- NUnit - testing platform
- Moq - used in tests for mocking objects
- FluentAssertions - for better validation