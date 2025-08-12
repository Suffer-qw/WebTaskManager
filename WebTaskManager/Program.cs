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
//��������� Swagger � JWT ���������������
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "������� JWT ����� � �������: Bearer �����",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    //AddSecurityDefinition("Bearer", ...) � ���������� ����� ������������ ��� ��������� "Bearer",
    //������� ����� ���������, ��� ����� ���������� JWT �����.

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        //AddSecurityRequirement(...) � ��������� ���������� ������������ � ����� API ��� ����������� ����������.
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
//��������� ���� ��������� Swagger UI ������� ������ Authorize, ���� ����� �������� JWT �����.
//��� ��������� ����������� ���������� ��������� ����� �� Swagger.

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
//��������� �� ������������ (appsettings.json) ��������� JWT (Secret, Issuer, Audience, Expiry).

// �������� ������� JWT-��������������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // ����������� ��������� �������������� ����� JWT bearer ������.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ���������, �� ������� ����� ����������� �������� �����:
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.Issuer,  
            ValidAudience = jwtSettings.Audience, 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))  // ������� ��������� ����, �������� � appsettings.json 
                                            };
    });
//����� ���� ��������� ASP.NET Core ������������� ����� ��������� JWT ������ � ���������� Authorization �������� HTTP-��������

// �������� ����������� (�����������, �� ������� ��� �������, ���� ����������� �����)
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
