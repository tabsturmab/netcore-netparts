using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Coravel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NetParts.Database;
using NetParts.ExtensionLogger;
using NetParts.Libraries.AutoMapper;
using NetParts.Libraries.Email;
using NetParts.Libraries.Login;
using NetParts.Libraries.Manager.Frete;
using NetParts.Libraries.Manager.Pagamento.PagarMe;
using NetParts.Libraries.Manager.Schedule.Invocable;
using NetParts.Libraries.Middleware;
using NetParts.Libraries.Session;
using NetParts.Libraries.ShoppingCart;
using NetParts.Models;
using NetParts.Repositories;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NuGet.Frameworks;
using WSCorreios;

namespace NetParts
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
            services.AddLocalization(options => { options.ResourcesPath = "Resources";});
            services.AddMvc(options => {
                    var F = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    var L = F.Create("ModelBindingMessages", "NetParts");
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "The '{0}' is required.");
                    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => L["The '{0}' is required."]);
                    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => L["The '{0}' is required."]);
                    //options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "The field must be filled!");
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((x) => L["The '{0}' is required.", x]);
                    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) => L["The value '{0}' is invalid."]);
            })
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pt-BR")
                };
                options.DefaultRequestCulture = new RequestCulture("en", "en");
               
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddAutoMapper(config => config.AddProfile<MappingProfile>());

            services.AddHttpContextAccessor();
            services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IArchiveRepository, ArchiveRepository>();
            services.AddScoped<ITechnicalAssistanceRepository, TechnicalAssistanceRepository>();
            services.AddScoped<ITechnicalAssistanceManufacturerRepository, TechnicalAssistanceManufacturerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderSituationRepository, OrderSituationRepository>();
            services.AddScoped<IOrderAdvertisementRepository, OrderAdvertisementRepository>();

            services.AddScoped<SmtpClient>(options => {
                SmtpClient smtp = new SmtpClient()
                {
                    Host = Configuration.GetValue<string>("Email:ServerSMTP"),
                    Port = Configuration.GetValue<int>("Email:ServerPort"),
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Configuration.GetValue<string>("Email:Username"), Configuration.GetValue<string>("Email:Password")),
                    EnableSsl = true
                };
                return smtp;
            });

            services.AddScoped<CalcPrecoPrazoWSSoap>(options => {
                var servico = new CalcPrecoPrazoWSSoapClient(CalcPrecoPrazoWSSoapClient.EndpointConfiguration.CalcPrecoPrazoWSSoap);
                return servico;
            });

            services.AddScoped<ManageEmail>();
            services.AddScoped<NetParts.Libraries.Cookie.Cookie>();
            services.AddScoped<LoginCollaborator>();
            services.AddScoped<CookieShoppingCart>();
            services.AddScoped<CookieFrete>();
            services.AddScoped<CalcularPacote>();
            services.AddScoped<WSCorreiosCalcularFrete>();

            services.AddMemoryCache();

            services.AddScoped<Session>();
            services.AddScoped<NetParts.Libraries.Cookie.Cookie>();
            services.AddScoped<LoginCollaborator>();
            services.AddScoped<GerenciarPagarMe>();

            services.AddMvc(options =>
                {
                    
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddSessionStateTempDataProvider();

            services.Configure<StorageConfig>(Configuration.GetSection("StorageConfig"));

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddDbContext<NetPartsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConexaoBD")));

            services.AddTransient<OrderPaymentSituationJob>();
            services.AddTransient<OrderDeliveryJob>();
            services.AddTransient<OrderFinishedJob>();
            services.AddTransient<OrderGiveBackDeliveryJob>();
            services.AddScheduler();

            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en")),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            string conexao = Configuration.GetConnectionString("ConexaoBD");
            loggerFactory.AddContext(LogLevel.Information, conexao);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error/Error500");
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            //app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Header-Name", "Header-Value");
                await next();

                if (!context.Response.Headers.ContainsKey("Header-Name"))
                {
                    context.Response.Headers.Add("Header-Name", "Header-Value");
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.ApplicationServices.UseScheduler(scheduler => {
                scheduler.Schedule<OrderPaymentSituationJob>().EveryTenSeconds()/*HourlyAt(60 * 3)*/;
                scheduler.Schedule<OrderDeliveryJob>().EveryTenSeconds()/*HourlyAt(60 * 11)*/;
                scheduler.Schedule<OrderFinishedJob>().EveryTenSeconds()/*HourlyAt(60 * 6)*/;
                scheduler.Schedule<OrderGiveBackDeliveryJob>().EveryTenSeconds()/*HourlyAt(60 * 6)*/;
            });
        }
    }
}
