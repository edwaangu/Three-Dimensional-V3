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
                oldPoints[i].X -= _cam.pos.X;
                oldPoints[i].Y -= _cam.pos.Y;
                oldPoints[i].Z -= _cam.pos.Z;

                float distXZ = Convert.ToSingle(Math.Sqrt(Math.Pow(oldPoints[i].X, 2) + Math.Pow(oldPoints[i].Z, 2)));
                float dirXZ = Convert.ToSingle(Math.Atan2(oldPoints[i].Z - _cam.pos.Z, oldPoints[i].X - _cam.pos.X));
                float distXYZ = Convert.ToSingle(Math.Sqrt(Math.Pow(distXZ, 2) + Math.Pow(oldPoints[i].Y, 2)));
                float dirY = Convert.ToSingle(Math.Atan2(oldPoints[i].Y - _cam.pos.Y, dirXZ));
                oldPoints[i].X = Convert.ToSingle(Math.Cos(dirXZ - _cam.direction.X) * distXZ);
                oldPoints[i].Z = Convert.ToSingle(Math.Sin(dirXZ - _cam.direction.X) * distXZ);
                oldPoints[i].Y = Convert.ToSingle(Math.Sin(dirY - _cam.direction.Y) * distXYZ);
            }
            
        }
        
        public bool ShouldBeOnScreen(Camera _cam, Object _obj, PointF _resolution)
        {
            TrianglePointsReset();
            TranslateByObject(_obj);
            TranslateByCamera(_cam);

            float toRad = Convert.ToSingle(180 / Math.PI);
            float Z0 = Convert.ToSingle((_resolution.X / 2) / Math.Tan((_cam.fov / 2) / toRad));

            saidZ = 0;
            for (int i = 0; i < points.Length; i++)
            {
                float theZ = oldPoints[i].Z - Z0;
                float theX = oldPoints[i].X * (Z0 / (Z0 + theZ));
                float theY = oldPoints[i].Y * (Z0 / (Z0 + theZ));
                if (theX > 10000)
                {
                    theX = 10000;
                }
                if (theX < -10000)
                {
                    theX = -10000;
                }
                if (theY > 10000)
                {
                    theY = 10000;
                }
                if (theY < -10000)
                {
                    theY = -10000;
                }
                Console.WriteLine($"Z of point {i} is {oldPoints[i].Z}");
                if (oldPoints[i].Z > 0)
                {
                    if (theX > -_resolution.X / 2 && theX < _resolution.X / 2 && theY > -_resolution.Y / 2 && theY < _resolution.Y / 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public PointF[] PointsOnScreen(Camera _cam, Object _obj, PointF _resolution, float thenum)
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
                if(theX > 10000)
                {
                    theX = 10000;
                }
                if(theX < -10000)
                {
                    theX = -10000;
                }
                if (theY > 10000)
                {
                    theY = 10000;
                }
                if (theY < -10000)
                {
                    theY = -10000;
                }
                thePoints[i].X = theX;
                thePoints[i].Y = theY;
                saidZ += Convert.ToSingle(Math.Sqrt(Math.Pow(points[i].X + _obj.pos.X - _cam.pos.X, 2) + Math.Pow(points[i].Y + _obj.pos.Y - _cam.pos.Y, 2) + Math.Pow(points[i].Z + _obj.pos.Z - _cam.pos.Z, 2)));
            }
            saidZ /= 3;
            Console.WriteLine($"saidZ of tri {thenum} is {saidZ}");

            return thePoints;
        }
    }
}
