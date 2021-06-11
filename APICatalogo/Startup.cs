using ApiCatalogo.Extensions;
using ApiCatalogo.Logging;
using APICatalogo.Context;
using APICatalogo.Models.Mappings;
using APICatalogo.Repository;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace APICatalogo
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
            services.AddCors(options =>
            {
                options.AddPolicy("PermitirApiRequest",
                    builder =>
                    builder.WithOrigins("https://www.apirequest.io")
                        .WithMethods("GET")
                        );
            });

            string mySqlConnection = Configuration.GetConnectionString("DefaultConnection");

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddScoped<ApiLoggingFilter>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(mySqlConnection,
            ServerVersion.AutoDetect(mySqlConnection)));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IMeuServico, MeuServico>();

            //JWT
            //adiciona o manipulador de autenticacao e define o 
            //esquema de autenticacao usado : Bearer
            //valida o emissor, a audiencia e a chave
            //usando a chave secreta valida a assinatura
            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidAudience = Configuration["TokenConfiguration:Audience"],
                     ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                 });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CatalogoAPI",
                    Description = "Catalogo de Produtos e Categorias",
                    TermsOfService = new Uri("https://delsivan.net/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "delsivan",
                        Email = "delsivan@yahoo.com",
                        Url = new Uri("https://www.delsivan.net"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Usar sobre LICX",
                        Url = new Uri("https://delsivan.net/license"),
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICatalogo", Version = "v1" });
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APICatalogo v1"));
            }

            loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
            {
                LogLevel = LogLevel.Information
            })); ;

            //adicionando o middleware de tratamento de erros
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            //adiciona o middleware de autenticacao
            app.UseAuthentication();

            //adiciona o middleware que habilita a autorizacao.
            app.UseAuthorization();

            //app.UseCors(opt => opt.
            //WithOrigins("https://www.apirequest.io")
            //.WithMethods("GET"));

            //Swagger
            app.UseSwagger();
            //SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                      "APICatalogo");
            });

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
