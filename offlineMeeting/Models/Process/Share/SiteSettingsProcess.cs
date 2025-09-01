using offlineMeeting.Models.Process.DBProcess;

namespace offlineMeeting.Models.Process.Share
{
    public class SiteSettingsProcess : SitesProcess
    {
        public SiteSettingsProcess() {}

        public new System.Text.Json.Nodes.JsonNode? Get(int siteId)
        {
            string? siteSettings = base.Get(siteId).SiteSettings;
            var json = System.Text.Json.Nodes.JsonNode.Parse(siteSettings ?? "");

            return json;
        }
    }
}
