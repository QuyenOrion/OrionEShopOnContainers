// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrionEShopOnContainer.Services.Identity.API
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            if (Environment.IsDevelopment())
                services.AddTransient<IDiscoveryResponseGenerator, CustomDiscoveryResponseGenerator>();

            string connectionString = Configuration.GetConnectionString("Default");
            Log.Information($"Connection string: {connectionString}");
            var postgresHost = System.Environment.GetEnvironmentVariable("AppSettings__ConnectionStrings__PostgresHost");
            if (!string.IsNullOrEmpty(postgresHost))
            {
                var database = System.Environment.GetEnvironmentVariable("AppSettings__ConnectionStrings__PostgresDb");
                var username = System.Environment.GetEnvironmentVariable("AppSettings__ConnectionStrings__PostgresUser");
                var password = System.Environment.GetEnvironmentVariable("AppSettings__ConnectionStrings__PostgresPassword");
                var options = System.Environment.GetEnvironmentVariable("AppSettings__ConnectionStrings__PostgresOptions");
                Log.Information($"Host={postgresHost};Database={database};Username={username};Password={password};{options}");
                connectionString = $"Host={postgresHost};Database={database};Username={username};Password={password};{options}";
            }
            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;

                options.Authentication.CookieLifetime = TimeSpan.FromHours(2);

                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddTestUsers(TestUsers.Users)
                .AddConfigurationStore(opts =>
                {
                    opts.ConfigureDbContext = b => b.UseNpgsql(connectionString,
                                    sql => sql.MigrationsAssembly(typeof(Startup).Assembly.FullName));
                })
                .AddOperationalStore(opts =>
                {
                    opts.ConfigureDbContext = b => b.UseNpgsql(connectionString,
                                    sql => sql.MigrationsAssembly(typeof(Startup).Assembly.FullName));
                });

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = Configuration.GetValue<string>("GoogleOAuth:ClientId");
                    options.ClientSecret = Configuration.GetValue<string>("GoogleOAuth:ClientSecret");
                });

            // not recommended for production - you need to store your key material somewhere secure
            if (Environment.IsDevelopment())
                builder.AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            // This cookie policy fixes login issues with Chrome 80+ using HHTP
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

            app.UseRouting();

            app.UseIdentityServer();

            InitializeDatabase(app);

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any() || context.Clients.Count() != Config.Clients.Count())
                {
                    if (context.Clients.Any())
                        context.Clients.RemoveRange(context.Clients.ToList());

                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any() || context.IdentityResources.Count() != Config.IdentityResources.Count())
                {
                    if (context.IdentityResources.Any())
                        context.IdentityResources.RemoveRange(context.IdentityResources.ToList());

                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any() || context.ApiScopes.Count() != Config.ApiScopes.Count())
                {
                    if (context.ApiScopes.Any())
                        context.ApiScopes.RemoveRange(context.ApiScopes.ToList());

                    foreach (var resource in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }

    public class CustomDiscoveryResponseGenerator : IdentityServer4.ResponseHandling.DiscoveryResponseGenerator
    {
        public CustomDiscoveryResponseGenerator(IdentityServerOptions options, IResourceStore resourceStore, IKeyMaterialService keys, ExtensionGrantValidator extensionGrant, ISecretsListParser secretParser, IResourceOwnerPasswordValidator resourceOwnerPasswordValidator, ILogger<DiscoveryResponseGenerator> logger) :
            base(options, resourceStore, keys, extensionGrant, secretParser, resourceOwnerPasswordValidator, logger)
        {

        }

        public override async Task<Dictionary<string, object>> CreateDiscoveryDocumentAsync(string baseUrl, string issuerUri)
        {
            var res = await base.CreateDiscoveryDocumentAsync(baseUrl, issuerUri);

            res["authorization_endpoint"] = "http://localhost:5105/connect/authorize";
            res["end_session_endpoint"] = "http://localhost:5105/connect/endsession";

            return res;
        }
    }
}
