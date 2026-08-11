using SimpleUserApi.Middleware;
using SimpleUserApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add custom logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// In-memory user database
var users = new List<User>
{
    new User
    {
        Id = 1,
        Name = "Alice",
        Email = "alice@example.com"
    },
    new User
    {
        Id = 2,
        Name = "Bob",
        Email = "bob@example.com"
    }
};

// GET: Get all users
app.MapGet("/api/users", () =>
{
    return Results.Ok(users);
});

// GET: Get user by ID
app.MapGet("/api/users/{id:int}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);

    if (user == null)
    {
        return Results.NotFound(new
        {
            message = "User not found"
        });
    }

    return Results.Ok(user);
});

// POST: Create a new user
app.MapPost("/api/users", (User user) =>
{
    if (!IsValidUser(user))
    {
        return Results.BadRequest(new
        {
            message = "Name and a valid email are required."
        });
    }

    if (users.Any(u =>
        u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
    {
        return Results.Conflict(new
        {
            message = "A user with this email already exists."
        });
    }

    user.Id = users.Count == 0
        ? 1
        : users.Max(u => u.Id) + 1;

    users.Add(user);

    return Results.Created($"/api/users/{user.Id}", user);
});

// PUT: Update an existing user
app.MapPut("/api/users/{id:int}", (int id, User updatedUser) =>
{
    if (!IsValidUser(updatedUser))
    {
        return Results.BadRequest(new
        {
            message = "Name and a valid email are required."
        });
    }

    var user = users.FirstOrDefault(u => u.Id == id);

    if (user == null)
    {
        return Results.NotFound(new
        {
            message = "User not found"
        });
    }

    user.Name = updatedUser.Name;
    user.Email = updatedUser.Email;

    return Results.Ok(user);
});

// DELETE: Delete a user
app.MapDelete("/api/users/{id:int}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);

    if (user == null)
    {
        return Results.NotFound(new
        {
            message = "User not found"
        });
    }

    users.Remove(user);

    return Results.Ok(new
    {
        message = "User deleted successfully"
    });
});

// Validate user information
static bool IsValidUser(User user)
{
    return !string.IsNullOrWhiteSpace(user.Name)
        && user.Name.Length >= 2
        && !string.IsNullOrWhiteSpace(user.Email)
        && new System.ComponentModel.DataAnnotations.EmailAddressAttribute()
            .IsValid(user.Email);
}

app.Run();