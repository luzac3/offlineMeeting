namespace offlineMeeting.Models.Share
{
    public class ManageUserCd
    {
        HttpContext HttpContext;

        public ManageUserCd(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        public void SetUserCd(string? userCd)
        {
            HttpContext.Response.Cookies.Append("UserCd", userCd ?? "0");
        }

        public string GetUserCd()
        {
            HttpContext.Request.Cookies.TryGetValue("UserCd", out string? userCd);
            return userCd ?? "0";
        }

        public void ClearUserCd()
        {
            HttpContext.Response.Cookies.Delete("UserCd");
        }
    }
}
