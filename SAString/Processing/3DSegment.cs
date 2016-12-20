using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAString.Processing
{
    public class ZSegment
    {
        public ZPoint p1, p2;
        public int ParentLine;
        public ZSegment(ZPoint P1, ZPoint P2, int parent)
        {
            p1 = P1; p2 = P2;
            ParentLine = parent;
        }
    }
}
