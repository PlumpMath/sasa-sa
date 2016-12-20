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
        private static double ModelHeight = 100, ClusterThreshold=2,PointClusterThreshold=10;
        public static List<ZPoint> CalcZPoints (List<RectSegment> Segments, List<Point> Points)
        {
            List<ZPoint> Result = new List<ZPoint>();
            List<List<Point>> GroupedPoints = new List<List<Point>>();
            List<Point> ClusteredPoints = new List<Point>();
            foreach(RectSegment rs in Segments)
            {
                Points.Add(rs.p1);
                Points.Add(rs.p2);
            }
            foreach(Point p in Points)
            {
                if (GroupedPoints.Count == 0)
                {
                    GroupedPoints.Add(new List<Point>());
                    GroupedPoints[0].Add(p);
                }
                else
                {
                    bool flag = false;
                    for (int i = 0; i < GroupedPoints.Count; i++)
                    {
                        if(GroupedPoints[i][0].DistanceTo(p)<=PointClusterThreshold)
                        {
                            GroupedPoints[i].Add(p);
                            flag = true;
                            break;
                        }
                    }
                    if(!flag)
                    {
                        GroupedPoints.Add(new List<Point>());
                        GroupedPoints[GroupedPoints.Count-1].Add(p);
                    }
                }
            }
            foreach(List<Point> pl in GroupedPoints)
            {
                Point AddedPoints = new Point();
                foreach(Point p in pl)
                {
                    AddedPoints.X += p.X;
                    AddedPoints.Y += p.Y;
                }
                AddedPoints.X /= pl.Count;
                AddedPoints.Y /= pl.Count;
                ClusteredPoints.Add(AddedPoints);
            }
            // 여기에 Z축을 추가하는 소스를 만들어 넣으세요.
            // Points 안에 들어가있는 점들에 대해서 Z값을 추가하면 됨.
            // Result에 점을 추가하려면 Result.Add(new ZPoint(x,y,z)) 이렇게
            // List<RectSegment> Segments : 스트링아트를 구성하는 선분들의 리스트
            //   - RectSegment.p1, RectSegment.p2: 선분의 각 끝점 (Point)
            // List<Point> Points : 교점들의 리스트 (Point)

            //Find Intersections
            List<RectLine> SegToLine = new List<RectLine>();
            foreach (RectSegment seg in Segments)
                SegToLine.Add(seg.ToLine());
            foreach(Point point in ClusteredPoints)
            {
                List<int> IncludedLines = new List<int>();
                int Count = 0; double ZSum = 0;
                for(int i=0;i<SegToLine.Count;i++)
                {
                    if (!Segments[i].ToArea().InArea(point,ClusterThreshold)) continue;
                    if (SegToLine[i].PointDistance(point.X,point.Y)<=ClusterThreshold)
                    {
                        Count++;
                        double val = CalcZ(Segments[i].p1, Segments[i].p2, point);
                        ZSum += val;
                        IncludedLines.Add(i);
                    }
                }
                Result.Add(new ZPoint(point.X, point.Y, ZSum / Count, Result.Count, IncludedLines));
            }
            return Result;
        }
        private static double CalcZ(Point StartPoint, Point EndPoint, Point CalcPoint)
        {
            double distStart = StartPoint.DistanceTo(CalcPoint), distEnd = EndPoint.DistanceTo(CalcPoint), distBetween = StartPoint.DistanceTo(EndPoint);
            if (distStart < distBetween / 2) return ModelHeight * (distStart / distBetween);
            else return ModelHeight * (distEnd / distBetween);
        }
    }
}
