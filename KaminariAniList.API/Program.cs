using Wms.Service.ServiceConnector;
using WMS.Core.ContextInterfaces;
using WMS.Core.Entities.ConnectionAndSettings.MongoDb;
using WMS.Core.RepositoryInterfaces;
using WMS.Core.ServiceInterfaces;
using WMS.DataAccess.ContextEntities.MongoDb;
using WMS.DataAccess.RepositoryArticles;
using WMS.DataAccess.RepositoryEntities;
using WMS.Service.ServiceArticles;
using WMS.Service.ServiceConnector.ApiClient;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WMS.Core.Entities.ConnectionAndSettings.App;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Settings & Connection
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

MongoDbConnection mongoDbConnection = builder.Configuration.GetSection("MongoDbConnection").Get<MongoDbConnection>();
BaseUrlContainer baseUrlContainer = builder.Configuration.GetSection("BaseUrlContainer").Get<BaseUrlContainer>();
JwtSettings jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(mongoDbConnection);
builder.Services.AddSingleton(baseUrlContainer);
builder.Services.AddScoped<IMongoDbContext, MongoDbContext>(serviceProvider =>
{
    string connectionString = mongoDbConnection.ConnectionString;
    string databaseName = mongoDbConnection.Database;
    return new MongoDbContext(connectionString, databaseName);
});


#region Auth Settings

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

#endregion
#endregion

#region Dependency Injection / IOC 

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(ServiceConnector<>));
builder.Services.AddScoped(typeof(ITogetherRepository), typeof(TogetherRepository));
builder.Services.AddScoped(typeof(ITogetherService), typeof(TogetherService));
builder.Services.AddScoped(typeof(IAnimeService), typeof(AnimeService));
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
builder.Services.AddScoped(typeof(IUserAnimeProfileService), typeof(UserAnimeProfileService));
builder.Services.AddScoped(typeof(IUserAnimeProfileRepository), typeof(UserAnimeProfileRepository));
builder.Services.AddScoped(typeof(IUserAnimeRepository), typeof(UserAnimeRepository));
builder.Services.AddScoped(typeof(IMovieService), typeof(MovieService));

#endregion

var app = builder.Build();

app.UseCors("AllowAll");


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
