using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;


public static class Common
{
    public static string GetClientIpAddress(this HttpRequestMessage request)
    {
        if (request.Properties.ContainsKey("MS_HttpContext"))
        {
            dynamic ctx = request.Properties["MS_HttpContext"];
            if (ctx != null)
            {
                return ctx.Request.UserHostAddress;
            }
        }

        if (request.Properties.ContainsKey("System.ServiceModel.Channels.RemoteEndpointMessageProperty"))
        {
            dynamic remoteEndpoint = request.Properties["System.ServiceModel.Channels.RemoteEndpointMessageProperty"];
            if (remoteEndpoint != null)
            {
                return remoteEndpoint.Address;
            }
        }

        return null;
    }

    public static string GetIPAddress(this HttpRequest request)
    {
        string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (string.IsNullOrEmpty(ipAddress)) ipAddress = request.ServerVariables["REMOTE_ADDR"];

        return ipAddress;
    }

    public static string ToObject<T>(this JObject data, string key, out T t)
    {
        t = default(T);
        try
        {
            if (string.IsNullOrWhiteSpace(key)) return "key is null or empty @JObjectToOject";
            if (data[key] == null) return string.Format("data[key] is null (key: {0})", key);

            t = data[key].ToObject<T>();

            //var type = t.GetType();
            //if (type == null) return "type == null";

            //var ps = type.GetProperties();
            //if (ps == null) return "ps == null";

            //foreach (var itemPS in ps)
            //{
            //    object value = itemPS.GetType().is.GetValue(oSource, null);
            //    if (value != null)
            //    {
            //        string info = value.ToString();

            //        object[] attrs = itemPS.GetCustomAttributes(typeof(SourceObjectAttribute), false);
            //        if (attrs.Length > 0)
            //            if (attrs[0] is SourceObjectAttribute)
            //            {
            //                SourceObjectAttribute attr = (SourceObjectAttribute)attrs[0];
            //                if (attr != null)
            //                    if (attr.FormatDate == null || value == null) info = attr.DisplayName + ": " + value;
            //                    else info = attr.DisplayName + ": " + DateTime.Parse(value.ToString()).ToString(attr.FormatDate);
            //            }

            //        ltInfo.Add(info);
            //    }
            //}

            return "";
        }
        catch (Exception ex)
        {
            return string.Format("ToObject (key: {0}): {1}", key, ex.ToString());
        }
    }
    public static string ToString(this JObject data, string key, out string str)
    {
        str = "";
        try
        {
            if (string.IsNullOrWhiteSpace(key)) return "key is null or empty @JObjectToString";
            if (data[key] == null) return string.Format("data[key] is null (key: {0})", key);

            str = data[key].ToString();

            //str = HttpUtility.HtmlEncode(str);
            return "";
        }
        catch (Exception ex)
        {
            return string.Format("ToString (key: {0}): {1}", key, ex.ToString());
        }
    }
    public static string StringToList(string str, out List<int> ltInt)
    {
        ltInt = new List<int>();

        try
        {
            if (string.IsNullOrEmpty(str)) return "";

            string[] strInt = str.Split(',');
            foreach (var item in strInt) ltInt.Add(int.Parse(item));

            return "";
        }
        catch (Exception ex)
        {
            return string.Format("ToList (key: {0}): {1}", str, ex.ToString());
        }
    }
    public static string ToNumber(this JObject data, string key, out int number)
    {
        number = 0;
        try
        {
            string msg = data.ToString(key, out string str);
            if (msg.Length > 0) return msg;

            number = int.Parse(str);
            return "";
        }
        catch (Exception ex)
        {
            return string.Format("ToNumber (key: {0}): {1}", key, ex.ToString());
        }
    }

    public static string ToGuid(this JObject data, string key, out Guid guid)
    {
        guid = Guid.Empty;
        try
        {
            string msg = data.ToString(key, out string str);
            if (msg.Length > 0) return msg;

            guid = Guid.Parse(str);
            return "";
        }
        catch (Exception ex)
        {
            return string.Format("ToGuid (key: {0}): {1}", key, ex.ToString());
        }
    }
    public static string ToDateTime(this JObject data, string key, out DateTime date)
    {
        date = DateTime.Now;
        try
        {
            string msg = data.ToString(key, out string str);
            if (msg.Length > 0) return msg;

            date = DateTime.Parse(str);
            return "";
        }
        catch (Exception ex)
        {
            return string.Format("ToDateTime (key: {0}): {1}", key, ex.ToString());
        }
    }
    public static string ToBool(this JObject data, string key, out bool b)
    {
        b = true;
        try
        {
            string msg = data.ToString(key, out string str);
            if (msg.Length > 0) return msg;

            b = bool.Parse(str);
            return "";
        }
        catch (Exception ex)
        {
            return string.Format("ToBool (key: {0}): {1}", key, ex.ToString());
        }
    }

    public static List<DateTime> GetAllDayInCurrentWeek()
    {
        var now = DateTime.Now;
        var currentDay = now.DayOfWeek;
        int days = (int)currentDay;
        DateTime sunday = now.AddDays(-days);
        List<DateTime> listDayInCurrentWeek = Enumerable.Range(0, 7)
            .Select(d => sunday.AddDays(d))
            .ToList();

        return listDayInCurrentWeek;
    }
}