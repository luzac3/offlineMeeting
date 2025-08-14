namespace offlineMeeting.Models.Process.DBProcess.Pos
{
    public class GetBinariesProcess : BinariesProcess
    {
        public GetBinariesProcess() { }

        public string? GetImage(string guid)
        {
            byte[]? bin = base.GetBin(guid);
            if(bin == null)
            {
                return null;
            }
            else
            {
                return Convert.ToBase64String(bin);
            }
        }
    }
}