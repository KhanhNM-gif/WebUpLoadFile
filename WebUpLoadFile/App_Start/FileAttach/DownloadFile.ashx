
    public class Dowloadfile1: IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition",
                               "attachment; filename=" + "007iocwWly1g5ewko22jgj31151hb7ag.jpg" + ";");
            response.TransmitFile(@"\File\FileUpload\24ffd43b-46e3-403e-aad1-7767d0970142\007iocwWly1g5ewko22jgj31151hb7ag.jpg");
            response.Flush();
            response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
