using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(
    typeof(CinemaTicketBooking.Infrastructure.MappingProfile),
    typeof(CinemaTicketBooking.Web.MappingProfile));

var connectionStringName = "CinemaTicketBooking";
var dbConnectionString = builder.Configuration.GetConnectionString(connectionStringName);
if (dbConnectionString is null)
    throw new StartupException($"Could not load the connection string: \"{connectionStringName}\"");

var databaseBinding = DatabaseBindingFactory.Create(dbConnectionString);
builder.Services.AddDbContext<CinemaTicketBookingDbContext>(
    options => databaseBinding.SetDatabaseType(options, dbConnectionString));

builder.Services.AddControllers(options =>
    options.Filters.Add<UnhandledExceptionFilter>());

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cinema Ticket Bookin System API",
        Description = "",
        Version = "v1",
        TermsOfService = null,
        Contact = null,
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://github.com/KGergo88/CinemaTicketBookingSystemApi/blob/main/license")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();
