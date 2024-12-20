using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using sonrysocialsapi.Authentication;
using sonrysocialsapi.Infrastructure;
using sonrysocialsapi.Models;
using TokenHandler = sonrysocialsapi.Infrastructure.TokenHandler;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder
                .WithOrigins("*") // Reemplaza "*" por el origen específico para mayor seguridad
                .AllowAnyMethod() // Permite cualquier método (GET, POST, PUT, DELETE, etc.)
                .AllowAnyHeader(); // Permite cualquier encabezado, incluyendo "Authorization"
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<MineContext>(options=>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("minedb"));
});

builder.Services.AddTransient<ITokenHandler, TokenHandler>();
builder.Services.AddTransient<IUserHandler, UserHandler>();
builder.Services.AddTransient<IPostHandler, PostHandler>();

builder.Services.AddAuthentication
        (SocialAuthOptions.DefaultScheme)
    .AddScheme<SocialAuthOptions, SocialAuthHandler>
    (SocialAuthOptions.DefaultScheme, 
        options => { });

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });




var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();



app.Run();
