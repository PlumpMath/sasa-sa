using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAString.Processing
{
    public static class MakeSegments
    {
        public static List<ZSegment> Make(List<ZPoint> pt, int lineCount)
        {
            List<ZSegment> Result = new List<ZSegment>();
            for(int i=0;i<lineCount;i++)
            {
                List<ZPoint> myPoints = new List<ZPoint>();
                foreach (ZPoint p in pt)
                    if (p.IncludedLines.Contains(i))
                        myPoints.Add(p);
                myPoints.OrderBy(p => p.x).ThenBy(p => p.y);
                for (int j = 0; j < myPoints.Count - 1; j++)
                    Result.Add(new ZSegment(myPoints[j], myPoints[j + 1], i));
            }
            return Result;
        }
    }
}
