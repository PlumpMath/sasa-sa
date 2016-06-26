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
    }
}
