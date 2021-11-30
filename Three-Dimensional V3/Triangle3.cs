using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Dimensional_V3
{
    internal class Triangle3
    {
        public Point3[] points = new Point3[3];

        public Point3[] oldPoints = new Point3[3];

        public Triangle3(Point3[] _points)
        {
            points = _points;
        }

        void TrianglePointsReset()
        {
            oldPoints = new Point3[]{ points[0], points[1], points[2]};
        }

        void TranslateByObject(Object _obj)
        {
            for (int i = 0; i < oldPoints.Length; i++)
            {
                oldPoints[i].X += _obj.pos.X;
                oldPoints[i].Y += _obj.pos.Y;
                oldPoints[i].Z += _obj.pos.Z;
            }
        }

        void TranslateByCamera(Camera _cam)
        {
            for (int i = 0; i < oldPoints.Length; i++)
            {
                oldPoints[i].X += _cam.pos.X;
                oldPoints[i].Y += _cam.pos.Y;
                oldPoints[i].Z += _cam.pos.Z;

            }
        }
    }
}
