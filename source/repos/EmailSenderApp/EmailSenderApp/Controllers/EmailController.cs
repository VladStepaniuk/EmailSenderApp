using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz.Impl.Triggers;
using Quartz;
using EmailSenderApp.DTO;
using EmailSenderApp.Services;

namespace EmailSenderApp.Controllers
{
    [Route("api/emailsettings")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IScheduler _scheduler;
        private readonly QuartzService _quartzService;

        public EmailController(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        [HttpGet]
        [Route("general")]
        public IActionResult GetEmailSettings()
        {
            var model = EmailSenderService.GetEmailSettings();
            return Ok(model);
        }

        [HttpPost]
        [Route("setgeneral")]
        public IActionResult SetEmailSettings([FromBody] EmailJobOptionsModel model)
        {
            EmailSenderService.SetEmailSettings(model);
            UpdateEmailJobTrigger();
            return Ok(model);
        }

        private void UpdateEmailJobTrigger()
        {
            
            if (EmailSenderService.GetIsScheduled())
            {
                if (!_scheduler.IsShutdown)
                {
                    _scheduler.RescheduleJob(new TriggerKey("emailTrigger"),
                        QuartzService.BuildEmailTrigger(EmailSenderService.GetEmailInterval(), EmailSenderService.GetPeriodicity())).Wait();
                }
                else
                {
                    QuartzService.RestartSchedule(_scheduler);
                    _scheduler.RescheduleJob(new TriggerKey("emailTrigger"),
                        QuartzService.BuildEmailTrigger(EmailSenderService.GetEmailInterval(), EmailSenderService.GetPeriodicity())).Wait();
                }
            }
            else
            {
                //_scheduler.Shutdown().GetAwaiter().GetResult();
            }
        }
    }
}
