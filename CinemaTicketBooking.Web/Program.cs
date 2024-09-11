using CinemaTicketBooking.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(
    typeof(CinemaTicketBooking.Application.MappingProfile),
    typeof(CinemaTicketBooking.Infrastructure.MappingProfile));

builder.Services.AddDbContext<CinemaTicketBookingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CinemaTicketDbContext")));

builder.Services.AddControllers();

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
