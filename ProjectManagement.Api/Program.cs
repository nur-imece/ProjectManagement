using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectManagement.Model.Configuration;
using Microsoft.OData.Edm;
using ProjectManagement.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using ProjectManagement.Data;
using ProjectManagement.Model.Services.Implementation;
using ProjectManagement.Model.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

configuration
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{env}.json", true, true);

// Register MongoDB settings
builder.Services.Configure<MongoDbSetting>(builder.Configuration.GetSection("MongoDbSetting"));
builder.Services.AddScoped<IMovieServices, MovieServices>();

IEdmModel GetEdmModel()
{
    var model = new ODataConventionModelBuilder();
    model.EntitySet<Movie>("Movies");
    return model.GetEdmModel();
}

builder.Services.AddEndpointsApiExplorer();



builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSetting>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton(prov =>
{
    var client = prov.GetRequiredService<IMongoClient>();
    return client.GetDatabase( configuration["MongoDbSetting:Database"]);
});


builder.Services.AddDbContext<MflixDbContext>((provider, options) =>
{
    var mongoClient = provider.GetRequiredService<IMongoClient>();
    var database = mongoClient.GetDatabase(configuration["MongoDbSetting:Database"]);
    options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
});



var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntityType<Movie>();


builder.Services.AddControllers()
    .AddOData(options => options
        .AddRouteComponents("odata", GetEdmModel())
        .Select()
        .Filter()
        .OrderBy()
        .SetMaxTop(20)
        .Count()
        .Expand()
    );

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Set up correct schema generation for OData if needed
});

builder.Services.AddControllers();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODataTutorial v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();