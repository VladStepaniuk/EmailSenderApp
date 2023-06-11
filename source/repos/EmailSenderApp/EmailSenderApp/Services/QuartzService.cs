using Quartz;
using Quartz.Impl;

namespace EmailSenderApp.Services
{
    public class QuartzService
    {

        public QuartzService()
        {
            
        }

        public static ITrigger BuildEmailTrigger(int emailInterval, string periodicity)
        {
            var emailTrigger = TriggerBuilder.Create().WithIdentity("emailTrigger")
                        .StartNow();

            switch (periodicity)
            {
                case "hours":
                    emailTrigger
                        .WithSimpleSchedule(x => x.WithIntervalInHours(emailInterval).RepeatForever());
                    break;
                case "minutes":
                    emailTrigger
                        .WithSimpleSchedule(x => x.WithIntervalInMinutes(emailInterval).RepeatForever());
                    break;
                case "seconds":
                    emailTrigger
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(emailInterval).RepeatForever());
                    break;
                default:
                    emailTrigger
                        .WithSimpleSchedule(x => x.WithIntervalInHours(emailInterval).RepeatForever());
                    break;
            }

            return emailTrigger.Build();
        }

        public static void StartSchedule(IScheduler _scheduler)
        {

            //_scheduler = schedulerAccessor.Scheduler;

            // schedule sending email 
            var job = JobBuilder.Create<EmailJob>()
                .WithIdentity("emailJob")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("emailTrigger")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInHours(EmailSenderService.GetEmailInterval()).RepeatForever())
                .Build();

            _scheduler.ScheduleJob(job, trigger).Wait();
        }

        public static void RestartSchedule(IScheduler _scheduler)
        {
            // Create a new instance of the scheduler
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            // Start the new scheduler
            _scheduler.Start().GetAwaiter().GetResult();
        }

    }
}
