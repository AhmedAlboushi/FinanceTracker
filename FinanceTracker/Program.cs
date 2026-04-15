using CloudinaryDotNet;
using FinanceTracker.Data;
using FinanceTracker.Guards;
using FinanceTracker.Helpers;
using FinanceTracker.IRepository;
using FinanceTracker.IService;
using FinanceTracker.Middleware;
using FinanceTracker.Repository;
using FinanceTracker.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var cloudinaryAccount = new Account(
    builder.Configuration["Cloudinary:CloudName"],
    builder.Configuration["Cloudinary:ApiKey"],
    builder.Configuration["Cloudinary:ApiSecret"]
);
builder.Services.AddSingleton(new Cloudinary(cloudinaryAccount));




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // The name of the HTTP header where the token will be sent.
        Name = "Authorization",


        // Indicates this is an HTTP authentication scheme.
        Type = SecuritySchemeType.Http,


        // Specifies the authentication scheme name.
        // Must be exactly "Bearer" for JWT Bearer tokens.
        Scheme = "Bearer",


        // Optional metadata to describe the token format.
        BearerFormat = "JWT",


        // Specifies that the token is sent in the request header.
        In = ParameterLocation.Header,


        // Text shown in Swagger UI to guide the user.
        Description = "Enter: Bearer {your JWT token}"
    });


    // This tells Swagger that endpoints protected by [Authorize]
    // require the Bearer token defined above.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                // Reference the previously defined "Bearer" security scheme.
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },


   
            new string[] {}
        }
    });
});






builder.Services.AddDbContext<FinanceTrackerDbContext>(options =>

    options.UseNpgsql(builder.Configuration.GetConnectionString("FinanceTrackerLocalDb")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("FinanceTrackerCorsPolicy", policy =>
    {
        policy
            .WithOrigins(
            "https://financetracker-project.vercel.app", //Frontend Server
                "http://localhost:5173", // React frontend
                "https://localhost:7217",
                "http://localhost:5215"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();

    });
});

// UTC Time handler
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TokenValidationParameters define how incoming JWTs will be validated.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Ensures the token was issued by a trusted issuer.
            ValidateIssuer = true,


            // Ensures the token is intended for this API (audience check).
            ValidateAudience = true,


            // Ensures the token has not expired.
            ValidateLifetime = true,


            // Ensures the token signature is valid and was signed by the API.
            ValidateIssuerSigningKey = true,


            // The expected issuer value (must match the issuer used when creating the JWT).
            ValidIssuer = "FinanceTracker",


            // The expected audience value (must match the audience used when creating the JWT).
            ValidAudience = "FinanceTrackerUsers",


            // The secret key used to validate the JWT signature.
            // This must be the same key used when generating the token.
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET_KEY"]))


        };
    });

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        await LogHelper.WriteLog(context.HttpContext, "RateLimit", (short)429, 3);
    };
    options.AddPolicy("AuthLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 4,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });

    options.AddPolicy("GeneralLimiter", httpContext =>
    {
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: userId,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 250,
                Window = TimeSpan.FromMinutes(2),
                QueueLimit = 0
            });
    });
});




builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IWalletRoleGuard, WalletRoleGuard>();
builder.Services.AddScoped<IAuthService, AuthService>();



builder.Services.AddScoped<IUserWalletsRepository, UserWalletsRepository>();

builder.Services.AddScoped<IWalletsRepository, WalletsRepository>();
builder.Services.AddScoped<IWalletsService, WalletService>();

builder.Services.AddScoped<IIncomeSourcesRepository, IncomeSourcesRepository>();
builder.Services.AddScoped<IIncomeSourcesService, IncomeSourcesService>();


builder.Services.AddScoped<IIncomesRepository, IncomesRepository>();
builder.Services.AddScoped<IIncomesService, IncomesService>();

builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

builder.Services.AddScoped<IExpensesRepository, ExpensesRepository>();
builder.Services.AddScoped<IExpensesService, ExpensesService>();

builder.Services.AddScoped<IWalletGoalsRepository, WalletGoalsRepository>();
builder.Services.AddScoped<IWalletGoalsService, WalletGoalsService>();

builder.Services.AddScoped<IBlockedUsersRepository, BlockedUsersRepository>();
builder.Services.AddScoped<IBlockedUsersService, BlockedUsersService>();

builder.Services.AddScoped<IFriendshipsRepository, FriendShipsRepository>();
builder.Services.AddScoped<IFriendshipsService, FriendshipsService>();

builder.Services.AddScoped<IChatMessagesRepository, ChatMessagesRepository>();
builder.Services.AddScoped<IChatMessagesService, ChatMessagesService>();

builder.Services.AddScoped<IBugReportsRepository, BugReportsRepository>();
builder.Services.AddScoped<IBugReportsService, BugReportsService>();

builder.Services.AddAuthorization();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddSingleton<IActiveUserTrackerService, ActiveUserTrackerService>();



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


var app = builder.Build();

// Only Runs Locally
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FinanceTrackerCorsPolicy");

app.UseRateLimiter();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();
app.Run();
