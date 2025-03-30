using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Swagger/OpenAPI support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

//// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // You can externalize these settings in appsettings.json or environment variables for security and flexibility.
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Externalize issuer
            ValidAudience = builder.Configuration["Jwt:Audience"],  // Externalize audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))  // Externalize secret key
        };
    });

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", builder =>
        builder.WithOrigins("http://localhost:4200")  // Specify the Angular app URL
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowLocalhost4200");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger middleware for the development environment
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

app.UseHttpsRedirection();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to endpoints
app.MapControllers();

app.Run();

// ✅ Make Program class partial for testing
public partial class Program { }
