using ApiCurso.Data;
using ApiCurso.Mapper;
using ApiCurso.Model;
using ApiCurso.Repository;
using ApiCurso.Repository.IRepository;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

//Soporte para autenticación con .Net Identity
builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

//Soporte para cache
builder.Services.AddResponseCaching();

builder.Services.AddControllers(options =>
{
    //Cache para todos los endpoints y no tener que agregar el atributo [ResponseCache] en todos los endpoints
    options.CacheProfiles.Add("Default30", new CacheProfile() { Duration = 30 });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Agregamos autentificación en la documentación Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Autentificación JWT usando el esquema Bearer. \r\n\r\n" +
        "Ingresa la palabra 'Bearer' seguido de un [espacio] y después su token en el campo de abajo.\r\n\r\n" +
        "Ejemplo: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            }, new List<string> { } }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api Curso v1",
        Version = "1.0",
        Description = "API para el curso de ASP.NET Core Web API con C# y SQL Server",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact()
        {
            Name = "VazquezCoder",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense()
        {
            Name = "Licencia de uso",
            Url = new Uri("https://example.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Api Curso v2",
        Version = "2.0",
        Description = "API para el curso de ASP.NET Core Web API con C# y SQL Server",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact()
        {
            Name = "VazquezCoder",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense()
        {
            Name = "Licencia de uso",
            Url = new Uri("https://example.com/license")
        }
    });
});

//Soporte repository's
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();


//Configuracion de autenticación JWT
builder.Services.AddAuthentication(j =>
{
    j.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    j.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(j =>
{
    j.RequireHttpsMetadata = false; //TODO: Producción = true
    j.SaveToken = true;
    j.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:secret"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Soporte para versionado de API
var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    //options.ApiVersionReader = ApiVersionReader.Combine(
    //    new QueryStringApiVersionReader("api-version"));
});

apiVersioningBuilder.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

//Soporte CORS
//Habilitar: 1-Un único dominio, 2- Multiples dominios, 3- Todos los dominios
//Ejemplo: http://localhost:1234
//(*) para todos los dominios
builder.Services.AddCors(builder =>
{
    builder.AddPolicy("CorsPolicy", options =>
    {
        options.AllowAnyHeader().WithOrigins("https://localhost:7174");
    });
});

//Add AutoMapper
builder.Services.AddAutoMapper(typeof(EntityToDtoReverseMapper));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Curso v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Api Curso v2");
    });
}

//Soporte para ficheros estáticos cdmo imágenes
app.UseStaticFiles();
app.UseHttpsRedirection();

//Soporte para CORS
app.UseCors("CorsPolicy");

app.UseAuthentication();

//Soporte para autenticación
app.UseAuthorization();

app.MapControllers();

app.Run();
