using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace SAString
{
    public static class SegmentFinding
    {
        static double distThreshold = 10;
        public static List<RectSegment> FindSegments(Mat CannyMatrix, List<RectLine> Lines, double Thickness, int ClusterThreshold)
        {
            var cannyBitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(CannyMatrix);
            List<RectArea> lineAreas = new List<RectArea>();
            List<List<Point>> fullPoints = new List<List<Point>>();
            List<RectSegment> result = new List<RectSegment>();
            for (int i = 0; i < Lines.Count; i++)
            {
                lineAreas.Add(new RectArea());
                fullPoints.Add(new List<Point>());
            }
            for (int i = 0; i < cannyBitmap.Width; i++)
            {
                for (int j = 0; j < cannyBitmap.Height; j++)
                {
                    var color = cannyBitmap.GetPixel(i, j);
                    if (color.R == 255 && color.G == 255 && color.B == 255)
                    {
                        for (int k = 0; k < Lines.Count; k++)
                        {
                            if (Lines[k].PointDistance(i, j) <= Thickness)
                            {
                                //lineAreas[k].AddComparison(new Point(i, j));
                                fullPoints[k].Add(new Point(i, j));
                            }
                        }
                    }
                }
            }
            //cluster points
            for(int i=0;i<Lines.Count;i++)
            {
                List<List<Point>> groups = new List<List<Point>>();
                groups.Add(new List<Point>());
                for (int j = 0; j < fullPoints[i].Count; j++)
                {
                    if(groups[groups.Count-1].Count==0)
                    {
                        groups[groups.Count - 1].Add(fullPoints[i][j]);
                    }
                    else if(fullPoints[i][j].DistanceTo(groups[groups.Count-1][groups[groups.Count-1].Count-1])<distThreshold)
                    {
                        groups[groups.Count - 1].Add(fullPoints[i][j]);
                    }
                    else
                    {
                        groups.Add(new List<Point>());
                        groups[groups.Count - 1].Add(fullPoints[i][j]);
                    }
                }
                foreach(List<Point> group in groups)
                {
                    if(group.Count>=ClusterThreshold)
                    {
                        foreach (Point pt in group)
                            lineAreas[i].AddComparison(pt);  
                    }
                }
            }
            StringBuilder allsb = new StringBuilder("n,x,y\r\n");
            for (int i = 0; i < Lines.Count; i++)
            {
                StringBuilder sb = new StringBuilder("n,x,y\r\n");
                foreach (Point p1 in fullPoints[i])
                {
                    allsb.Append(String.Format("{0},{1},{2}\r\n", i + 1, p1.X, p1.Y));
                    sb.Append(String.Format("{0},{1},{2}\r\n", i + 1, p1.X, p1.Y));
                }
                System.IO.File.WriteAllText(String.Format("out/line_{0}.csv", i + 1), sb.ToString());
            }
            System.IO.File.WriteAllText("out/allpoints.csv", allsb.ToString());
            for (int i = 0; i < Lines.Count; i++)
            {
                if (Lines[i].b == 0)
                    result.Add(new RectSegment(new Point(-1d * Lines[i].c / Lines[i].a, lineAreas[i].p1.Y), new Point(-1d * Lines[i].c / Lines[i].a, lineAreas[i].p2.Y)));
                else
                {
                    bool started = false;
                    Point StartPoint = new Point(), EndPoint = new Point();
                    for (double j = lineAreas[i].p1.X; j <= lineAreas[i].p2.X; j += 0.01)
                    {
                        if (lineAreas[i].InArea(j,Lines[i].Substitute(j)))
                        {
                            if (!started) { StartPoint = new Point(j, Lines[i].Substitute(j)); started = true; }
                            EndPoint = new Point(j, Lines[i].Substitute(j));
                        }
                    }
                    result.Add(new RectSegment(StartPoint, EndPoint));
                }
            }
            return result;
        }
    }
}
