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
using System.Collections;

namespace MultipleWindow
{
    public partial class RangeWindow : DockContent
    {
        public string Cmd { get; set; }
        public string CmdArg { get; set; }
        public string WorkingDirectory { get; set; }
        public string ClassName { get; set; }
        public string Title { get; set; }
        public int UpLevel { get; set; }
        public string HideRootWindowClassName { get; set; }
        public string HideRootWindowText { get; set; }

        public int overtop { get; set; }
        public int overbottom { get; set; }
        public int overleft { get; set; }
        public int overright { get; set; }
        public int sleep { get; set; }

        private Process process = null;
        public RangeWindow()
        {
            InitializeComponent();
            CloseButton = false;
            CloseButtonVisible = false;
        }
        public RangeWindow(
            string cmd,
            string cmdArg,
            string workingDirectory,
            string className,
            string title,
            int upLevel,
            string hideRootWindowClassName,
            string hideRootWindowText,
            int overtop = 30,
            int overbottom = 8,
            int overleft = 8,
            int overright = 8,
            int sleep = 0
            ) : base()
        {
            this.Cmd = cmd;
            this.CmdArg = cmdArg;
            this.WorkingDirectory = workingDirectory;
            this.ClassName = className;
            this.Title = title;
            this.UpLevel = upLevel;
            this.HideRootWindowClassName = hideRootWindowClassName;
            this.HideRootWindowText = hideRootWindowText;


            this.overtop = overtop;
            this.overbottom = overbottom;
            this.overleft = overleft;
            this.overright = overright;

            this.sleep = sleep;
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
                var evs = Environment.GetEnvironmentVariables();
                foreach (DictionaryEntry ev in evs)
                {
                    Cmd = (Cmd ?? "").Replace("${" + ev.Key.ToString() + "}", ev.Value.ToString());
                    CmdArg = (CmdArg ?? "").Replace("${" + ev.Key.ToString() + "}", ev.Value.ToString());
                    WorkingDirectory = (WorkingDirectory ?? "").Replace("${" + ev.Key.ToString() + "}", ev.Value.ToString());
                }
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    WorkingDirectory = WorkingDirectory,
                    FileName = Cmd,
                    Arguments = CmdArg
                };
                process = Process.Start(psi);
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
                    after = (
                        (string.IsNullOrWhiteSpace(ClassName) || t.ClassName.Contains(ClassName)) &&
                        (string.IsNullOrWhiteSpace(Title) || t.Text.Contains(Title))
                    )
                    && t.Process.StartTime >= now;
                }
                catch (Exception)
                {
                }
                return after;
            });
            ControlApplication main = new ControlApplication(mainpre, overtop, overbottom, overleft, overright);
            this.Controls.Add(main);
            main.Dock = DockStyle.Fill;
            main.OpenApplication(UpLevel);
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
