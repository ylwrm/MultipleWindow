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
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (DesignMode)
                return;

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode)
                return;
            this.OnResize(EventArgs.Empty);
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

                dockPanel1.DockTopPortion = layout.DockTopPortion;
                dockPanel1.DockBottomPortion = layout.DockBottomPortion;
                dockPanel1.DockRightPortion = layout.DockRightPortion;
                dockPanel1.DockLeftPortion = layout.DockLeftPortion;

                // for
                Dictionary<string, RangeWindow> windows = new Dictionary<string, RangeWindow>();
                foreach (var range in layout.Ranges)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(range.DockState))
                        {
                            Enum.TryParse(range.DockState, out DockState dockState);

                            RangeWindow dockWindow4 = new RangeWindow(
                                range.Cmd,
                                range.CmdArg,
                                range.ClassName,
                                range.UpLevel,
                                range.HideRootWindowClassName,
                                range.HideRootWindowText,
                                range.overtop,
                                range.overbottom,
                                range.overleft,
                                range.overright,
                                range.Sleep);
                            dockWindow4.Text = range.Name;
                            dockWindow4.Show(dockPanel1, dockState);
                            windows.Add(range.Name, dockWindow4);
                        }
                        else
                        {
                            if (windows.ContainsKey(range.Pane))
                            {
                                RangeWindow dockWindow = new RangeWindow(
                                    range.Cmd,
                                    range.CmdArg,
                                    range.ClassName,
                                    range.UpLevel,
                                    range.HideRootWindowClassName,
                                    range.HideRootWindowText,
                                    range.overtop,
                                    range.overbottom,
                                    range.overleft,
                                    range.overright,
                                    range.Sleep);
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
                        this.OnResize(EventArgs.Empty);
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
