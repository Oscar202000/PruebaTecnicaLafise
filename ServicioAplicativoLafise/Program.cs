using Microsoft.EntityFrameworkCore;
using Servicio.Lafise.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Aplicativo.Lafise", policy =>
        policy.WithOrigins("https://localhost:7131")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddDbContext<GestionBdContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Aplicativo.Lafise");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
