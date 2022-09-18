using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using rentapp.backend.Authorization;
using rentapp.backend.Configuration;
using rentapp.BL.Core.Helpers;
using rentapp.BL.Entities;
using rentapp.BL.MappingConfigurations;
using rentapp.Data;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    builder.Services.AddControllers(options =>
    {
        //options.Filters.Add(typeof(GlobalExceptionHandlerAttribute));
    });

    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationClientsideAdapters();
    builder.Services.AddRouting(options => options.LowercaseUrls = true);
    builder.Services.AddHttpContextAccessor();

    #region Db
    builder.Services.AddDbContext<DataContext>(option =>
    {
        if (builder.Environment.IsDevelopment())
        {
            option.EnableSensitiveDataLogging();
        }

        option.UseInMemoryDatabase("testDb");

        //option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"), options =>
        //{
        //    options.CommandTimeout(45);
        //    options.EnableRetryOnFailure();
        //});
    });
    #endregion

    builder.Services.AddServices();

    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
}


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(EntityMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// add hardcoded test user to db on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

    var directoryTest = new rentapp.BL.Entities.Directory()
    {
        DirectoryId = 1,
        City = "Chivilcoy",
        State = "Buenos Aires",
        Number = "230",
        Name = "Agustin",
        PlainCity = "chivilcoy",
        PlainStreet = "sarmiento",
        Country = "Argentina",
        PlainCountry = "argentina",
        Street = "Sarmiento",
        Unit = "i",
        PlainZipCode = "6620",
        ZipCode = "6620",
        Floor = "2",
        PlainState = "buenosiares",
        IsActive = true,
        DocumentTypeId = 1,
        DocumentNumber = "20379768068",
        CreatedUserId = 1
    };

    context.Directories.Add(directoryTest);
    context.SaveChanges();

    var customerTest = new Customer()
    {
        CustomerId = 1,
        DocumentNumber = "37976806",
        Name = "Agustin",
        LastName = "Yuse",
        PhoneNumber = "2227534620",
        DocumentTypeId = 1,
        Email = "agustinyuse@gmail.com",
        IsActive = true,
        CreatedUserId = 1
    };

    context.Customers.Add(customerTest);
    context.SaveChanges();

    var customerAddress = new CustomerAddress()
    {
        City = "Chivilcoy",
        State = "Buenos Aires",
        Number = "230",
        PlainCity = "chivilcoy",
        PlainStreet = "sarmiento",
        Country = "Argentina",
        PlainCountry = "argentina",
        Street = "Sarmiento",
        Unit = "i",
        PlainZipCode = "6620",
        ZipCode = "6620",
        Floor = "2",
        PlainState = "buenosiares",
        IsActive = true,
        CustomerId = 1,
        CreatedUserId = 1
    };

    context.CustomerAddresses.Add(customerAddress);
    context.SaveChanges();

    var testUser = new User
    {
        UserId = 1,
        Email = "admin",
        CustomerId = 1,
        Password = BCrypt.Net.BCrypt.HashPassword("test"),
        IsActive = true
    };
    context.Users.Add(testUser);
    context.SaveChanges();
}

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
