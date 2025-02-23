var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddApplicationConfigurations(configuration);
builder.Services.AddControllerConfigurations();
builder.Services.AddDocumentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDocumentation();
app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();
app.UseCors(b => b
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();