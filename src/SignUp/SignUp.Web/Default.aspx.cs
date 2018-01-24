using SignUp.Web.Logging;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.UI;

namespace SignUp.Web
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var homepageUrl = ConfigurationManager.AppSettings["Homepage.Url"];
            if (!string.IsNullOrEmpty(homepageUrl))
            {
                Log.Info($"Loading homepage content from: {homepageUrl}");
                Response.Clear();
                var request = HttpWebRequest.Create(homepageUrl);
                var response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var html = reader.ReadToEnd();
                    Response.Write(html);
                }
                Response.End();
            }
        }
    }
}