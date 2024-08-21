var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(CinemaTicketBooking.Application.MappingProfile));
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();
