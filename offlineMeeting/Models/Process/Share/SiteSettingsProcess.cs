using offlineMeeting.Models.Process.DBProcess;

namespace offlineMeeting.Models.Process.Share
{
    public class SiteSettingsProcess : SitesProcess
    {
        private int SakeSiteId = 123;

        public SiteSettingsProcess(int sakeSiteId) {
            SakeSiteId = sakeSiteId;
        }

        public new System.Text.Json.Nodes.JsonNode? Get()
        {
            string? siteSettings = base.Get(SakeSiteId).SiteSettings;
            var json = System.Text.Json.Nodes.JsonNode.Parse(siteSettings ?? "");

            return json;
        }
    }
}
