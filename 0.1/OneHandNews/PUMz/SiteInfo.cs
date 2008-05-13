using System;
using System.Collections.Generic;
using System.Text;

namespace PUMz
{
    public class SiteInfo
    {
        protected string m_id;
        protected string m_title;
        protected string m_url;
        protected object m_tag;

        public string Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        public object Tag
        {
            get { return m_tag; }
            set { m_tag = value; }
        }

        public SiteInfo()
        {
        }

        public SiteInfo(string title, string url)
        {
            m_title = title;
            m_url = url;
        }

        public SiteInfo(string title, string id, string url)
        {
            m_title = title;
            m_id = id;
            m_url = url;
        }

        public override string ToString()
        {
            return string.Format("Site[id={0}, title={1}, url={2}, tag={3}]", m_id, m_title, m_url, m_tag.ToString());
        }
    }
}
