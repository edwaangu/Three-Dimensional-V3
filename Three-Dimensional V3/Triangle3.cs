using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Three_Dimensional_V3
{
    internal class Triangle3
    {
        public Point3[] points = new Point3[3];

        public Point3[] oldPoints = new Point3[3];

        public float saidZ;

        public Triangle3(Point3[] _points)
        {
            points = _points;
        }

        void TrianglePointsReset()
        {
            oldPoints = new Point3[3];
            for (int i = 0; i < oldPoints.Length; i++)
            {
                oldPoints[i] = new Point3(0 + points[i].X, 0 + points[i].Y, 0 + points[i].Z);
            }
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

        public PointF[] PointsOnScreen(Camera _cam, Object _obj, PointF _resolution)
        {
            PointF[] thePoints = new PointF[3];

            TrianglePointsReset();
            TranslateByObject(_obj);
            TranslateByCamera(_cam);

            float toRad = Convert.ToSingle(180 / Math.PI);
            float Z0 = Convert.ToSingle((_resolution.X / 2) / Math.Tan((_cam.fov / 2) / toRad));

            saidZ = 0;
            for(int i = 0;i < thePoints.Length;i++)
            {
                float theZ = oldPoints[i].Z - Z0;
                float theX = oldPoints[i].X * (Z0 / (Z0 + theZ));
                float theY = oldPoints[i].Y * (Z0 / (Z0 + theZ));
                thePoints[i].X = theX;
                thePoints[i].Y = theY;
                saidZ += theZ;
            }
            saidZ /= 3;

            return thePoints;
        }
    }
}
