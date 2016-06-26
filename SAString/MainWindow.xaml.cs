using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace SAString
{

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        bool rndCls=false;
        int hough_n=150;
        double clean_a=15, clean_b=0.15, thickness=1.0d;
        string fileLoc;
        Mat src, cny, result, res2, seg, pts;
        BitmapSource oriSrc, cannySrc, dstSrc, cleanSrc, segSrc, ptSrc;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void doWork()
        {
            rndCls = (checkBox_Copy.IsChecked??false);
            src = new Mat(fileLoc, ImreadModes.GrayScale);
            cny = new Mat();
            Cv2.Canny(src, cny, 50, 200);
            result = new Mat(fileLoc); res2 = new Mat(fileLoc); seg = new Mat(fileLoc); pts = new Mat(fileLoc);
            var lines = Cv2.HoughLines(cny, 1, System.Math.PI / 180, hough_n);
            StringBuilder sb = new StringBuilder(String.Format("Rho,Theta\r\n"));
            List<HesseForm> li = new List<HesseForm>();
            Random rnd = new Random();
            foreach (LineSegmentPolar lsp in lines)
            {
                double a = Math.Cos(lsp.Theta), b = Math.Sin(lsp.Theta);
                double x0 = a * lsp.Rho, y0 = b * lsp.Rho;
                OpenCvSharp.Point p1 = new OpenCvSharp.Point(x0 - 1000 * b, y0 + 1000 * a), p2 = new OpenCvSharp.Point(x0 + 1000 * b, y0 - 1000 * a);
                Cv2.Line(result, p1, p2, rndCls ? new Scalar(rnd.Next(127, 255), rnd.Next(127, 255), rnd.Next(127, 255)) : new Scalar(0, 0, 255), 2);
                sb.Append(String.Format("{0},{1}\r\n", lsp.Rho, lsp.Theta));
                li.Add(new HesseForm(lsp.Theta, lsp.Rho));
            }
            if (Settings.SaveAsCSV) File.WriteAllText("pre.csv", sb.ToString());
            li = CleanLines.Clean(li, clean_a, clean_b);
            List<RectLine> ast = new List<RectLine>();
            foreach (HesseForm lsp in li)
            {
                double a = Math.Cos(lsp.Theta), b = Math.Sin(lsp.Theta);
                double x0 = a * lsp.Rho, y0 = b * lsp.Rho;
                OpenCvSharp.Point p1 = new OpenCvSharp.Point(x0 - 1000 * b, y0 + 1000 * a), p2 = new OpenCvSharp.Point(x0 + 1000 * b, y0 - 1000 * a);
                Cv2.Line(res2, p1, p2, rndCls?new Scalar(rnd.Next(127,255), rnd.Next(127, 255), rnd.Next(127, 255)):new Scalar(0,0,255), 3);
                ast.Add(new RectLine(lsp));
            }
            var segments = SegmentFinding.FindSegments(cny, ast, thickness);
            foreach (RectSegment rs in segments)
                Cv2.Line(seg, rs.p1, rs.p2, rndCls ? new Scalar(rnd.Next(127, 255), rnd.Next(127, 255), rnd.Next(127, 255)) : new Scalar(0, 0, 255), 3);
            foreach (RectSegment rs in segments)
            {
                Cv2.Circle(pts, rs.p1, 2, new Scalar(255, 0, 0), 10);
                Cv2.Circle(pts, rs.p2, 2, new Scalar(255, 0, 0), 10);
            }
            cannySrc = OpenCvSharp.Extensions.BitmapSourceConverter.ToBitmapSource(cny);
            dstSrc = OpenCvSharp.Extensions.BitmapSourceConverter.ToBitmapSource(result);
            cleanSrc = OpenCvSharp.Extensions.BitmapSourceConverter.ToBitmapSource(res2);
            segSrc = OpenCvSharp.Extensions.BitmapSourceConverter.ToBitmapSource(seg);
            ptSrc = OpenCvSharp.Extensions.BitmapSourceConverter.ToBitmapSource(pts);
            cannyImage.Source = cannySrc;
            dstImage.Source = dstSrc;
            cleanedImage.Source = cleanSrc;
            segmentizedImage.Source = segSrc;
            pointsImage.Source = ptSrc;
        }
        #region UI Methods
        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() ?? false)
            {
                fileLoc = ofd.FileName;
                BitmapImage l = new BitmapImage();
                l.BeginInit();
                l.UriSource = new Uri(fileLoc);
                l.EndInit();
                oriSrc = l;
                oriImage.Source = oriSrc;
                src = new Mat(fileLoc, ImreadModes.GrayScale);
            }
        }

        private void points_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new OpenCvSharp.Window("points image", pts);
        }

        private void segmentized_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new OpenCvSharp.Window("segment image", seg);
        }

        private void ori_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new OpenCvSharp.Window("src image", src);
        }
        private void canny_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new OpenCvSharp.Window("canny image", cny);
        }
        private void aValue_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                Int32.TryParse(aValue.Text, out hough_n);
                Double.TryParse(aValue_Copy.Text, out clean_a);
                Double.TryParse(aValue_Copy1.Text, out clean_b);
                Double.TryParse(aValue_Copy2.Text, out thickness);
            }
            catch (Exception) { }
        }
        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location.Substring(0,System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf('\\')));
        }
        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.SaveAsCSV = false;
        }
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Settings.SaveAsCSV = true;
        }
        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            doWork();
        }
        private void dst_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new OpenCvSharp.Window("dst image", result);
        }
        private void cleaned_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new OpenCvSharp.Window("cleaned image", res2);
        }
        #endregion
    }
}
