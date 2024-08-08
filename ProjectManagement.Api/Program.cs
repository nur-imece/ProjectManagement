using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Model;
using ProjectManagement.Model.Services;
using System.Text;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using AspNetCore.Identity.MongoDbCore.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// MongoDBSettings'i yapılandırma
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// IBookService ve BooksService kayıtları
builder.Services.AddSingleton<IBookService, BooksService>();

// Identity ve MongoDB ayarlarını yapılandırma
builder.Services.AddIdentity<MongoIdentityUser<Guid>, MongoIdentityRole<Guid>>()
    .AddMongoDbStores<MongoIdentityUser<Guid>, MongoIdentityRole<Guid>, Guid>(
        builder.Configuration["MongoDB:ConnectionString"], 
        builder.Configuration["MongoDB:DatabaseName"])
    .AddDefaultTokenProviders();


// JWT ayarlarını yapılandırma
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

// Kontrolleri ekleme ve JSON seçeneklerini yapılandırma
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Swagger hizmetlerini ekleme ve JWT konfigürasyonunu yapılandırma
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// HTTP istek hattını yapılandırma
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Kimlik doğrulama ve yetkilendirme middleware'lerini ekleme
app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();
