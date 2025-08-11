using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiDotNet.Data;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Repository;
using WebApiDotNet.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Thêm dòng này nếu chưa có

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<ICouponUsageRepository, CouponUsageRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ISystemReportRepository, SystemReportRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ITechnicianRepository, TechnicianRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IWarrantyRepository, WarrantyRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ICommissionConfigRepository, CommissionConfigRepository>();
builder.Services.AddScoped<IFinancialReportRepository, FinancialReportRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<ICouponUsageService, CouponUsageService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISystemReportService, SystemReportService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IUserService, UserService>();    
builder.Services.AddScoped<ITechnicianService, TechnicianService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IWarrantyService, WarrantyService>();
builder.Services.AddScoped<IServiceService, ServiceService>();  
builder.Services.AddScoped<ICommissionConfigService, CommissionConfigService>();
builder.Services.AddScoped<IFinancialReportService, FinancialReportService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Authentication: configure JWT from appsettings:Jwt
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection.GetValue<string>("Key");
var jwtIssuer = jwtSection.GetValue<string>("Issuer");
var jwtAudience = jwtSection.GetValue<string>("Audience");

// Add logging to debug JWT configuration
Console.WriteLine($"🔐 JWT Configuration:");
Console.WriteLine($"  Key: {(string.IsNullOrEmpty(jwtKey) ? "❌ NULL" : "✅ SET")}");
Console.WriteLine($"  Issuer: {jwtIssuer}");
Console.WriteLine($"  Audience: {jwtAudience}");

if (!string.IsNullOrWhiteSpace(jwtKey))
{
    Console.WriteLine("✅ JWT Authentication configured successfully");
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero
            };

            // Read JWT from HttpOnly cookie as fallback
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (string.IsNullOrEmpty(context.Token))
                    {
                        var cookieName = builder.Configuration.GetValue<string>("Jwt:AccessTokenCookieName") ?? "AuthToken";
                        if (context.Request.Cookies.TryGetValue(cookieName, out var token))
                        {
                            context.Token = token;
                        }
                    }
                    return Task.CompletedTask;
                }
            };
        });
}
else
{
    Console.WriteLine("❌ JWT Authentication NOT configured - missing key");
    Console.WriteLine("⚠️  All [Authorize] attributes will be ignored!");
    Console.WriteLine("💡 Check Render Environment Variables for JWT__Key");
    Console.WriteLine("💡 Or check if appsettings.Production.json exists and has JWT configuration");
}

// Authorization (basic)
builder.Services.AddAuthorization();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "http://localhost:3001",
                "https://front-pcw3xl50l-kds-projects-ce66334c.vercel.app",
                "https://front-end-git-dat-kds-projects-ce66334c.vercel.app",
                "https://fixtech.id.vn/api"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebApiDotNet", Version = "v1" });
    var jwtSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer {your token}",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // In production, still enable Swagger for API documentation
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
