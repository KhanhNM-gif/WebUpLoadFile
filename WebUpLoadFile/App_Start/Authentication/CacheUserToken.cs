using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BSS;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Web.Http;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for Cache
/// </summary>
public static class CacheUserToken
{
    const int HOUR_TIMEOUT_TOKEN = 4;
    const int HOUR_TIMEOUT_TOKEN_REMEMBER = 24 * 7;

    public static string CreateToken(int UserID, bool IsRememberPassword, out string JWT)
    {
        string msg = "";

        object payload = new
        {
            UserID = UserID,
            ExpiredDate = DateTime.Now.AddHours(IsRememberPassword ? HOUR_TIMEOUT_TOKEN_REMEMBER : HOUR_TIMEOUT_TOKEN),
            CreateDate = DateTime.Now
        };

        JWT = JsonWebToken.Encode(payload, ConfigurationManager.AppSettings["PrivateKey"], JwtHashAlgorithm.RS256);

        return msg;
    }

    //public static string GetToken()
    //{
    //    char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    //    byte[] data = new byte[15];
    //    using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
    //    {
    //        crypto.GetNonZeroBytes(data);
    //    }

    //    StringBuilder result = new StringBuilder(15);
    //    foreach (byte b in data) result.Append(chars[b % (chars.Length)]);

    //    return result.ToString();
    //}
    //public static Result GetResultUserToken(HttpRequestMessage request)
    //{
    //    UserToken UserToken;
    //    return GetResultUserToken(request, out  UserToken);
    //}
    //public static Result GetResultUserToken(HttpRequestMessage request, out UserToken UserToken)
    //{
    //    string msg = GetUserToken(request, out  UserToken);
    //    if (msg.Length > 0) return Log.ProcessError(msg).ToResult(-1);
    //    return Result.ResultOk;
    //}
    //public static string GetUserToken(HttpRequestMessage request, out UserToken UserToken)
    //{
    //    UserToken = null;

    //    string token = "";
    //    if (request.Headers.Contains("Authorization"))
    //        token = request.Headers.GetValues("Authorization").First();
    //    else
    //        return ("Header không chứa key Authorization (có value là Token đăng nhập)").ToMessageForUser();

    //    return GetUserToken(token, out  UserToken);
    //}
    //public static Result GetResultUserToken()
    //{
    //    UserToken UserToken;
    //    return GetResultUserToken(out  UserToken);
    //}
    public static Result GetResultUserToken(out UserToken UserToken)
    {
        string msg = GetUserToken(out UserToken);
        if (msg.Length > 0) return Log.ProcessError(msg).ToResult(-1);
        return Result.ResultOk;
    }
    public static string GetUserToken(out UserToken UserToken)
    {
        UserToken = null;

        HttpContext context = HttpContext.Current;
        if (context == null) return "HttpContext.Current == null".ToMessageForUser();

        HttpRequest request = context.Request;
        if (request == null) return "request == null".ToMessageForUser();

        string token = "";
        if (request.Headers["Authorization"] != null) token = request.Headers["Authorization"];
        else return ("Header không chứa key Authorization (có value là Token đăng nhập)").ToMessageForUser();

        return GetUserToken(token, out UserToken);
    }
    public static string GetUserToken(string Token, out UserToken UserToken)
    {
        string msg = "";


        if (vDB_Token == null || vDB_Token.Count() == 0) return "Không tồn tại token".ToMessageForUser();

        UserToken = vDB_Token.First();
        if (UserToken.ExpiredDate < DateTime.Now) return "Token đã hết hạn".ToMessageForUser();

        UserToken.ExpiredDate = DateTime.Now.AddHours(UserToken.IsRememberPassword ? HOUR_TIMEOUT_TOKEN_REMEMBER : HOUR_TIMEOUT_TOKEN);

        if ((DateTime.Now - UserToken.TimeUpdateExpiredDateToDB).TotalMinutes > 3)
        {
            UserToken.TimeUpdateExpiredDateToDB = DateTime.Now;
            msg = UserToken.UpdateExpiredDate(UserToken.ID, UserToken.ExpiredDate);
            if (msg.Length > 0) return msg;
        }

        return "";
    }
    public static string Logout([FromBody] JObject data, out string urlLogout)
    {
        string msg = "";
        urlLogout = "";
        UserToken UserToken;
        msg = GetUserToken(out UserToken);
        if (msg.Length > 0) return msg;


        msg = UserToken.Delete(UserToken.ID);
        if (msg.Length > 0) return msg;

        bool UsingSSO = bool.Parse(ConfigurationManager.AppSettings["UsingSSO"]);
        if (!UsingSSO) return "";

        string redirectUri = data["redirect_uri"].ToString();
        if (string.IsNullOrWhiteSpace(redirectUri)) return "Chưa truyền param redirect_uri vào";

        string ApiLogout = ConfigurationManager.AppSettings["ApiLogout"];
        if (string.IsNullOrWhiteSpace(ApiLogout)) return "Chưa cấu hình ApiLogout";

        urlLogout = ApiLogout + "?id_token_hint=" + UserToken.JsonWebToken + "&state=" + UserToken.SessionState + "&post_logout_redirect_uri=" + redirectUri;

        return msg;
    }
}