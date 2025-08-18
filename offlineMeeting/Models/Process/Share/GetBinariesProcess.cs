using offlineMeeting.Models.Process.DBProcess;

namespace offlineMeeting.Models.Process.Share
{
    public class GetBinariesProcess : BinariesProcess
    {
        public GetBinariesProcess() { }

        public string? GetImage(string guid)
        {
            byte[]? bin = GetBin(guid);
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