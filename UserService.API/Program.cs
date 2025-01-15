
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Infrastructure.DbContexts;
using UserService.Infrastructure.Repositories;

namespace UserService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            string connectionString = builder.Configuration.GetConnectionString("MyConnectionString");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add Authorization services
            builder.Services.AddAuthorization();

            // Register application services
            builder.Services.AddScoped<IUserService, UserService.Application.Services.UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            // Configure JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; 
                    options.SaveToken = true; // Save the token in the request for future use
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false, 
                        ValidateAudience = false, 
                        ValidateLifetime = true, 
                        ValidateIssuerSigningKey = true, 
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])) 
                    };
                });

            // Add Authorization Policies
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));
            });

            builder.Services.AddControllers();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); 
            }

            app.UseHttpsRedirection(); 

            app.UseAuthentication(); 
            app.UseAuthorization(); 

            app.Run(); 
        }

    }
}
