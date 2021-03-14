using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LackBot.API.Repositories;
using LackBot.API.Repositories.Implementation;
using LShort.Common.Database.Implementation;
using LShort.Common.Logging;
using LShort.Common.Logging.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unity;
using Unity.Lifetime;

namespace LackBot.API
{
    public class Startup
    {
        private IUnityContainer container;
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void ConfigureContainer(IUnityContainer container)
        {
            this.container = container;
            
            // setup database
            var dbConnectivityProvider = new MongoDatabaseConnectivityProvider();
            var db = dbConnectivityProvider.Connect(Environment.GetEnvironmentVariable("DB_CREDENTIALS"));

            if (db == null)
            {
                Console.WriteLine("Failed to connect to database. Exiting...");
                Environment.Exit(1);
            }

            container.RegisterInstance(db, new SingletonLifetimeManager());
            
            // init logger; for some reason Unity can't instantiate one properly without a stack overflow
            var logger = new MongoAppLogger(db) as IAppLogger;
            container.RegisterInstance(logger, new SingletonLifetimeManager());
            
            // repositories
            container.RegisterType<IAutoResponseRepository, AutoResponseMongoRepository>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IAutoReactRepository, AutoReactMongoRepository>(
                new ContainerControlledLifetimeManager());
            
            logger.Information("Application initialized successfully.");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}