using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using WMSAuthentication.Interfaces;
using WMSAuthentication.WMSProxy;
using WMSDataAccess;
using WMSDataAccess.UserManagement;
using WMSDataAccess.UserManagement.DBContexts;
using WMSTools;
using WMSTools.Interfaces;
using ITokenHelper = WMSTools.Interfaces.ITokenHelper;

namespace WMSAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddMvc(options =>
                options.Filters.Add(new IgnoreAntiforgeryTokenAttribute())
            );

            var optionsConnectionString = _configuration.GetConnectionString("UsersDatabase");
            Console.WriteLine(optionsConnectionString);
            services.AddDbContext<UserDBContext>(options => options.UseNpgsql(optionsConnectionString));
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigins",
                    builder => builder
                    .WithOrigins("http://localhost:4200")
                    .WithOrigins("http://appgis.eastus.cloudapp.azure.com")
                    .WithOrigins("http://appgis.com")
                    //.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<RuntimeOptions>(options =>
            {
                options.ConnectionString = optionsConnectionString;
                options.ProxyToPort = _configuration.GetValue("ProxyToPort", 80);
                options.MapServerHost = _configuration.GetValue("MapServerHost", "127.0.0.1");
                options.MapServerName = _configuration.GetValue("MapServerName", "mapserv");
                options.MapFilesDir = _configuration.GetValue("MapFilesDir", "/var/www/appgiswms/maps/usermaps/");
                options.AllowedUrls = _configuration.GetSection("AllowedUrls").Get<List<string>>();
                options.SessionToken = _configuration["SessionToken"];
                options.CookieToken = _configuration["CookieToken"];
                options.SetCookie = _configuration["SetCookie"];
                options.CacheControlMaxAgeInASeconds = _configuration.GetValue("CacheControlMaxAgeInASeconds", 60 * 60 * 24);
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
            });

            services.AddHttpContextAccessor();

            //_loggerFactory.AddConsole();
            //_loggerFactory.AddDebug(LogLevel.Debug);

            services.AddTransient(l => _loggerFactory.CreateLogger("Middleware"));

            services.AddTransient<IProxyRulesProvider, ProxyRulesProvider>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IAuthenticatorHandler, AuthenticatorHandler>();
            services.AddTransient<IHashHelper, HashHelper>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IReverseProxyHandler, ReverseProxyHandler>();
            services.AddTransient<IUserDBSeeder, UserDBSeeder>();
            services.AddTransient<ICredentialHandler, CredentialHandler>();
            services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();
            services.AddTransient<ITokenHelper, TokenHelper>();
            services.AddTransient<IRandomNumberGenerator, RandomNumberGenerator>();
            services.AddTransient<IWMSRuleFactory, WMSRuleFactory>();
            services.AddTransient<IWMSProxy, WMSProxy.WMSProxy>();
            services.AddTransient<IWMSProxyManager, WMSProxyManager>();
            services.AddTransient<IContextHandler, ContextHandler>();
            services.AddTransient<IHttpRequestMessageHandler, HttpRequestMessageHandler>();
            services.AddTransient<IWMSProxyRequestHandler, WMSProxyRequestHandler>();
            services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
            services.AddTransient<IWMSMessageProcessor, WMSMessageProcessor>();
            services.AddTransient<ISessionTokenHandler, SessionTokenHandler>();
            services.AddTransient<IWMSResponseHandler, WMSResponseHandler>();
            services.AddTransient<IUserDBContextFactory, UserDBContextFactory>();
            services.AddTransient<ICredentialExtractor, CredentialExtractor>();
            services.AddTransient<ICredentialsAuthenticator, CredentialsAuthenticator>();
            services.AddTransient<ITokenAuthenticator, TokenAuthenticator>();
            services.AddTransient<IDateTimeHelper, DateTimeHelper>();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IReverseProxyHandler rph, IUserDBSeeder userDBSeeder, IUserDBContextFactory userDBContextFactory)
        {
            UpdateDatabase(app);
            userDBSeeder.Seed();

            app.UseCors("AllowOrigins");
            app.UseSession();
            var proxyOptions = rph.GetProxyOptions();
            app.UseMiddleware<WMSProxyMiddleware>(Options.Create(proxyOptions));
            app.UseRouting();
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<UserDBContext> ())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}