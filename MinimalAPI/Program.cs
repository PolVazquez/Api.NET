using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Data;
using MinimalAPI.Models;
using MinimalAPI.Models.DTOs;
using MinimalAPI.Validations;

var builder = WebApplication.CreateBuilder(args);

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


app.MapGet("/api/propiedades", (ILogger<Program> logger) =>
{

    logger.LogInformation("Se ha solicitado la lista de propiedades.");

    ApiResponse response = new ApiResponse
    {
        Success = true,
        Result = DatosPropiedad.Propiedades,
        StatusCode = System.Net.HttpStatusCode.OK
    };

    return Results.Ok(response);
}).WithName("GetPropiedades").Produces<ApiResponse>(200);

app.MapGet("/api/propiedades/{id:int}", (int id) =>
{

    ApiResponse response = new ApiResponse
    {
        Success = true,
        Result = DatosPropiedad.Propiedades.FirstOrDefault(p => p.IdPropiedad == id),
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

app.MapPost("/api/propiedades", async (IMapper _mapper, IValidator<AddPropiedadDTO> _validation, [FromBody] AddPropiedadDTO dto) =>
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

    if (DatosPropiedad.Propiedades.FirstOrDefault(p => p.Nombre.ToLower() == dto.Nombre.ToLower()) != null)
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

    propiedad.IdPropiedad = DatosPropiedad.Propiedades.Max(p => p.IdPropiedad) + 1;

    DatosPropiedad.Propiedades.Add(propiedad);

    PropiedadDTO propiedadDTO = _mapper.Map<PropiedadDTO>(propiedad);

    response.Success = true;
    response.Result = propiedadDTO;
    response.StatusCode = System.Net.HttpStatusCode.Created;

    return Results.Ok(response);
}).WithName("PostPropiedad").Accepts<PropiedadDTO>("application/json").Produces<ApiResponse>(201).Produces(400);


app.MapPut("/api/propiedades/{id:int}", async (IMapper _mapper, IValidator<UpdatePropiedadDTO> _validation, int id, [FromBody] UpdatePropiedadDTO dto) =>
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
    if (DatosPropiedad.Propiedades.FirstOrDefault(p => p.Nombre.ToLower() == dto.Nombre.ToLower() && p.IdPropiedad != id) != null)
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

    var existingPropiedad = DatosPropiedad.Propiedades.FirstOrDefault(p => p.IdPropiedad == id);
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

    PropiedadDTO propiedadDTO = _mapper.Map<PropiedadDTO>(existingPropiedad);

    response.Success = true;
    response.Result = propiedadDTO;
    response.StatusCode = System.Net.HttpStatusCode.OK;

    return Results.Ok(response);
}).WithName("PutPropiedad").Accepts<UpdatePropiedadDTO>("application/json").Produces<ApiResponse>();


app.MapDelete("/api/propiedades/{id:int}", (int id) =>
{
    ApiResponse response = new()
    {
        Success = false,
        Result = null,
        StatusCode = System.Net.HttpStatusCode.BadRequest,
        Errores = []
    };

    var existingPropiedad = DatosPropiedad.Propiedades.FirstOrDefault(p => p.IdPropiedad == id);
    if (existingPropiedad == null)
    {
        response.Success = false;
        response.StatusCode = System.Net.HttpStatusCode.NotFound;
        response.Errores = new List<string> { "No se encontró la propiedad." };
        return Results.NotFound(response);
    }

    DatosPropiedad.Propiedades.Remove(existingPropiedad);
    response.Success = true;
    response.Result = existingPropiedad;
    response.StatusCode = System.Net.HttpStatusCode.OK;

    return Results.Ok(response);
}).WithName("DeletePropiedad");

app.UseHttpsRedirection();

app.Run();