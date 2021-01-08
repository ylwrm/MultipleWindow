using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using WeifenLuo.WinFormsUI.Docking;

namespace MultipleWindow
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        public T GetXmlObject<T>(string path) where T : class
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(path);
                T main = ser.Deserialize(reader) as T;
                reader.Close();
                return main;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode)
                return;

            try
            {
                // layout config
                var args = Environment.GetCommandLineArgs();
                string layoutFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Layout.xml");
                if (args.Length > 1)
                {
                    layoutFile = args[1];
                }

                // layout
                Layout layout = GetXmlObject<Layout>(layoutFile);

                // for
                Dictionary<string, DockWindow> windows = new Dictionary<string, DockWindow>();
                foreach (var range in layout.Ranges)
                {
                    try
                    {
                        if (range.DockState != DockState.Unknown)
                        {
                            DockWindow dockWindow4 = new DockWindow(
                                range.Cmd,
                                range.CmdArg,
                                range.ClassName,
                                range.HideRootWindowClassName,
                                range.HideRootWindowText);
                            dockWindow4.Text = range.Name;
                            dockWindow4.Show(dockPanel1, range.DockState);
                            windows.Add(range.Name, dockWindow4);
                        }
                        else
                        {
                            if (windows.ContainsKey(range.Pane))
                            {
                                DockWindow dockWindow = new DockWindow(
                                    range.Cmd,
                                    range.CmdArg,
                                    range.ClassName,
                                    range.HideRootWindowClassName,
                                    range.HideRootWindowText);
                                dockWindow.Text = range.Name;
                                if (range.Proportion == 0)
                                {
                                    dockWindow.Show(windows[range.Pane].Pane, null);
                                }
                                else
                                {
                                    dockWindow.Show(windows[range.Pane].Pane, range.DockAlignment, range.Proportion);
                                }
                                windows.Add(range.Name, dockWindow);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {

            }

            /*

            dockPanel1.DockLeftPortion = 0.5;
            dockPanel1.DockRightPortion = 0.5;
            //DockWindow dockWindow1 = new DockWindow(@"C:\Program Files\Microsoft Office\Office15\WINWORD.EXE", "", @"OpusApp", "MsoSplash");
            DockWindow dockWindow1 = new DockWindow(
                @"C:\Program Files\Microsoft Office\Office15\WINWORD.EXE",
                @"C:\Users\Jerry\Desktop\蚌埠吉的堡天之星幼儿居家身体情况统计表.docx",
                @"_WwG",
                @"OpusApp");
            dockWindow1.Text = "AAA";
            dockWindow1.Show(dockPanel1, DockState.DockLeft);


            DockWindow dockWindow3 = new DockWindow(
                @"C:\Program Files\Internet Explorer\iexplore.exe",
                @"",
                @"Internet Explorer_Server",
                @"");
            dockWindow3.Text = "CCC";
            dockWindow3.Show(dockPanel1, DockState.DockRight);


            //DockWindow dockWindow2 = new DockWindow(@"C:\Program Files\Microsoft Office\Office15\EXCEL.EXE", "", @"FullpageUIHost", "MsoSplash");
            DockWindow dockWindow2 = new DockWindow(
                @"C:\Program Files\Microsoft Office\Office15\EXCEL.EXE",
                @"C:\Users\Jerry\Desktop\XXX.xlsx",
                @"EXCEL7",
                @"XLMAIN");
            dockWindow2.Text = "BBB";
            dockWindow2.Show(dockPanel1, DockState.DockBottom);
            dockWindow2.Show(dockWindow3.Pane, DockAlignment.Left, 0.5);


            //DockWindow dockWindow2 = new DockWindow(@"C:\Program Files\Microsoft Office\Office15\EXCEL.EXE", "", @"FullpageUIHost", "MsoSplash");
            DockWindow dockWindow4 = new DockWindow(
                @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                @"C:\Users\Jerry\Desktop\dist\index.html",
                @"Chrome_WidgetWin_1",
                @"");
            dockWindow4.Text = "TTT";
            dockWindow4.Show(dockPanel1, DockState.DockTop);


            */
        }
    }
}
