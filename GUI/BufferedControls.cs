using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class BufferedControls
    {
  
        public class BufferedSplitContainer : SplitContainer
        {
            public BufferedSplitContainer()
            {
                this.DoubleBuffered = true;
            }
        }

        public class BufferedFlowLayoutPanel : FlowLayoutPanel
        {
            public BufferedFlowLayoutPanel()
            {
                this.DoubleBuffered = true;
            }
        }
    }
}
