using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAString.Processing;
using System.IO;
using Rhino;

namespace SAString
{
    public static class Build3dm
    {
        public static void Write3dm(List<ZSegment> Segments, string Filename)
        {
            //System.IO.File.Copy("template.3dm", "out/" + Filename);
            // Rhino.FileIO.File3dm file = new Rhino.FileIO.File3dm();
            StringBuilder sb = new StringBuilder();
            foreach(ZSegment zs in Segments)
            {
                sb.Append(String.Format("Segment[({0:G6},{1:G6},{2:G6}),({3:G6},{4:G6},{5:G6})]\r\n", zs.p1.x, zs.p1.y, zs.p1.z, zs.p2.x, zs.p2.y, zs.p2.z));
            }
            File.WriteAllText("out/result_script.txt", sb.ToString());
        }
        private static Rhino.Geometry.Vector3f CreateVector(ZSegment Segment)
        {
            Rhino.Geometry.Point3d startPoint = new Rhino.Geometry.Point3d(Segment.p1.x, Segment.p1.y, Segment.p1.z);
            Rhino.Geometry.Point3d endPoint = new Rhino.Geometry.Point3d(Segment.p2.x, Segment.p2.y, Segment.p2.z);
            Rhino.Geometry.Vector3d vect = endPoint - startPoint;
            return new Rhino.Geometry.Vector3f((float)vect.X, (float)vect.Y, (float)vect.Z);
        }
        private static Rhino.Geometry.Line BuildLine(ZSegment Segment)
        {
            //    if (v.IsTiny(Rhino.RhinoMath.ZeroTolerance))
            //          return Rhino.Commands.Result.Nothing;
            return new Rhino.Geometry.Line(new Rhino.Geometry.Point3d(Segment.p1.x, Segment.p1.y, Segment.p1.z), new Rhino.Geometry.Point3d(Segment.p2.x, Segment.p2.y, Segment.p2.z));
        }
    }
}
