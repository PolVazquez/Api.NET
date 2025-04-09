using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Models;
using MinimalAPI.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MinimalAPI.AutoMapper.MapperDTOs));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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


app.MapGet("/api/propiedades", async (ApplicationDbContext _db, ILogger<Program> logger) =>
{

    logger.LogInformation("Se ha solicitado la lista de propiedades.");

    ApiResponse response = new ApiResponse
    {
        Success = true,
        Result = _db.Propiedades,
        StatusCode = System.Net.HttpStatusCode.OK
    };

    return Results.Ok(response);
}).WithName("GetPropiedades").Produces<ApiResponse>(200);

app.MapGet("/api/propiedades/{id:int}", async (ApplicationDbContext _db, int id) =>
{

    ApiResponse response = new ApiResponse
    {
        Success = true,
        Result = await _db.Propiedades.FirstOrDefaultAsync(p => p.IdPropiedad == id),
        StatusCode = System.Net.HttpStatusCode.OK
    };

    if (response.Result == null)
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.NotFound;
        response.Errores = new List<string> { "No se encontró la propiedad." };
        return Results.NotFound(response);
    }

    return Results.Ok(response);
}).WithName("GetPropiedad").Produces<ApiResponse>(200);

app.MapPost("/api/propiedades", async (ApplicationDbContext _db, IMapper _mapper, IValidator<AddPropiedadDTO> _validation, [FromBody] AddPropiedadDTO dto) =>
{
    ApiResponse response = new()
    {
        Success = false,
        Result = null,
        StatusCode = System.Net.HttpStatusCode.BadRequest,
        Errores = []
    };

    var resultValidation = await _validation.ValidateAsync(dto);

    if (!resultValidation.IsValid)
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        response.Errores = resultValidation.Errors.Select(e => e.ErrorMessage).ToList();
        return Results.BadRequest(response);
    }

    if (await _db.Propiedades.FirstOrDefaultAsync(p => p.Nombre.ToLower() == dto.Nombre.ToLower()) != null)
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        response.Errores = new List<string> { "Ya existe una propiedad con ese nombre." };
        return Results.BadRequest(response);
    }

    if (string.IsNullOrWhiteSpace(dto.Descripcion) || string.IsNullOrWhiteSpace(dto.Ubicacion))
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        response.Errores = new List<string> { "La descripción y la ubicación son obligatorias." };
        return Results.BadRequest(response);
    }

    Propiedad propiedad = _mapper.Map<Propiedad>(dto);
    propiedad.FechaCreacion = DateTime.UtcNow;
    await _db.Propiedades.AddAsync(propiedad);
    await _db.SaveChangesAsync();

    PropiedadDTO propiedadDTO = _mapper.Map<PropiedadDTO>(propiedad);

    response.Success = true;
    response.Result = propiedadDTO;
    response.StatusCode = System.Net.HttpStatusCode.Created;

    return Results.Ok(response);
}).WithName("PostPropiedad").Accepts<PropiedadDTO>("application/json").Produces<ApiResponse>(201).Produces(400);


app.MapPut("/api/propiedades/{id:int}", async (ApplicationDbContext _db, IMapper _mapper, IValidator<UpdatePropiedadDTO> _validation, int id, [FromBody] UpdatePropiedadDTO dto) =>
{
    ApiResponse response = new()
    {
        Success = false,
        Result = null,
        StatusCode = System.Net.HttpStatusCode.BadRequest,
        Errores = []
    };
    var resultValidation = await _validation.ValidateAsync(dto);
    if (!resultValidation.IsValid)
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        response.Errores = resultValidation.Errors.Select(e => e.ErrorMessage).ToList();
        return Results.BadRequest(response);
    }
    if (await _db.Propiedades.FirstOrDefaultAsync(p => p.Nombre.ToLower() == dto.Nombre.ToLower() && p.IdPropiedad != id) != null)
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        response.Errores = new List<string> { "Ya existe una propiedad con ese nombre." };
        return Results.BadRequest(response);
    }
    if (string.IsNullOrWhiteSpace(dto.Descripcion) || string.IsNullOrWhiteSpace(dto.Ubicacion))
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        response.Errores = new List<string> { "La descripción y la ubicación son obligatorias." };
        return Results.BadRequest(response);
    }

    var existingPropiedad = await _db.Propiedades.FirstOrDefaultAsync(p => p.IdPropiedad == id);
    if (existingPropiedad == null)
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.NotFound;
        response.Errores = new List<string> { "No se encontró la propiedad." };
        return Results.NotFound(response);
    }

    existingPropiedad.Nombre = dto.Nombre;
    existingPropiedad.Descripcion = dto.Descripcion;
    existingPropiedad.Ubicacion = dto.Ubicacion;
    existingPropiedad.Activa = dto.Activa;

    await _db.SaveChangesAsync();

    PropiedadDTO propiedadDTO = _mapper.Map<PropiedadDTO>(existingPropiedad);

    response.Success = true;
    response.Result = propiedadDTO;
    response.StatusCode = System.Net.HttpStatusCode.OK;

    return Results.Ok(response);
}).WithName("PutPropiedad").Accepts<UpdatePropiedadDTO>("application/json").Produces<ApiResponse>();


app.MapDelete("/api/propiedades/{id:int}", async (ApplicationDbContext _db, int id) =>
{
    ApiResponse response = new()
    {
        Success = false,
        Result = null,
        StatusCode = System.Net.HttpStatusCode.BadRequest,
        Errores = []
    };

    var existingPropiedad = await _db.Propiedades.FirstOrDefaultAsync(p => p.IdPropiedad == id);
    if (existingPropiedad == null)
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.NotFound;
        response.Errores = new List<string> { "No se encontró la propiedad." };
        return Results.NotFound(response);
    }

    _db.Propiedades.Remove(existingPropiedad);
    await _db.SaveChangesAsync();
    response.Success = true;
    response.Result = existingPropiedad;
    response.StatusCode = System.Net.HttpStatusCode.OK;

    return Results.Ok(response);
}).WithName("DeletePropiedad");

app.UseHttpsRedirection();

app.Run();