using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Data;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

//app.MapGet("/helloWorld", () =>
//{
//    return "Hello World!";
//});

//app.MapGet("/responseType", () =>
//{
//    return Results.BadRequest("Bad Request");
//});

//app.MapPost("/withParameters/{parameter:int}", (int parameter) =>
//{
//    return Results.Ok($"Parametro recibido {parameter}");
//});

app.MapGet("/api/propiedades", () =>
{
    return Results.Ok(DatosPropiedad.Propiedades);
});

app.MapGet("/api/propiedades/{id:int}", (int id) =>
{
    var propiedad = DatosPropiedad.Propiedades.FirstOrDefault(p => p.IdPropiedad == id);
    if (propiedad == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(propiedad);
});

app.MapPost("/api/propiedades", ([FromBody] Propiedad propiedad) =>
{
    if (propiedad == null || propiedad.IdPropiedad != 0 || string.IsNullOrWhiteSpace(propiedad.Nombre))
    {
        return Results.BadRequest();
    }

    if(DatosPropiedad.Propiedades.FirstOrDefault(p => p.Nombre.ToLower() == propiedad.Nombre.ToLower()) != null)
    {
        return Results.Conflict("Ya existe una propiedad con ese nombre.");
    }
    if (string.IsNullOrWhiteSpace(propiedad.Descripcion) || string.IsNullOrWhiteSpace(propiedad.Ubicacion))
    {
        return Results.BadRequest("La descripción y la ubicación son obligatorias.");
    }
    if (propiedad.FechaCreacion == null)
    {
        propiedad.FechaCreacion = DateTime.UtcNow;
    }

    propiedad.IdPropiedad = DatosPropiedad.Propiedades.Max(p => p.IdPropiedad) + 1;

    DatosPropiedad.Propiedades.Add(propiedad);
    return Results.Created($"/api/propiedades/{propiedad.IdPropiedad}", propiedad);
});

app.MapPut("/api/propiedades/{id:int}", (int id, [FromBody] Propiedad propiedad) =>
{
    var existingPropiedad = DatosPropiedad.Propiedades.FirstOrDefault(p => p.IdPropiedad == id);
    if (existingPropiedad == null)
    {
        return Results.NotFound();
    }
    existingPropiedad.Nombre = propiedad.Nombre;
    existingPropiedad.Descripcion = propiedad.Descripcion;
    existingPropiedad.Ubicacion = propiedad.Ubicacion;
    existingPropiedad.Activa = propiedad.Activa;
    existingPropiedad.FechaCreacion = propiedad.FechaCreacion;
    return Results.Ok(existingPropiedad);
});

app.MapPatch("/api/propiedades/{id:int}", (int id, [FromBody] Propiedad propiedad) =>
{
    var existingPropiedad = DatosPropiedad.Propiedades.FirstOrDefault(p => p.IdPropiedad == id);
    if (existingPropiedad == null)
    {
        return Results.NotFound();
    }
    existingPropiedad.Nombre = propiedad.Nombre;
    existingPropiedad.Descripcion = propiedad.Descripcion;
    existingPropiedad.Ubicacion = propiedad.Ubicacion;
    existingPropiedad.Activa = propiedad.Activa;
    existingPropiedad.FechaCreacion = propiedad.FechaCreacion;
    return Results.Ok(existingPropiedad);
});

app.MapDelete("/api/propiedades/{id:int}", (int id) =>
{
    var propiedad = DatosPropiedad.Propiedades.FirstOrDefault(p => p.IdPropiedad == id);
    if (propiedad == null)
    {
        return Results.NotFound();
    }
    DatosPropiedad.Propiedades.Remove(propiedad);
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.Run();