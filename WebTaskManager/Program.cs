using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebTaskManager.Endpoints;
using WebTaskManager.Extensions;
using WebTaskManager.Interface;
using WebTaskManager.Model;
using WebTaskManager.Services;

var builder = WebApplication.CreateBuilder(args);


builder.AddApplicationServies();


builder.Services.AddProblemDetails();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
//Настройка Swagger с JWT аутентификацией
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите JWT токен в формате: Bearer токен",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    //AddSecurityDefinition("Bearer", ...) — определяем схему безопасности под названием "Bearer",
    //которая будет описывать, как нужно передавать JWT токен.

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        //AddSecurityRequirement(...) — добавляем требование безопасности к всему API или определённым эндпоинтам.
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});
//Благодаря этой настройке Swagger UI покажет кнопку Authorize, куда можно вставить JWT токен.
//Это позволяет тестировать защищённые эндпоинты прямо из Swagger.

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
//считываем из конфигурации (appsettings.json) параметры JWT (Secret, Issuer, Audience, Expiry).

// Добавьте сервисы JWT-аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // настраиваем поведение аутентификации через JWT bearer токены.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // параметры, по которым будет проверяться входящий токен:
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.Issuer,  
            ValidAudience = jwtSettings.Audience, 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))  // сильный секретный ключ, хранится в appsettings.json 
                                            };
    });
//После этой настройки ASP.NET Core автоматически будет проверять JWT токены в заголовках Authorization входящих HTTP-запросов

// Добавьте авторизацию (опционально, но полезно для политик, если потребуется позже)
builder.Services.AddAuthorization();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.MapGroup("api/v1/Login")
    .WithTags("Login endpoint")
    .MapLoginEndpoints();
app.MapGroup("api/v1/TaskStatus")
    .WithTags("TaskStatus endpoint")
    .MapTaskStatusEndPoint();
app.MapGroup("api/v1/mytask")
    .WithTags("mytask endpoint")
    .MapMyTaskEndPoint();
app.MapGroup("api/v1/UserProfile")
    .WithTags("UserProfile endpoint")
    .MapUserProfileEndPoint();


app.Run();
