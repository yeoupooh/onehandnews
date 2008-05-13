using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenCS.RBP.WinForms;

namespace OneHandNews
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            richBrowserControlMain.WebBrowserDockContentFactory = new WinFormsWebBrowserDockContentFactory();

            this.Load += new EventHandler(FormMain_Load);
            this.FormClosing += new FormClosingEventHandler(FormMain_FormClosing);
        }

        void FormMain_Load(object sender, EventArgs e)
        {
            richBrowserControlMain.LoadPlugins();
        }

        void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            richBrowserControlMain.UnloadPlugins();
        }
    }
}