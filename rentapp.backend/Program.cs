using Microsoft.EntityFrameworkCore;
using NLog;
using rentapp.backend.Authorization;
using rentapp.backend.Configuration;
using rentapp.backend.ErrorHandling;
using rentapp.BL.Core.Helpers;
using rentapp.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

{
    builder.Services.AddCors();
    builder.Services.AddControllers(options =>
    {
        //options.Filters.Add(typeof(GlobalExceptionHandlerAttribute));
    }).AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

  
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

    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
}


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

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

GlobalDiagnosticsContext.Set("SqlServerConnection", builder.Configuration.GetConnectionString("SqlServerConnection"));

app.Run();
