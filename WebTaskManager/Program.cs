
using WebTaskManager.Endpoints;
using WebTaskManager.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServies();

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

app.MapGroup("api/v1/")
    .WithTags("mytask endpoint")
    .MapMyTaskEndPoint();

app.Run();
