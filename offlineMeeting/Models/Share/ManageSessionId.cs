namespace offlineMeeting.Models.Share
{
    public class ManageSessionId
    {
        HttpContext HttpContext;

        public ManageSessionId(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        public void SetSessionId(string sessionId)
        {
            HttpContext.Response.Cookies.Append("SessionId", sessionId);
        }

        public string? GetSessionId()
        {
            HttpContext.Request.Cookies.TryGetValue("SessionId", out string? sessionId);
            return string.IsNullOrEmpty(sessionId) ? null : sessionId;
        }

        public void ClearSessionId()
        {
            HttpContext.Response.Cookies.Delete("SessionId");
        }
    }
}
