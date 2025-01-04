using Serilog;

var builder = WebApplication.CreateBuilder(args);
// logger is already registered in the CreateBuilder method

// We will register the serilog logger here
//  Anything above the minimum level (debug) will be logged
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("logs/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog(); // tells the application to use the serilog logger instead of the default console logger
// By default, the ILogger interface uses the built-in logging providers, such as Console, Debug, or EventSource.
// The line builder.Host.UseSerilog(); tells ASP.NET Core to use Serilog instead of the default logger.

// Since we used DI, we can change the basic implementation of the logger to use the serilog logger.
// You do NOT need to change the logger in the controllers, services, etc. because they are using the ILogger interface.

// Add services to the container.

builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
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

app.UseAuthorization();

app.MapControllers();

app.Run();

// In an HTTP GET request, the Accept header indicates the type(s) of content the client (e.g., a browser or API client) is willing to receive in response to the request.