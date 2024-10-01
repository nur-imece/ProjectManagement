using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectManagement.Model.Configuration;
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

// MongoDB Ayarlarını Kayıt Etme
builder.Services.Configure<MongoDbSetting>(builder.Configuration.GetSection("MongoDbSetting"));
builder.Services.AddScoped<IMovieServices, MovieServices>();
builder.Services.AddScoped<IJobServices, JobServices>();
builder.Services.AddScoped<ICommentServices, CommentServices>();

// MongoDB Bağlantısı için IMongoClient Kaydı
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSetting>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// MongoDB Veritabanını Ayarlama
builder.Services.AddSingleton(prov =>
{
    var client = prov.GetRequiredService<IMongoClient>();
    return client.GetDatabase(configuration["MongoDbSetting:Database"]);
});

// AddDbContext YERİNE MongoDB için özel çözüm kullanıyoruz (Doğrudan IMongoDatabase)
builder.Services.AddSingleton<MflixDbContext>();

// Sadece standart API controller yapılandırması
builder.Services.AddControllers();

// Swagger yapılandırması
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Geliştirme ortamında Swagger'ı etkinleştirin
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectManagement API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
