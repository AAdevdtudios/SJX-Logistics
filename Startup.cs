using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SjxLogistics.Components;
using SjxLogistics.Controllers.AuthenticationComponent;
using SjxLogistics.Data;
using SjxLogistics.Models;
using System;
using System.Text;

namespace SjxLogistics
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            AccessConfig authConfig = new ();
            Configuration.Bind("Authentication", authConfig);
            services.AddSingleton(authConfig);


            //Singletons
            services.AddSingleton<IpasswordHasher, BcryptPassHasher>();
            services.AddSingleton<IUserRepository, AuthenticationRepository>();
            services.AddSingleton<AccessToken>();
            services.AddSingleton<TokkenGeneratorClass>();


            //HandleJWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.AccessKey)),
                    ValidIssuer = authConfig.Issuer,
                    ValidAudience = authConfig.Audiance,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });

            //Api Requirement 
            services.AddHttpContextAccessor();
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddDbContext<DataBaseContext>((option) => option.UseNpgsql(Configuration["ApisConnection"]));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient<IAuditRepository, AuditRepository>();
            //services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add(typeof(AuditFilterAttribute));
            //});

            //services.AddScoped<AuditFilterAttribute>();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".Audit.Session";
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
            });
         //SwaggerDoc
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SjxLogistics", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SjxLogistics v1"));
            }

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
