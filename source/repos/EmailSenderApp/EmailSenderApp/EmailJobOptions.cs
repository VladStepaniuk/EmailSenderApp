namespace EmailSenderApp
{
    public class EmailJobOptions
    {
        public int IntervalHours { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public IList<DayOfWeek> DaysOfWeek { get; set; }
    }
}
