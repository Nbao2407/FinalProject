using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVT.Helper
{
    class BufferedControls
    {
  
        public class BufferedSplitContainer : SplitContainer
        {
            public BufferedSplitContainer()
            {
                DoubleBuffered = true;
            }
        }

        public class BufferedFlowLayoutPanel : FlowLayoutPanel
        {
            public BufferedFlowLayoutPanel()
            {
                DoubleBuffered = true;
            }
        }
    }
}
