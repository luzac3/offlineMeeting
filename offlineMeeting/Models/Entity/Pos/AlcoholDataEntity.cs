namespace offlineMeeting.Models.Entity.Pos
{
    public class AlcoholDataEntity
    {
        public long? AlcoholNumber {  get; set; }
        public string? AlcoholName { get; set;}
        public int? Amount { get; set; }
        public string? Brewery { get; set; }
        public string? Prefectures { get; set; }
        public string? GenryoMai { get; set; }
        public string? SpeciallyDesignated { get; set; }
        public int? RicePolishingRatio { get; set; }
        public decimal? SakeMeterValue { get; set; }
        public decimal? AcidRate { get; set; }
        public decimal? Abv { get; set; }
        public int? Rarity { get; set; }
        public AlcoholDataEntity() { }
        public AlcoholDataEntity(
            long? alcoholNumber,
            string? alcoholName,
            int? amount,
            string? brewery,
            string? prefectures,
            string? genryoMai,
            string? speciallyDesignated,
            int? ricePolishingRatio,
            decimal? sakeMeterValue,
            decimal? acidRate,
            decimal? abv,
            int? rarity
        )
        {
            AlcoholNumber = alcoholNumber;
            AlcoholName = alcoholName;
            Amount = amount;
            Brewery = brewery;
            Prefectures = prefectures;
            GenryoMai = genryoMai;
            SpeciallyDesignated = speciallyDesignated;
            RicePolishingRatio = ricePolishingRatio;
            SakeMeterValue = sakeMeterValue;
            AcidRate = acidRate;
            Abv = abv;
            Rarity = rarity;
        }
    }
}