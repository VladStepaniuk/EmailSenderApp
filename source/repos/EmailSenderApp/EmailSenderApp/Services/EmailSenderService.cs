using System.Net;
using EmailSenderApp.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Quartz;

namespace EmailSenderApp.Services
{
    public class EmailSenderService
    {
        private readonly IConfiguration _configuration;

        private static int emailInterval = 1;
        private static TimeSpan startTime = new TimeSpan(9, 0, 0);
        private static TimeSpan endTime = new TimeSpan(23, 0, 0);
        private static List<DayOfWeek> sendDaysOfWeek = new List<DayOfWeek> {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
        private static string periodicity = "hours";
        private static bool isScheduled = false;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void SetEmailInterval(int updatedIntervalValue)
        {
            if (updatedIntervalValue > 0)
            {
                emailInterval = updatedIntervalValue;
            }
        }

        public static void SetEmailTime(TimeSpan start, TimeSpan end)
        {
            startTime = start;
            endTime = end;
        }

        public static int GetEmailInterval()
        {
            return emailInterval;
        }

        public static void SetDaysOfWeek(List<DayOfWeek> daysOfWeek)
        {
            sendDaysOfWeek = daysOfWeek;
        }

        public static List<DayOfWeek> GetDaysOfWeek()
        {
            return sendDaysOfWeek;
        }

        public static string GetPeriodicity()
        {
            return periodicity;
        }

        public static void SetPeriodicity(string period)
        {
            periodicity = period;
        }

        public static EmailJobOptionsModel GetEmailSettings()
        {
            EmailJobOptionsModel model = new EmailJobOptionsModel();

            model.IntervalValue = emailInterval;
            model.StartTime = (int)startTime.TotalHours;
            model.EndTime = (int)endTime.TotalHours;
            model.Periodicity = periodicity;
            model.DaysOfWeek = sendDaysOfWeek;
            model.IsScheduled = isScheduled;

            return model;
        }

        public static void SetEmailSettings(EmailJobOptionsModel model)
        {
            emailInterval = model.IntervalValue;
            startTime = TimeSpan.FromHours(model.StartTime);
            endTime = TimeSpan.FromHours(model.EndTime);
            periodicity = model.Periodicity;
            isScheduled = model.IsScheduled;
        }

        public static bool GetIsScheduled()
        {
            return isScheduled;
        }

        public void SendEmail()
        {
            if (isScheduled) {
                try
                {
                    var now = DateTime.Now;

                    //checks if now is in time range
                    if (now.TimeOfDay < startTime || now.TimeOfDay > endTime)
                        return;

                    //checks if days to send email contains today
                    if (!sendDaysOfWeek.Contains(now.DayOfWeek))
                        return;

                    string smtpServer = _configuration.GetValue<string>("Email:SmtpServer");
                    int smtpPort = _configuration.GetValue<int>("Email:SmtpPort");
                    string email = _configuration.GetValue<string>("Email:Email");
                    string password = _configuration.GetValue<string>("Email:Password");

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Vladyslav Stepaniuk Testing", email));
                    message.To.Add(new MailboxAddress("Recipient", "vladstepaniuk33@gmail.com"));
                    message.Subject = "Test Email Vladyslav Stepaniuk!";

                    message.Body = new TextPart("plain")
                    {
                        Text = "Hello World there!"
                    };

                    using (var client = new SmtpClient())
                    {
                        client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                        client.Authenticate(email, password);
                        client.Send(message);
                        client.Disconnect(true);
                    }


                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while sending email: {ex.Message}");
                }
            }
        }

        
    }
}
