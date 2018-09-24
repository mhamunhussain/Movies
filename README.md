# Movies Api

## Packages 

The following packages were used to help build this API. 
- AutoMapper
- EF Core
- Fluent Assertions 
- Xunit
- Moq

## Developer Notes

Due to time constraints and upon reflection there were a few things I would have approached differently. 

- Implemented a *UserService* and *MovieService*. Currently the repositories are being injected directly into the controller. With a bit more time, a service class could have been used to help maintain this better.
- Component tests in have a lot of data setup within the test, this should be broken out into helper methods to make the tests read better
- Missing unit tests for the repostories. I decided a better use of the time I had was to create component tests using [TestServer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.testhost.testserver?view=aspnetcore-2.0)