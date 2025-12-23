using Microsoft.EntityFrameworkCore;
using MyMovie.Application.Interfaces;
using MyMovie.Application.Services;
using MyMovie.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient(); 

builder.Services.AddDbContext<MoviesDbContext>(options=>
options.UseSqlServer( //koristim SQL server kao bazu podataka
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("MyMovie.Infrastructure"))); // migracije su u MyFood.Infrastructure

builder.Services.AddScoped<IMovieRepository, MovieSqlRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddHttpClient<IMovieExternalService, TmdbService>();

// CORS za frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReact");

app.UseCors("AllowAll");

app.MapControllers(); 

app.Run();
