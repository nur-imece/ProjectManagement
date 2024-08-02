using ProjectManagement.Model;
using Microsoft.Extensions.Options;
using ProjectManagement.Model.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoDBSettings'i yapılandırma
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// IBooksService ve BooksService kayıtları
builder.Services.AddSingleton<IBookService, BooksService>();

// Kontrollerleri ekleme ve JSON seçeneklerini yapılandırma
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Swagger hizmetlerini ekleme
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP istek hattını yapılandırma
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();