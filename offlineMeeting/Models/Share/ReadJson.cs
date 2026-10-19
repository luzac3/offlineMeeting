using System.Text;

namespace offlineMeeting.Models.Share
{
    public class ReadJson
    {
        public string  Get(string jsonFile, string unicode)
        {
            using StreamReader sr = new StreamReader(jsonFile, Encoding.GetEncoding(unicode));
            return sr.ReadToEnd();
        }
    }
}