using Quartz;

namespace EmailSenderApp
{
    public class SchedulerAccessor : ISchedulerAccessor
    {
        public SchedulerAccessor(IScheduler scheduler) {
            Scheduler = scheduler;  
        }  

        public IScheduler Scheduler { get; }
    }
}
