using System;
using System.Collections.Generic;
using System.Text;

namespace PUMz
{
    public class DownloadItem : SiteInfo
    {
        private string m_filename;

        public string Filename
        {
            get { return m_filename; }
            set { m_filename = value; }
        }

        public DownloadItem(string title, string url, string filename)
            : base(title, url)
        {
            m_filename = filename;
        }

        public override string ToString()
        {
            return string.Format("{0}[id={1}, title={2}, url={3}, tag={4}, filename={5}]", this.GetType().FullName, m_id, m_title, m_url, m_tag.ToString(), m_filename);
        }
    }
}
