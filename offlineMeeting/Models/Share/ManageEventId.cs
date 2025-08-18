namespace offlineMeeting.Models.Share
{
    public class ManageEventId
    {
        HttpContext HttpContext;

        public ManageEventId(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        public void SetEventId(string? eventId)
        {
            HttpContext.Response.Cookies.Append("EventId", eventId ?? "0");
        }

        public string GetEventId()
        {
            HttpContext.Request.Cookies.TryGetValue("EventId", out string? eventId);
            return eventId ?? "0";
        }

        public void ClearEventId()
        {
            HttpContext.Response.Cookies.Delete("EventId");
        }
    }
}
