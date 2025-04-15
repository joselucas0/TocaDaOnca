using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TocaDaOnca.AppDbContext;
using TocaDaOnca.Services;

try
{
    
    var builder = WebApplication.CreateBuilder(args);
    Console.WriteLine("JWT KEY => " + builder.Configuration["Jwt:Key"]);

    // Add services to the container
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddControllers();

    // Configure PostgreSQL with Entity Framework
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<Context>(options =>
        options.UseNpgsql(connectionString)
        .EnableSensitiveDataLogging() // Adiciona mais detalhes ao log
        .LogTo(Console.WriteLine, LogLevel.Information)); // Exibe logs no console);

    // Adicionar configuração JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new NullReferenceException("The JWT key is null.")))
            };
        });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });


    // Registrar serviços de autenticação e token
    builder.Services.AddScoped<AuthService>();
    builder.Services.AddScoped<TokenService>();

    var app = builder.Build();

    app.UseCors("AllowAll");
    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // Adicionar middleware para autenticação e autorização
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}