namespace offlineMeeting.Models.Entity.Pos
{
    public class ExtraDataEntity
    {
        public long? ResultId {  get; set; }
        public string? Image {  get; set; }
        public string? ExtraName { get; set; }
        public int? CoinNumber { get; set; }
        public int? LeftAmount { get; set; }
        public bool Ordered { get; set; } = false;
        public ExtraDataEntity(
            long? resultId,
            string? image,
            string? extraName,
            int? coinNumber,
            int? leftAmount,
            bool ordered
        )
        {
            ResultId = resultId;
            Image = image;
            ExtraName = extraName;
            CoinNumber = coinNumber;
            LeftAmount = leftAmount;
            Ordered = ordered;
        }
    }
}