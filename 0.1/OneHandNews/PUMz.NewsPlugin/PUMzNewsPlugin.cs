﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenCS.RBP;
using OpenCS.Common.Action;
using WeifenLuo.WinFormsUI.Docking;

namespace PUMz.NewsPlugin
{
    public class PUMzNewsPlugin : BaseRbpPlugin
    {
        public override void Deinit()
        {

        }

        public override ActionResult HandleAction(IAction action)
        {
            return ActionResult.NotHandled;
        }

        public override void Init()
        {
            DCPUMzNews dc = new DCPUMzNews();
            dc.RichBrowserControl = RichBrowserControl;
            dc.Show(RichBrowserControl.DockPanel, DockState.DockLeft);
            dc.ActionHandler = RichBrowserControl;
        }

        public override string Title
        {
            get { return "PUMz.News"; }
        }

        public override Version Version
        {
            get { return new Version("0.1.1.1"); }
        }
    }
}
