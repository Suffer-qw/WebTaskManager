
using WebTaskManager.Endpoints;
using WebTaskManager.Exceptions;
using WebTaskManager.Extensions;
using WebTaskManager.Interface;
using WebTaskManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServies();
builder.Services.AddScoped<IMyTaskService, MyTaskServices>();

//Add Global Exception handling

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

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

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.MapGroup("api/v1/mytask")
    .WithTags("mytask endpoint")
    .MapMyTaskEndPoint();
app.MapGroup("api/v1/UserProfile")
    .WithTags("UserProfile endpoint")
    .MapUserProfileEndPoint();


app.Run();
