using Quartz;
using System.Net.Mail;
using System.Net;
using EmailSenderApp.Services;

namespace EmailSenderApp
{
    public class EmailJob:IJob
    {
        private readonly EmailSenderService _emailSenderService;

        public EmailJob(EmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        public Task Execute(IJobExecutionContext context)
        {

            _emailSenderService.SendEmail();
            return Task.CompletedTask;
        
        }
    }
}
