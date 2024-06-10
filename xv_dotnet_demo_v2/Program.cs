using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using xv_dotnet_demo.Profiles;
using xv_dotnet_demo_v2_domain.Entities;
using xv_dotnet_demo_v2_domain.Settings;
using xv_dotnet_demo_v2_infrastructure.Repository;
using xv_dotnet_demo_v2_infrastructure;
using xv_dotnet_demo_v2_infrastructure.DbContext;
using xv_dotnet_demo_v2_services.Implementations;
using xv_dotnet_demo_v2_services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Add connection string
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlite("name=ConnectionStrings:DefaultConnection"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IGenericRepository<Message>, MessageRepository>();
builder.Services.AddScoped<IGenericRepository<Names>, NamesRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<INamesService, NamesService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("jwt"));
builder.Services.Configure<RickAndMortyApiSettings>(builder.Configuration.GetSection("RickAndMortyAPI"));
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddHttpClient<IRickAndMortyService, RickAndMortyService>();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthorization();
//AutoMapper config
var autoMapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new MappingProfile());
});

var mapper = autoMapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
