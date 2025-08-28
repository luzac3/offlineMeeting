namespace offlineMeeting.Models.Entity.Pos
{
    public class AlcoholDataEntity
    {
        public long? ResultId {  get; set; }
        public string? Image {  get; set; }
        public string? AlcoholName { get; set; }
        public string? Abbreviation { get; set; }
        public string? Brewery { get; set; }
        public string? Prefectures { get; set; }
        public string? GenryoMai { get; set; }
        public string? SpeciallyDesignated { get; set; }
        public int? RicePolishingRatio { get; set; }
        public decimal? SakeMeterValue { get; set; }
        public decimal? AcidRate { get; set; }
        public decimal? Abv { get; set; }
        public int? Rarity { get; set; }
        public int? CoinNumber { get; set; }
        public int? LeftAmount { get; set; }
        public bool Ordered { get; set; } = false;
        public AlcoholDataEntity(
            long? resultId,
            string? image,
            string? alcoholName,
            string? abreviation,
            string? brewery,
            string? prefectures,
            string? genryoMai,
            string? speciallyDesignated,
            int? ricePolishingRatio,
            decimal? sakeMeterValue,
            decimal? acidRate,
            decimal? abv,
            int? rarity,
            int? coinNumber,
            int? leftAmount,
            bool ordered
        )
        {
            ResultId = resultId;
            Image = image;
            AlcoholName = alcoholName;
            Abbreviation = abreviation;
            Brewery = brewery;
            Prefectures = prefectures;
            GenryoMai = genryoMai;
            SpeciallyDesignated = speciallyDesignated;
            RicePolishingRatio = ricePolishingRatio;
            SakeMeterValue = sakeMeterValue;
            AcidRate = acidRate;
            Abv = abv;
            Rarity = rarity;
            CoinNumber = coinNumber;
            LeftAmount = leftAmount;
            Ordered = ordered;
        }
    }
}