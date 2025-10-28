using CsvImportService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Register Services from Infrastructure (Kafka + Redis + CsvProcessor)
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();


// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS if Angular is accessing this
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); // Use attribute-based controllers (e.g., UploadController)

app.Run();
