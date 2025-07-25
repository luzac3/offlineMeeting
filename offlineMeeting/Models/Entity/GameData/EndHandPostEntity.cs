namespace offlineMeeting.Models.Entity.GameData
{
    public class EndHandPostEntity
    {
        public int EventNumber { get; set; }
        public int HoraKind { get; set; }
        public int Parent { get; set; }
        public int? ToUser { get; set; }
        public bool IsPao { get; set; }

        public EndHandPostEntity() {
            IsPao = false;
        }
    }
}
