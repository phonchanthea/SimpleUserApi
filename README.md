# Simple User API with Copilot

A simple ASP.NET Core Web API for managing users.

## Features

- Create users
- Read users
- Update users
- Delete users
- Input validation
- Duplicate email detection
- Request logging middleware
- Swagger API documentation

## Technologies

- C#
- ASP.NET Core
- .NET 8
- Swagger/OpenAPI
- GitHub Copilot

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | /api/users | Get all users |
| GET | /api/users/{id} | Get a user |
| POST | /api/users | Create a user |
| PUT | /api/users/{id} | Update a user |
| DELETE | /api/users/{id} | Delete a user |

## Validation

The API validates:

- User name is required
- User name must contain at least two characters
- Email must be valid
- Duplicate email addresses are rejected

## Middleware

A custom request logging middleware records:

- HTTP method
- Request path
- Response status code
- Request processing time

## GitHub Copilot

GitHub Copilot was used during development to assist with
debugging, code review, validation, and improving the API.

## Running the Application

```bash
dotnet restore
dotnet run
