namespace offlineMeeting.Models.Entity.EventList
{
    public class RegisterEventPostEntity
    {
        public int EventNumber { get; set; }
        public string? EventName { get; set; }
        public DateTime? EventDate { get; set; }
        public string? TsvUsers { get; set; }
        public string? UsersCd { get; set; }

        public RegisterEventPostEntity() { }
    }
}
