using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rentapp.backend.Configuration;
using rentapp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

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

app.Run();
