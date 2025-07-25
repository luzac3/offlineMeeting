namespace offlineMeeting.Models.Entity.GameData
{
    public class RecordedMediaPostEntity
    {
        public int EventNumber { get; set; }
        public int GameNumber { get; set; }
        public int HandNumber { get; set; }
        public int HandSubNumber { get; set; }
        public int? MediaNumber { get; set; } = null;
        public int MediaCount { get; set; }
        public int MediaMinutes { get; set; }
        public string? Base64 { get; set; }
        public bool IsVideo { get; set; }
        public bool IsChangeFile { get; set; } = false;
    }
}