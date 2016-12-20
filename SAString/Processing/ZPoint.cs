using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAString.Processing
{
    public class ZPoint
    {
        public ZPoint (double X, double Y, double Z, int id)
        {
            x = X; y = Y; z = Z; ID = id;
            IncludedLines = new List<int>();
        }
        public ZPoint (double X, double Y, double Z, int id, List<int> incl_lines)
        {
            x = X; y = Y; z = Z; ID = id;
            IncludedLines = incl_lines;
        }
        public List<int> IncludedLines { get; set; }  
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public int ID { get; set; }
    }
}
