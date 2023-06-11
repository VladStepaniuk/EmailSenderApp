using EmailSenderApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;



namespace EmailSenderApp
{
    public class Startup
    {
        private static IScheduler _scheduler;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;  
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<EmailSenderService>();

            services.AddCors(o => o.AddPolicy("AllowAnyOrigin",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                      }));

            // registration Quartz.NET
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start().Wait();
                return scheduler;
            });

            services.AddTransient<EmailJob>();
            services.AddSingleton<ISchedulerAccessor>(provider =>
            {
                return new SchedulerAccessor(provider.GetService<IScheduler>());
            });

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISchedulerAccessor schedulerAccessor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors("AllowAnyOrigin");

            _scheduler = schedulerAccessor.Scheduler;

            QuartzService.StartSchedule(_scheduler);

        }
    }
}
