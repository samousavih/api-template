# User API Dev Guide

## Building
### Local env
To build and run the api in local environment,

1) Run the Api using docker compose, 
    ```
    docker-compose up
    ```
    This should spin up the api and a postgres db instance using docker.
2) The Api would be accessible on port 8080 by default

## Testing
### Unit tests
All of the unit tests are added in ``Api.UnitTests`` project. More unit tests would be needed for the followings:
1) Handlers
2) Mappers
3) Validators
### Integration tests
All of the integration tests are added in ``Api.IntegrationTests`` project. These tests can be run without a database and they mock repositories.
More integration tests would be needed for the followings:
1) One exception scenario per each endpoint 
### End to end tests
Using docker compose e2e tests can also be setup, which I skipped in the interest of time.


## Deploying
To deploy the Api in production using docker ``dockerfile`` in the root can be used. However, more configs needs to be added depending on CI/CD pipeline. 

## Logging
Atm logging is minimum. only ``ExceptionMiddle`` logs any unhandled exceptions.

## Api documentation
Swagger docs should be added but I skipped that in the interest of time


