using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OpenCS.RBP;
using OpenCS.Common.Action;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using OpenCS.Common;
using OpenCS.Common.Logging;

namespace PUMz.NewsPlugin
{
    public partial class DCPUMzNews : DockContent, ILoggable
    {
        private IRichBrowserControl m_rbp;
        private IActionHandler m_ah;
        private ILogger m_logger;

        public IActionHandler ActionHandler
        {
            get { return m_ah; }
            set { m_ah = value; }
        }

        public IRichBrowserControl RichBrowserControl
        {
            get { return m_rbp; }
            set { m_rbp = value; }
        }

        public DCPUMzNews()
        {
            InitializeComponent();

            InitTreeView(treeViewMain);
        }

        private void InitTreeView(TreeView tv)
        {
            tv.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tv_NodeMouseDoubleClick);
        }

        void tv_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is SiteInfo)
            {
                m_rbp.Navigate((e.Node.Tag as SiteInfo).Url);
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            if (sender == toolStripButtonStart)
            {
                StartClipping(treeViewMain);
            }
            else if (sender == toolStripButtonDown)
            {
                DownArticles(treeViewMain);
            }
        }

        private void WriteHeader(StreamWriter sw)
        {
            sw.WriteLine(@"<html>");
            sw.WriteLine(@"<head>");
            sw.WriteLine(@"<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
            sw.WriteLine(@"</head>");
            sw.WriteLine(@"<body>");
            sw.WriteLine(@"<pre>");
        }

        private void WriteFooter(StreamWriter sw)
        {
            sw.WriteLine(@"</pre>");
            sw.WriteLine(@"</body>");
            sw.WriteLine(@"</html>");
        }


        private void DownArticles(TreeView tv)
        {
            if (toolStripTextBoxDownFolder.Text == "")
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    toolStripTextBoxDownFolder.Text = dlg.SelectedPath;
                    DownArticles(tv);
                }
            }
            else
            {
                foreach (TreeNode siteTN in tv.Nodes)
                {
                    SiteInfo site = siteTN.Tag as SiteInfo;
                    if (site != null)
                    {
                        string siteFolder = toolStripTextBoxDownFolder.Text + @"\" + site.Id;
                        if (Directory.Exists(siteFolder) == false)
                        {
                            Directory.CreateDirectory(siteFolder);
                        }

                        foreach (TreeNode catTN in siteTN.Nodes)
                        {
                            CategoryInfo cat = catTN.Tag as CategoryInfo;
                            if (cat != null)
                            {
                                string catFolder = siteFolder + @"\" + cat.Id;
                                string catFile = catFolder + @"\index.html";
                                if (Directory.Exists(catFolder) == false)
                                {
                                    Directory.CreateDirectory(catFolder);
                                }

                                using (StreamWriter swCat = new StreamWriter(catFile))
                                {
                                    WriteHeader(swCat);

                                    int i = 0;
                                    foreach (TreeNode artTN in catTN.Nodes)
                                    {
                                        ArticleInfo art = artTN.Tag as ArticleInfo;
                                        if (art != null)
                                        {
                                            //string artContent = GetWebContent(art.Url);
                                            string artContent = PUMzUtils.GetWebContent(art.Url);
                                            string artFile = string.Format(@"{0}\{1}.html", catFolder, i);
                                            using (StreamWriter sw = new StreamWriter(artFile))
                                            {
                                                string grabbed = StringUtils.GrabString(m_logger, artContent, Properties.Resources.TAG_S_ARTICLE, Properties.Resources.TAG_E_ARTICLE, true, true);
                                                WriteHeader(sw);
                                                sw.Write(grabbed);
                                                WriteFooter(sw);
                                            }
                                            swCat.WriteLine(string.Format("- <a href='{0}.html'>{1}</a><br>", i, art.Title));
                                            i++;
                                        }
                                    }

                                    WriteFooter(swCat);
                                }
                            }
                        }
                    }
                }
            }
        }

        private TreeNode AddSite(TreeNodeCollection col, SiteInfo site)
        {
            TreeNode siteTN = new TreeNode();
            siteTN.Text = site.Title;
            siteTN.Tag = site;

            col.Add(siteTN);

            return siteTN;
        }

        private void ClipArticles(TreeNode catTN)
        {
            CategoryInfo cat = catTN.Tag as CategoryInfo;
            if (cat == null)
            {
                return;
            }

            // get article list
            //string content = GetWebContent(cat.Url);
            string content = PUMzUtils.GetWebContent(cat.Url);
            Regex re = new Regex(Properties.Resources.RE_LIST_URL);
            MatchCollection mc = re.Matches(content);
            foreach (Match m in mc)
            {
                if (m.Groups.Count == 3)
                {
                    // found art
                    bool found = false;
                    foreach (TreeNode art in catTN.Nodes)
                    {
                        if (art.Tag is ArticleInfo && (art.Tag as ArticleInfo).Url == m.Groups[1].Value)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found == false)
                    {
                        ArticleInfo art = new ArticleInfo();
                        art.Title = m.Groups[2].Value;
                        art.Url = m.Groups[1].Value;

                        TreeNode artTN = AddSite(catTN.Nodes, art);
                    }
                }
            }
        }

        private void StartClipping(TreeView tv)
        {
            tv.Nodes.Clear();

            SiteInfo site = new SiteInfo();
            site.Title = "MK";
            site.Id = "mk";
            site.Url = "http://news.mk.co.kr/";

            TreeNode siteTN = AddSite(tv.Nodes, site);

            AddSite(siteTN.Nodes, new CategoryInfo("경제,금융", "30000016", @"http://news.mk.co.kr/main/main_eco.php"));
            AddSite(siteTN.Nodes, new CategoryInfo("증권", "30000019", @"http://news.mk.co.kr/main/main_stock.php"));
            AddSite(siteTN.Nodes, new CategoryInfo("부동산", "30000020", @"http://news.mk.co.kr/main/main_land.php"));

            foreach (TreeNode catTN in siteTN.Nodes)
            {
                ClipArticles(catTN);
            }

        }

        #region ILoggable 멤버

        public ILogger Logger
        {
            set { m_logger = value; }
        }

        #endregion
    }
}