using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NLog;
using rentapp.backend.Configuration;
using rentapp.backend.ErrorHandling;
using rentapp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();

#region Db
builder.Services.AddDbContext<DataContext>(option =>
{
    if (builder.Environment.IsDevelopment())
    {
        option.EnableSensitiveDataLogging();
    }

    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"), options =>
    {
        options.CommandTimeout(45);
        options.EnableRetryOnFailure();
    });
});
#endregion

builder.Services.AddServices();

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(GlobalExceptionHandlerAttribute));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

GlobalDiagnosticsContext.Set("SqlServerConnection", builder.Configuration.GetConnectionString("SqlServerConnection"));

app.Run();
