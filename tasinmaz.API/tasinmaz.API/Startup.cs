using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using tasinmaz.API.Data;
using tasinmaz.API.Services.Abstract;
using tasinmaz.API.Services.Concrete;
using Newtonsoft.Json;
using tasinmaz.API.Helpers;
using Microsoft.AspNetCore.Http;
using System.Configuration;
using tasinmaz.API.Models;

namespace tasinmaz.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);

            services.AddDbContext<DataContext>(x => x.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
 
            services.AddAutoMapper();

            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                });
            });

            services.AddHttpContextAccessor();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITasinmazRepository, TasinmazRepository>();
            services.AddScoped<ITasinmazService, TasinmazService>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ILocationRepository<Il>, LocationRepository<Il>>();
            services.AddScoped<ILocationRepository<Ilce>, LocationRepository<Ilce>>();
            services.AddScoped<ILocationRepository<Mahalle>, LocationRepository<Mahalle>>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILocationRepository<Il>, LocationRepository<Il>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["AppSettings:Issuer"],
                    ValidAudience = Configuration["AppSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
