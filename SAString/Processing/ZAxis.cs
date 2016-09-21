using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAString.Processing
{
    public static class ZAxis
    {
        public static List<ZPoint> CalcZPoints (List<RectSegment> Segments, List<Point> Points)
        {
            List<ZPoint> Result = new List<ZPoint>();
            //===============================================================================
            //
            // 여기에 Z축을 추가하는 소스를 만들어 넣으세요.
            // Points 안에 들어가있는 점들에 대해서 Z값을 추가하면 됨.
            // Result에 점을 추가하려면 Result.Add(new ZPoint(x,y,z)) 이렇게
            // List<RectSegment> Segments : 스트링아트를 구성하는 선분들의 리스트
            //   - RectSegment.p1, RectSegment.p2: 선분의 각 끝점 (Point)
            // List<Point> Points : 교점들의 리스트 (Point)

            /* 샘플 코드 */
            foreach (Point p in Points)
            {
                Result.Add(new ZPoint(p.X, p.Y, 123));
            }


            //===============================================================================
            return Result;
        }
    }
}
