using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using TesteViaVarejo.Domain.Entidades;
using TesteViaVarejo.Domain.Entidades.ValuesObjects;
using TesteViaVarejo.Domain.Interfaces.Repositorios;
using TesteViaVarejo.Domain.Interfaces.Servicos;
using TesteViaVarejo.Domain.Servicos;
using TesteViaVarejo.Infra.Data.Config;
using TesteViaVarejo.Infra.Data.Repositorios;

namespace TesteViaVarejo.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfig = new TokenConfig();
            new ConfigureFromConfigurationOptions<TokenConfig>(
                Configuration.GetSection("TokenConfig"))
                    .Configure(tokenConfig);
            services.AddSingleton(tokenConfig);

            services.AddDbContext<Contexto>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:64615");
                });
            });

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfig.Audiencia;
                paramsValidation.ValidIssuer = tokenConfig.Emissor;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Teste Via Varejo API",
                        Version = "v1",
                        Description = "Teste Via Varejo",
                        Contact = new Contact
                        {
                            Name = "Desenvolvedor",
                            Email = "fabiobluz1@gmail.com",
                            Url = String.Empty
                        }
                    });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });


            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddMvc();

            services.AddTransient<IServicoBase<UsuarioAcesso>, ServicoBase<UsuarioAcesso>>();
            services.AddTransient<IServicoBase<CalculoHistoricoLog>, ServicoBase<CalculoHistoricoLog>>();
            services.AddTransient<IServicoBase<Amigo>, ServicoBase<Amigo>>();
            services.AddTransient<IRepositorioBase<UsuarioAcesso>, RepositorioBase<UsuarioAcesso>>();
            services.AddTransient<IRepositorioBase<CalculoHistoricoLog>, RepositorioBase<CalculoHistoricoLog>>();
            services.AddTransient<IRepositorioBase<Amigo>, RepositorioBase<Amigo>>();
            services.AddTransient<IUsuarioAcessoServico, UsuarioAcessoServico>();
            services.AddTransient<IAmigoServico, AmigoServico>();
            services.AddTransient<IUsuarioAcessoRepositorio, UsuarioAcessoRepositorio>();
            services.AddTransient<IServicoBase<Usuario>, ServicoBase<Usuario>>();
            services.AddTransient<IUsuarioServico, UsuarioServico>();
            services.AddTransient<IRepositorioBase<Usuario>, RepositorioBase<Usuario>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = String.Empty;
                c.SwaggerEndpoint("/TesteViaVarejo.API/swagger/v1/swagger.json", "Teste Via Varejo API V1 Docs");
            });

            app.UseCors(MyAllowSpecificOrigins);
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
