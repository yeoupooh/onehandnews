using System;
using System.Collections.Generic;
using System.Text;
using OpenCS.Common.Action;

namespace PUMz.Actions
{
    public class AddDownloadItemAction : IAction
    {
        private DownloadItem m_item;

        public DownloadItem Item
        {
            get { return m_item; }
            set { m_item = value; }
        }

        public AddDownloadItemAction(DownloadItem item)
        {
            m_item = item;
        }
    }
}
