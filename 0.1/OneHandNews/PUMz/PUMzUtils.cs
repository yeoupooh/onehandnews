using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using OpenCS.Common.Logging;

namespace PUMz
{
    public class PUMzUtils
    {
        public static string GetWebContent(ILogger logger, string url)
        {
            string content = null;

            using (WebClient wc = new WebClient())
            {
                try
                {
                    content = wc.DownloadString(url);
                }
                catch (WebException ex)
                {
                    logger.Error(ex.Message);
                    logger.Debug(ex.ToString());
                }
            }

            return content;
        }

        public static string GetWebContent(string url)
        {
            return GetWebContent(new ConsoleLogger(), url);
        }
    }
}
