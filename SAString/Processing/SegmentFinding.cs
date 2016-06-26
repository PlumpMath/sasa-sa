using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace SAString
{
    public static class SegmentFinding
    {   
        public static List<RectSegment> FindSegments(Mat cannyMatrix, List<HesseForm> lines, double thickness)
        {
            List<RectLine> rl = new List<RectLine>();
            foreach (HesseForm hf in lines)
                rl.Add(new RectLine(hf));
            return FindSegments(cannyMatrix, rl, thickness);
        }
        public static List<RectSegment> FindSegments(Mat cannyMatrix, List<RectLine> lines, double thickness)
        {
            var cannyBitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(cannyMatrix);
            List<RectArea> lineAreas = new List<RectArea>();
            List<List<Point>> fullPoints = new List<List<Point>>();
            List<RectSegment> result = new List<RectSegment>();
            for (int i = 0; i < lines.Count; i++) lineAreas.Add(new RectArea());
            for (int i = 0; i < lines.Count; i++) fullPoints.Add(new List<Point>());
            StringBuilder sb = new StringBuilder("x,y\r\n");
            for (int i = 0; i < cannyBitmap.Width; i++)
            {
                for (int j = 0; j < cannyBitmap.Height; j++)
                {
                    var color = cannyBitmap.GetPixel(i, j);
                    if (color.R == 255 && color.G == 255 && color.B == 255)
                        for (int k = 0; k < lines.Count; k++)
                            if (lines[k].PointDistance(i, j) <= thickness)
                            { sb.Append(String.Format("{0},{1}\r\n", i, j)); lineAreas[k].AddComparison(new Point(i, j)); fullPoints[k].Add(new Point(i, j)); }
                }
            }
            System.IO.File.WriteAllText("asdf.csv", sb.ToString());
            for (int i = 0; i < lines.Count; i++)
            {
                sb = new StringBuilder("n, x, y\r\n");
                foreach (Point p1 in fullPoints[i])
                    sb.Append(String.Format("{0},{1},{2}\r\n", i + 1, p1.X, p1.Y));
                System.IO.File.WriteAllText(String.Format("asdf_{0}.csv", i + 1), sb.ToString());
            }
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].b == 0)
                    result.Add(new RectSegment(new Point(-1d * lines[i].c / lines[i].a, lineAreas[i].p1.Y), new Point(-1d * lines[i].c / lines[i].a, lineAreas[i].p2.Y)));
                else
                {
                    bool started = false;
                    Point StartPoint = new Point(), EndPoint = new Point();
                    for (double j = lineAreas[i].p1.X; j <= lineAreas[i].p2.X; j += 0.01)
                    {
                        if (lineAreas[i].InArea(j,lines[i].Substitute(j)))
                        {
                            if (!started) { StartPoint = new Point(j, lines[i].Substitute(j)); started = true; }
                            EndPoint = new Point(j, lines[i].Substitute(j));
                        }
                    }
                    result.Add(new RectSegment(StartPoint, EndPoint));
                }
            }
            return result;
        }
    }
}
