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
        [XmlAttribute]
        public double DockTopPortion { get; set; }

        [XmlAttribute]
        public double DockRightPortion { get; set; }

        [XmlAttribute]
        public double DockLeftPortion { get; set; }

        [XmlAttribute]
        public double DockBottomPortion { get; set; }
        
        [XmlArray]
        public List<Range> Ranges { get; set; }
    }

    public class Range
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string DockState { get; set; }

        [XmlAttribute]
        public string Pane { get; set; }

        [XmlAttribute]
        public string DockAlignment { get; set; }

        [XmlAttribute]
        public double Proportion { get; set; }


        [XmlAttribute]
        public string Cmd { get; set; }

        [XmlAttribute]
        public string WorkingDirectory { get; set; }

        [XmlAttribute]
        public string CmdArg { get; set; }

        [XmlAttribute]
        public string ClassName { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string HideRootWindowClassName { get; set; }

        [XmlAttribute]
        public string HideRootWindowText { get; set; }

        [XmlAttribute]
        public int UpLevel { get; set; }

        [XmlAttribute]
        public int Sleep { get; set; }



        [XmlAttribute]
        public int overtop { get; set; }
        [XmlAttribute]
        public int overbottom { get; set; }
        [XmlAttribute]
        public int overleft { get; set; }
        [XmlAttribute]
        public int overright { get; set; }
    }
}
