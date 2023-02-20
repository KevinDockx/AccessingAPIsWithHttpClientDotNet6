using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movies.API.DbContexts;
using Movies.API.Services;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // Return a 406 when an unsupported media type was requested
    options.ReturnHttpNotAcceptable = true;
})
// Override System.Text.Json with Json.NET
//.AddNewtonsoftJson(setupAction =>
//{
//    setupAction.SerializerSettings.ContractResolver =
//       new CamelCasePropertyNamesContractResolver();
//});
.AddXmlSerializerFormatters();

// add support for (de)compressing requests/responses (eg gzip)
builder.Services.AddResponseCompression();
builder.Services.AddRequestDecompression();

// register the DbContext on the container, getting the
// connection string from appSettings   
builder.Services.AddDbContext<MoviesDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:MoviesDBConnectionString"]));

builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<IPostersRepository, PostersRepository>();
builder.Services.AddScoped<ITrailersRepository, TrailersRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
    {
        setupAction.SwaggerDoc("v1",
            new() { Title = "Movies API", Version = "v1" });
    }
);

var app = builder.Build();

// For demo purposes, delete the database & migrate on startup so 
// we can start with a clean slate
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetService<MoviesDbContext>();
        if (context != null)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// use response compression (client should pass through 
// Accept-Encoding)
app.UseResponseCompression();

// use request decompression (client should pass through 
// Content-Encoding)
app.UseRequestDecompression();

app.UseAuthorization();

app.MapControllers();

app.Run();
