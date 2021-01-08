using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Threading;

namespace MultipleWindow
{
    public partial class DockWindow : DockContent
    {
        public string Cmd { get; set; }
        public string CmdArg { get; set; }
        public string ClassName { get; set; }
        public string HideRootWindowClassName { get; set; }
        public string HideRootWindowText { get; set; }

        public int overtop { get; set; }
        public int overbottom { get; set; }
        public int overleft { get; set; }
        public int overright { get; set; }

        private Process process = null;
        public DockWindow()
        {
            InitializeComponent();
            CloseButton = false;
            CloseButtonVisible = false;
        }
        public DockWindow(
            string cmd,
            string cmdArg,
            string className,
            string hideRootWindowClassName,
            string hideRootWindowText,
            int overtop = 30,
            int overbottom = 8,
            int overleft = 8,
            int overright = 8
            ) :base()
        {
            this.Cmd = cmd;
            this.CmdArg = cmdArg;
            this.ClassName = className;
            this.HideRootWindowClassName = hideRootWindowClassName;
            this.HideRootWindowText = hideRootWindowText;


            this.overtop = overtop;
            this.overbottom = overbottom;
            this.overleft = overleft;
            this.overright = overright;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode)
                return;
            // Process
            var now = DateTime.Now;
            try
            {
                process = Process.Start(Cmd, CmdArg);
            }
            catch (Exception)
            {
                throw;
            }

            // Main
            var mainpre = new Func<Win32Window, bool>(t =>
            {
                bool after = false;
                try
                {
                    after = t.ClassName.Contains(ClassName) && t.Process.StartTime >= now;
                }
                catch (Exception)
                {
                }
                return after;
            });
            ControlApplication main = new ControlApplication(null, mainpre, overtop, overbottom, overleft, overright);
            this.Controls.Add(main);
            main.Dock = DockStyle.Fill;
            main.OpenApplication(HideRootWindowClassName, HideRootWindowText);
            this.Disposed += DockWindow_Disposed;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
        }
        private void DockWindow_Disposed(object sender, EventArgs e)
        {
            try
            {
                process.Kill();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
