using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace MultipleWindow
{
    /// <summary>
    /// 应用程序容器控件
    /// </summary>
    public partial class ControlApplication : UserControl
    {
        public string CMD { get; set; }
        public int overtop { get; set; }
        public int overbottom { get; set; }
        public int overleft { get; set; }
        public int overright { get; set; }
        public Func<Win32Window, bool> Predicate { get; set; }
        public Win32Window Window { get; set; }

        public ControlApplication()
        {
            InitializeComponent();
        }
        public ControlApplication(string CMD = null, Func<Win32Window, bool> Predicate = null, int overtop = 0, int overbottom = 0, int overleft = 0, int overright = 0)
        {
            this.CMD = CMD;
            this.Predicate = Predicate;

            this.overtop = overtop;
            this.overbottom = overbottom;
            this.overleft = overleft;
            this.overright = overright;

            this.Predicate = Predicate;
            InitializeComponent();
        }
        public void OpenApplication(string hideRootWindowClass = null, string hideRootWindowText = null)
        {
            if (!string.IsNullOrWhiteSpace(CMD))
            {
                try
                {
                    Process process = Process.Start(CMD);
                }
                catch (Exception)
                {
                }
            }

            Win32WindowEvents.WaitForWindowWhere(Predicate, (Process process, Win32Window window, Win32WindowEvents.EventTypes type) =>
            {
                try
                {
                    this.Controls.Clear();
                    this.Window = window;

                    // hide form
                    if (!string.IsNullOrWhiteSpace(hideRootWindowClass) || !string.IsNullOrWhiteSpace(hideRootWindowText))
                    {
                        var root = window;
                        while (root != null)
                        {
                            if (
                                (hideRootWindowClass == null || root.ClassName.Contains(hideRootWindowClass))
                                &&
                                (hideRootWindowText == null || root.Text.Contains(hideRootWindowText))
                            )
                            {
                                root.Visible = false;
                                break;
                            }
                            root = root.Parent;
                        }
                    }

                    // use wanted
                    window.Parent = new Win32Window(this.Handle);

                    window.Style = window.Style & (~WinAPI.WindowStyles.WS_CAPTION);
                    resize();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        [DllImport("user32")]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            resize();
        }

        private void resize()
        {
            if (Window != null)
            {
                Window.Height = this.Height + overtop + overbottom;
                Window.Width = this.Width + overleft + overright;
                Window.Pos_X = this.Location.X - overleft;
                Window.Pos_Y = this.Location.Y - overtop;
            }
        }
    }
}
