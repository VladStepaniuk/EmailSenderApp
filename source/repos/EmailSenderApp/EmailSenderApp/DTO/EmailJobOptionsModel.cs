namespace EmailSenderApp.DTO
{
    public class EmailJobOptionsModel
    {
        public int IntervalValue { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string Periodicity { get; set; }
        public IList<DayOfWeek> DaysOfWeek { get; set; }
        public bool IsScheduled { get; set; }

    }
}
