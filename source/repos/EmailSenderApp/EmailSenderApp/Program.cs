using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Quartz.Simpl;
using Quartz.Spi;
using System;

namespace EmailSenderApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory(options =>
                        {
                            options.AllowDefaultConstructor = true;
                        });

                        q.UseSimpleTypeLoader();
                        q.UseInMemoryStore();
                        q.UseDefaultThreadPool(tp =>
                        {
                            tp.MaxConcurrency = 10;
                        });

                        var jobKey = new JobKey("emailJob");
                        q.AddJob<EmailJob>(jobKey);

                        var triggerKey = new TriggerKey("emailTrigger");
                        q.AddTrigger(t => t
                            .WithIdentity(triggerKey)
                            .ForJob(jobKey)
                            .StartNow()
                            .WithSimpleSchedule(x => x
                                .WithInterval(TimeSpan.FromHours(1))
                                .RepeatForever())
                        );
                    });

                    services.AddQuartzHostedService(options =>
                    {
                        options.WaitForJobsToComplete = true;
                    });
                })
                .Build();

            await host.RunAsync();
        }
    }
}



