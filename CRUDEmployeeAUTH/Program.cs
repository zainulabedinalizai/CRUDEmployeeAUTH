using CrudEmployeeAUTH.Repositories;
using CRUDEmployeeAUTH.ActionFilters;
using CRUDEmployeeAUTH.Data;
using CRUDEmployeeAUTH.IRepositories;
using CRUDEmployeeAUTH.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EmployeeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repos
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmployeTRepository, EmployeeTRepository>();
builder.Services.AddScoped<IEmployeeStoredProcedureRepository, EmployeeStoredProcedureRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Action Filters
builder.Services.AddMemoryCache();
builder.Services.AddScoped<CacheFilter>(sp =>
{
    var cache = sp.GetRequiredService<IMemoryCache>();
    var logger = sp.GetRequiredService<ILogger<CacheFilter>>();
    return new CacheFilter(60, cache, logger);
});
builder.Services.AddScoped<LoggingFilter>();
builder.Services.AddScoped<ExceptionHandlingFilter>();
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddScoped<ResultFilter>();
builder.Services.AddScoped<CustomAuthorizationFilter>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<ReviewLeaveFilter>();

// JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SystemAdmin", policy => policy.RequireRole("SystemAdmin"));
    options.AddPolicy("CompanyAdminOrSystemAdmin", policy => policy.RequireRole("CompanyAdmin", "SystemAdmin"));
    options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    c.OperationFilter<UserIdHeaderParameter>();
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// UserIdMiddleware
app.UseMiddleware<UserIdMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();