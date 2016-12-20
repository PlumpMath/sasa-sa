using System;
using OpenCvSharp;

namespace SAString
{
    public class RectLine
    {
        //ax+by+c=0
        public RectLine(double A, double B, double C)
        {
            a = A; b = B; c = C;
        }
        public RectLine(HesseForm hf)
        {
            a = Math.Cos(hf.Theta);
            b = Math.Sin(hf.Theta);
            c = -1 * hf.Rho;
        }
        public double PointDistance(int x, int y)
        {
            return Math.Abs(a * x + b * y + c) / Math.Sqrt(a * a + b * b);
        }
        public void ChangeSlope(double val) { a += b * val * -1; }
        public void ChangeSegment(double val) { c += b * val * -1; }
        public double Substitute(double n) { return -1 * (a * n + c) / b; }
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }
        public double Slope { get { return -1 * a / b; } }
        public double YSegment { get { return -1 * c / b; } }
    }
    public class RectSegment
    {
        public RectSegment(Point P1, Point P2)
        {
            p1 = P1; p2 = P2;
        }
        public Point p1 { get; set; }
        public Point p2 { get; set; }
        public RectLine ToLine()
        {
            return new RectLine(p2.Y-p1.Y, p1.X-p2.X, p2.X*p1.Y-p1.X*p2.Y);
        }
        public RectArea ToArea()
        {
            RectArea ra = new RectArea();
            ra.AddComparison(p1);
            ra.AddComparison(p2);
            return ra;
        }
    }
    public class RectArea
    {
        private static readonly int INF = 999999999;
        public RectArea()
        {
            p1 = new Point(1 * INF, 1 * INF);
            p2 = new Point(-1 * INF, -1 * INF);
        }
        public Point p1 = new Point(); //small end
        public Point p2 = new Point(); //large end
        public void AddComparison(Point p)
        {
            if (p.X < p1.X) p1.X = p.X;
            if (p.X > p2.X) p2.X = p.X;
            if (p.Y < p1.Y) p1.Y = p.Y;
            if (p.Y > p2.Y) p2.Y = p.Y;
        }
        public bool InArea(double x, double y)
        {
            return InArea(new Point(x, y));
        }
        public bool InArea(Point p, double Error)
        {
            if (p1.X-Error <= p.X && p.X <= p2.X+Error && p1.Y-Error <= p.Y && p.Y <= p2.Y+Error) return true;
            else return false;
        }
        public bool InArea(Point p)
        {
            if (p1.X <= p.X && p.X <= p2.X && p1.Y <= p.Y && p.Y <= p2.Y) return true;
            else return false;
        }
    }
}
