﻿using Kalantyr.Auth.Client;
using Microsoft.Extensions.Options;
using Relax.Characters.Config;
using Relax.Characters.Services;
using Relax.Characters.Services.Impl;

namespace Relax.Characters
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthConfig>(_configuration.GetSection("AuthService"));

            services.AddSingleton<IAppAuthClient>(sp => new AuthClient(
                sp.GetService<IHttpClientFactory>(),
                sp.GetService<IOptions<AuthConfig>>().Value.AppKey));
            services.AddScoped<ICharacterService, CharacterService>();

            services.AddHttpClient<AuthClient>((sp, client) =>
            {
                client.BaseAddress = new Uri(sp.GetService<IOptions<AuthConfig>>().Value.ServiceUrl);
            });

            services.AddSwaggerDocument();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();
            app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
        }
    }
}
