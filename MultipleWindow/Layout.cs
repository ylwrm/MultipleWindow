using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WeifenLuo.WinFormsUI.Docking;

namespace MultipleWindow
{
    /// <summary>
    /// 
    /// </summary>
    public class Layout
    {
        public List<Range> Ranges { get; set; }
    }

    public class Range
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public DockState DockState { get; set; }

        [XmlAttribute]
        public string Pane { get; set; }

        [XmlAttribute]
        public DockAlignment DockAlignment { get; set; }

        [XmlAttribute]
        public double Proportion { get; set; }


        [XmlAttribute]
        public string Cmd { get; set; }

        [XmlAttribute]
        public string CmdArg { get; set; }

        [XmlAttribute]
        public string ClassName { get; set; }

        [XmlAttribute]
        public string HideRootWindowClassName { get; set; }

        [XmlAttribute]
        public string HideRootWindowText { get; set; }
    }
}
