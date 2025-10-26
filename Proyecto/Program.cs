using Proyecto.Data;
using Proyecto.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
var builder = WebApplication.CreateBuilder(args);

// Base de datos
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<Database>(options => options.UseNpgsql(connectionString));

builder.Services.AddSingleton<CloudinaryService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new CloudinaryService(config);
});


// Limitar tamaño de archivos
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("myApp", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://127.0.0.1:5500");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("myApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
