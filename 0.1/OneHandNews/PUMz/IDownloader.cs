using System;
using System.Collections.Generic;
using System.Text;

namespace PUMz
{
    public interface IDownloader
    {
        void Add(DownloadItem item);
    }
}
