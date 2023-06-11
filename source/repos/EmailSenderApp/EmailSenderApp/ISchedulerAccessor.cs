using Quartz;

namespace EmailSenderApp
{
    public interface ISchedulerAccessor
    {
        IScheduler Scheduler {get;}
    }
}
