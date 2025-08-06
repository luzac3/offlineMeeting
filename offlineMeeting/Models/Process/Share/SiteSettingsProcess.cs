using offlineMeeting.Models.Process.DBProcess;

namespace offlineMeeting.Models.Process.Share
{
    public class SiteSettingsProcess : SitesProcess
    {
        private int SiteId = 123;

        public SiteSettingsProcess() {}

        public new System.Text.Json.Nodes.JsonNode? Get()
        {
            string? siteSettings = base.Get(SiteId).SiteSettings;
            var json = System.Text.Json.Nodes.JsonNode.Parse(siteSettings ?? "");

            return json;
        }
    }
}
