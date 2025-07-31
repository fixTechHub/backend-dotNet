using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiDotNet.Data;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Repository;
using WebApiDotNet.Services;
using WebApiDotNet.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Thêm dòng này nếu chưa có

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();

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
builder.Services.AddScoped<IActionLogRepository, ActionLogRepository>();
builder.Services.AddScoped<IFinancialReportRepository, FinancialReportRepository>();

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
builder.Services.AddScoped<ActionLogService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin() // Cho phép tất cả các nguồn gốc
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
// app.UseHttpsRedirection(); // This is not needed behind Render's proxy
app.UseActionLogging(); // Add action logging middleware
app.UseAuthorization();
app.MapControllers();
app.Run();
