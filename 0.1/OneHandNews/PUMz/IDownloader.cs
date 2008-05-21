using System;
using System.Collections.Generic;
using System.Text;
using OpenCS.Common.Action;

namespace PUMz
{
    public interface IDownloader : IActionHandler
    {
        void Add(DownloadItem item);
    }
}
