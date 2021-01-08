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
                        if (!string.IsNullOrWhiteSpace(range.DockState))
                        {
                            Enum.TryParse(range.DockState, out DockState dockState);

                            DockWindow dockWindow4 = new DockWindow(
                                range.Cmd,
                                range.CmdArg,
                                range.ClassName,
                                range.HideRootWindowClassName,
                                range.HideRootWindowText,
                                range.overtop,
                                range.overbottom,
                                range.overleft,
                                range.overright);
                            dockWindow4.Text = range.Name;
                            dockWindow4.Show(dockPanel1, dockState);
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
                                    range.HideRootWindowText,
                                    range.overtop,
                                    range.overbottom,
                                    range.overleft,
                                    range.overright);
                                dockWindow.Text = range.Name;
                                if (range.Proportion == 0)
                                {
                                    dockWindow.Show(windows[range.Pane].Pane, null);
                                }
                                else
                                {
                                    Enum.TryParse(range.DockAlignment, out DockAlignment dockAlignment);
                                    dockWindow.Show(windows[range.Pane].Pane, dockAlignment, range.Proportion);
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
        }
    }
}
