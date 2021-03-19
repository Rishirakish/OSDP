using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Neubel.Wow.Win.Authentication.WebAPI.Mappers;
using Neubel.Wow.Win.Authentication.Data.Repository;

namespace Neubel.Wow.Win.Authentication.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static TokenValidationParameters tokenValidationParameters;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });

            //services
            services.AddScoped<Core.Interfaces.IAuthenticationService, Services.AuthenticationService>();
            services.AddScoped<Core.Interfaces.IUserService, Services.UserService>();
            services.AddScoped<Core.Interfaces.IOrganizationService, Services.OrganizationService>();
            services.AddScoped<Core.Interfaces.IRoleService, Services.RoleService>();
            services.AddScoped<Core.Interfaces.ISecurityParameterService, Services.SecurityParameterService>();

            //DAL
            services.AddScoped<Infrastructure.IConnectionFactory, Infrastructure.ConnectionFactory>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISecurityParameterRepository, SecurityParameterRepository>();

            // Mappers
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Neubel.Wow.Win.Authentication.WebAPI", Version = "v1" });
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Neubel.Wow.Win.Authentication.WebAPI",
                    Version = "v1",
                    Description = "Neubel.Wow.Win.Authentication.WebAPI",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Rishikesh",
                        Email = "rishikeshm13@outlook.com",
                        Url = new Uri("https://twitter.com/rishikesh"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Neubeltech",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Neubel.Wow.Win.Authentication.WebAPI v1"));
            }
            tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Configuration["JWT:ValidIssuer"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration["JWT:ValidAudience"],

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
