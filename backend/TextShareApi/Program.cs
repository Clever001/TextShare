using System.Text;
using System.Text.Json.Serialization;
using System.Transactions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using TextShareApi.Attributes;
using TextShareApi.Data;
using TextShareApi.Extensions;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;
using TextShareApi.Repositories;
using TextShareApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options => {
        options.Filters.Add<ValidateModelStateAttribute>();
        if (bool.Parse(builder.Configuration["LogExecutionTime"] ?? ""))
            options.Filters.Add<LogExecutionTimeFilter>();
    }).ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
    .AddJsonOptions(options => { 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// builder.Services.AddGrpcClient<Greeter.GreeterClient>(options => {
//     options.Address = new Uri(builder.Configuration["GrpcServices:Auth"]
//         ?? throw new InvalidConfigurationException());
// });

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IFriendPairRepository, FriendPairRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IHashSeedRepository, HashSeedRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITextRepository, TextRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<ITextSecurityService, TextSecurityService>();
builder.Services.AddScoped<ITextService, TextService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUniqueIdService, UniqueIdService>();
builder.Services.AddScoped<PasswordHasher<AppUser>>();

builder.Logging.AddConsole();

builder.Services.AddHostedService<TextCleanupService>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        if (builder.Environment.IsDevelopment()) {
            policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
        }
        policy.WithOrigins("http://localhost")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
var app = builder.Build();

// using (var scope = app.Services.CreateScope()) {
//     var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     if ((await dbContext.Database.GetPendingMigrationsAsync()).Any()) 
//         await dbContext.Database.MigrateAsync();
// }

app.UseCors("AllowAll");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
    app.MapScalarApiReference(options => {
        options.WithTitle("TextShare API");
    });
    app.Use(async (context, next) => {
        if (context.Request.Path == "/") {
            context.Response.Redirect("/scalar/v1");
            return;
        }

        await next();
    });
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();