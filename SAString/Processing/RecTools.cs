using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace SAString
{
    public static class Tools
    {
        public static bool isInRegion(OpenCvSharp.Point checkPoint, OpenCvSharp.Point regionP1, OpenCvSharp.Point regionP2)
        {
            int minX = regionP1.X > regionP2.X ? regionP2.X : regionP1.X,
                maxX = regionP1.X < regionP2.X ? regionP2.X : regionP1.X,
                minY = regionP1.Y > regionP2.Y ? regionP2.Y : regionP1.Y,
                maxY = regionP1.Y < regionP2.Y ? regionP2.Y : regionP1.Y;
            if (minX <= checkPoint.X && checkPoint.X <= maxX && minY <= checkPoint.Y && checkPoint.Y <= maxY) return true;
            else return false;
        }
        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        //A simple outer product method showing which direction two lines are placed.
        //clockwise: >0, counter-clockwise: <0
        public static int ccw (int x1, int y1, int x2, int y2, int x3, int y3)
        {
            int v = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);
            if (v > 0) return 1;
            if (v < 0) return -1;
            else return 0;
        }
        public static bool cross (int ax, int ay, int bx, int by, int cx, int cy, int dx, int dy)
        {
            if (ccw(ax, ay, bx, by, cx, cy) * ccw(ax, ay, bx, by, dx, dy) < 0 &&
                ccw(cx, cy, dx, dy, ax, ay) * ccw(cx, cy, dx, dy, bx, by) < 0) return true;
            return false;
        }
        public static OpenCvSharp.Point findIntercept(double ax, double ay, double bx, double by, double cx, double cy, double dx, double dy)
        {
            return new OpenCvSharp.Point(
                ((ax * by - ay * bx) * (cx - dx) - (cx * dy - cy * dx) * (ax - bx)) / ((ax - bx) * (cy - dy) - (ay - by) * (cx - dx)),
                ((ax * by - ay * bx) * (cy - dy) - (cx * dy - cy * dx) * (ay - by)) / ((ax - bx) * (cy - dy) - (ay - by) * (cx - dx))
                );
        }
    }
}
