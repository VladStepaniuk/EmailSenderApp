using EmailSenderApp;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EmailScheduler
{
    private readonly IScheduler _scheduler;
    private readonly IJobFactory _jobFactory;

    public EmailScheduler(IScheduler scheduler, IJobFactory jobFactory)
    {
        _scheduler = scheduler;
        _jobFactory = jobFactory;
    }

    public async Task ScheduleEmailJob(int intervalHours, TimeSpan startTime, TimeSpan endTime, IList<DayOfWeek> daysOfWeek)
    {
        var jobKey = new JobKey("emailJob");
        var triggerKey = new TriggerKey("emailTrigger");

        var jobDetail = JobBuilder.Create<EmailJob>()
            .WithIdentity(jobKey)
            .Build();

        var triggerBuilder = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .ForJob(jobKey);

        if (intervalHours > 0)
        {
            triggerBuilder.WithSimpleSchedule(x => x
                .WithInterval(TimeSpan.FromHours(intervalHours))
                .RepeatForever());
        }
        else
        {
            triggerBuilder.WithSimpleSchedule(x => x
                .WithInterval(TimeSpan.FromHours(1))
                .RepeatForever());
        }

        if (startTime != TimeSpan.MinValue && endTime != TimeSpan.MinValue)
        {
            triggerBuilder.StartAt(DateBuilder.TodayAt(startTime.Hours, startTime.Minutes, startTime.Seconds))
                .EndAt(DateBuilder.TodayAt(endTime.Hours, endTime.Minutes, endTime.Seconds));
        }

        if (daysOfWeek != null && daysOfWeek.Count > 0)
        {
            var calendar = new WeeklyCalendar();

            foreach (var dayOfWeek in daysOfWeek)
            {
                calendar.SetDayExcluded(dayOfWeek, false);
            }

            //triggerBuilder.ModifiedByCalendarIntervalTrigger(x => x
            //    .WithInterval(1)
            //    .WithMisfireHandlingInstructionIgnoreMisfires()
            //    .WithSkipDayIfHourDoesNotExist(false)
            //    .WithCalendarIntervalSchedule()
            //    .PreserveHourOfDayAcrossDaylightSavings(true)
            //    .InTimeZone(TimeZoneInfo.Utc)
            //    .SkipDayIfHourDoesNotExist(false)
            //    .CalendarIntervalTrigger(calendar));
        }

        var trigger = triggerBuilder.Build();

        await _scheduler.ScheduleJob(jobDetail, trigger);
    }




    public async Task RescheduleEmailJob(int intervalHours, TimeSpan startTime, TimeSpan endTime, IList<DayOfWeek> daysOfWeek)
    {
        var triggerKey = new TriggerKey("emailTrigger");

        var trigger = await _scheduler.GetTrigger(triggerKey) as ISimpleTrigger;
        if (trigger != null)
        {
            var builder = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .ForJob(trigger.JobKey);

            if (intervalHours > 0)
            {
                builder.WithSimpleSchedule(x => x
                    .WithInterval(TimeSpan.FromHours(intervalHours))
                    .RepeatForever());
            }
            else
            {
                builder.WithSimpleSchedule(x => x
                    .WithInterval(TimeSpan.FromHours(1))
                    .RepeatForever());
            }

            //if (startTime != TimeSpan.MinValue && endTime != TimeSpan.MinValue)
            //{
            //    builder.StartAt(TimeOfDay.HourAndMinuteOfDay(startTime.Hours, startTime.Minutes))
            //        .EndAt(TimeOfDay.HourAndMinuteOfDay(endTime.Hours, endTime.Minutes));
            //}

            //if (daysOfWeek != null && daysOfWeek.Count > 0)
            //{
            //    var daysArray = new bool[7];
            //    foreach (var dayOfWeek in daysOfWeek)
            //    {
            //        daysArray[(int)dayOfWeek] = true;
            //    }
            //    builder.ModifiedByCalendarIntervalTrigger(x => x
            //        .WithInterval(1)
            //        .WithMisfireHandlingInstructionIgnoreMisfires()
            //        .OnDaysOfTheWeek(daysArray));
            //}

            trigger = (ISimpleTrigger)builder.Build();

            await _scheduler.RescheduleJob(triggerKey, trigger);
        }
    }


    public async Task DeleteEmailJob()
    {
        var jobKey = new JobKey("emailJob");

        if (await _scheduler.CheckExists(jobKey))
        {
            await _scheduler.DeleteJob(jobKey);
        }
    }
}
