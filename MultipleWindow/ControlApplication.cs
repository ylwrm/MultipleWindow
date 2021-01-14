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
        public int overtop { get; set; }
        public int overbottom { get; set; }
        public int overleft { get; set; }
        public int overright { get; set; }
        public Func<Win32Window, bool> Predicate { get; set; }
        public Win32Window Window { get; set; }

        public Win32Window WindowOldParent { get; set; }

        public ControlApplication()
        {
            InitializeComponent();
        }
        public ControlApplication(Func<Win32Window, bool> Predicate = null, int overtop = 0, int overbottom = 0, int overleft = 0, int overright = 0)
        {
            this.Predicate = Predicate;

            this.overtop = overtop;
            this.overbottom = overbottom;
            this.overleft = overleft;
            this.overright = overright;

            this.Predicate = Predicate;
            InitializeComponent();
        }
        public void CloseApplication()
        {
            try
            {
                try
                {
                    Window.Parent = WindowOldParent;
                }
                catch (Exception)
                {
                }
                Window.Process.CloseMainWindow();
                Window.Process.Kill();
                Window.Process.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void OpenApplication(int upLevel = 0)
        {
            Win32WindowEvents.WaitForWindowWhere(Predicate, (Process process, Win32Window window, Win32WindowEvents.EventTypes type) =>
            {
                try
                {
                    this.Controls.Clear();
                    for (int i = 0; i < upLevel; i++)
                    {
                        window = window.Parent;
                    }
                    this.Window = window;

                    // hide
                    var root = window.Parent;
                    while (root.ClassName != "")
                    {
                        try
                        {
                            root.Visible = false;
                            root = root.Parent;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                    // use wanted
                    WindowOldParent = window.Parent;
                    var handle = new Win32Window(this.Handle);
                    window.Parent = handle;
                    //while (true)
                    //{
                    //    try
                    //    {
                    //        if (window.Parent.hWnd == this.Handle)
                    //        {
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            Thread.Sleep(1000);
                    //            window.Parent = handle;
                    //        }
                    //    }
                    //    catch (Exception)
                    //    {

                    //    }
                    //}

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
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            resize();
        }

        private void resize()
        {
            try
            {
                if (Window != null)
                {
                    Window.Parent = new Win32Window(this.Handle);

                    var h = this.Height + overtop + overbottom;
                    var w = this.Width + overleft + overright;
                    var x = this.Location.X - overleft;
                    var y = this.Location.Y - overtop;
                    if (
                        h != Window.Height ||
                        w != Window.Width ||
                        x != Window.Pos_X ||
                        y != Window.Pos_Y
                        )
                    {
                        Window.Height = this.Height + overtop + overbottom;
                        Window.Width = this.Width + overleft + overright;
                        Window.Pos_X = this.Location.X - overleft;
                        Window.Pos_Y = this.Location.Y - overtop;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
