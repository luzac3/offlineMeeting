namespace offlineMeeting.Models.Entity.GameData
{
    public static class horaKindDictionary
    {

        public static Dictionary<int, string> digetHoraKindList()
        {
            return new Dictionary<int, string> {
                {0, "----"},
                {1,"ツモ"},
                { 2,"ロン"},
                { 30,"流局" },
                { 31,"四風連打"},
                { 32,"九種九牌"},
                { 33,"四家立直"},
                { 34,"四開槓"},
                { 21,"ダブロン"},
                { 22,"トリロン"}
            };
        }
    }
}
