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
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "������� JWT ����� � �������: Bearer {�����}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
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

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
// �������� ������� JWT-��������������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,  // ��������, "https://yourapp.com" � ������ ��������� � ���, ��� ������������ � JwtServices.GenerateToken
            ValidAudience = jwtSettings.Audience,  // ��������, "https://yourapp.com" � ������ ��������� � GenerateToken
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))  // ����������� ������� ��������� ����; ������� � appsettings.json ��� ��������� ��������
                                            };
    });

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
