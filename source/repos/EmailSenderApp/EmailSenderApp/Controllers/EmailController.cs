using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz.Impl.Triggers;
using Quartz;

namespace EmailSenderApp.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IScheduler _scheduler;

        public EmailController(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        [HttpPost("interval")]
        public async Task<IActionResult> SetEmailInterval([FromBody] int interval)
        {
            var triggerKey = new TriggerKey("EmailTrigger");
            var trigger = await _scheduler.GetTrigger(triggerKey) as ISimpleTrigger;

            if (trigger == null)
                return NotFound();

            var updatedTrigger = (ISimpleTrigger)trigger.GetTriggerBuilder()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(interval)
                    .RepeatForever())
                .Build();

            await _scheduler.RescheduleJob(triggerKey, updatedTrigger);

            return Ok();
        }
    }
}
